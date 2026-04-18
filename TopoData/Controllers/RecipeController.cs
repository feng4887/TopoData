using auDAServer;
using auDASLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hiServer.Controllers
{
    public class RecipeController : ControllerBase
    {
        /// <summary>
        /// 下载配方到设备
        /// http://127.0.0.1:8083/api/Recipe/DownloadRecipe
        /// </summary>
        /// <param name="RecipeName">配方名</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Recipe/DownloadRecipe")]
        public async Task<IActionResult> DownloadRecipe([FromBody] string RecipeName)
        {
            if (string.IsNullOrWhiteSpace(RecipeName))
                return NotFound("Recipe Name is empty.");

            equipment_recipe recipe = null;
            if (master_recipe.Instance == null || master_recipe.Instance.equipment_recipes == null)
                return NotFound("Recipe is empty.");

            try
            {

                recipe = master_recipe.Instance?.equipment_recipes?
                    .FirstOrDefault(r => string.Equals(r.equipment_id, RecipeName, StringComparison.OrdinalIgnoreCase)
                                      || string.Equals(r.equipment_name, RecipeName, StringComparison.OrdinalIgnoreCase));

                if (recipe == null)
                    return NotFound($"Recipe '{RecipeName}' not found.");

                if (recipe.recipe_status != RecipeStatus.InUse)
                    return BadRequest($"Recipe '{RecipeName}' Status is not Active.");


                // 构建待写入项（根据 recipe.recipe_data 的 tag_name 与 value）
                var writeItems = new List<WriteValue>();
                foreach (var pd in recipe.recipe_data ?? Enumerable.Empty<recipe_data>())
                {
                    if (string.IsNullOrWhiteSpace(pd.tag_name))
                        continue;

                    // 尝试从 TagPool 获取地址 / 数据类型 / 通道信息
                    string cannelId = string.Empty;
                    string address = string.Empty;
                    if (ItemPool.TagPool != null && ItemPool.TagPool.ContainsKey(pd.tag_name))
                    {
                        var tag = ItemPool.TagPool[pd.tag_name];
                        cannelId = tag.CannelID ?? string.Empty;
                        address = tag.Address ?? string.Empty;
                    }

                    object valueToWrite = pd.value;
                    writeItems.Add(new WriteValue
                    {
                        CannelID = cannelId,
                        ValueName = pd.tag_name,
                        value = valueToWrite,
                        Address = address,
                    });
                }

                if (writeItems.Count == 0)
                    return BadRequest("No writable parameters found in the recipe.");

                // 在后台按通道分组写入（避免在 Task.Run 内调用 Controller 辅助方法）
                var results = await Task.Run(() =>
                {
                    var rtvs = new List<RealTimeValue>();

                    if (ItemPool.Instance != null)
                    {
                        var groups = writeItems.GroupBy(w => string.IsNullOrEmpty(w.CannelID) ? "__default__" : w.CannelID);
                        foreach (var g in groups)
                        {
                            var wvq = new WriteValueQueue
                            {
                                Datetime = DateTime.Now,
                                Cannel = g.Key == "__default__" ? string.Empty : g.Key
                            };

                            foreach (var w in g)
                                wvq.WriteValues.Add(w);

                            try
                            {
                                ItemPool.Instance.WriteData(wvq);
                            }
                            catch
                            {
                                // 写失败不要抛出整个任务，继续尝试收集返回值
                            }
                        }

                        //// 写入后，尝试从内存值池读取当前实时值作为返回结果
                        //foreach (var w in writeItems)
                        //{
                        //    if (ItemPool.ValuePool != null && ItemPool.ValuePool.TryGetValue(w.ValueName, out var cur))
                        //    {
                        //        rtvs.Add(cur);
                        //    }
                        //    else
                        //    {
                        //        rtvs.Add(new RealTimeValue
                        //        {
                        //            ValueName = w.ValueName,
                        //            RealValue = null,
                        //            Quality = 0,
                        //            Timestamp = DateTime.Now
                        //        });
                        //    }
                        //}
                    }

                    return rtvs;
                });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
