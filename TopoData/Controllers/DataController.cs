using auDASLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace auDAServer.Controllers
{
    public class DataController : ControllerBase
    {
        /// <summary>
        /// 读数据函数
        /// </summary>
        /// <param name="TagID">TagID清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Data/GetRealtimeValues")]
        public async Task<IActionResult> GetRealtimeValues([FromBody] List<string> TagID = null)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    List<string> x = TagID;
                    List<RealTimeValue> rtvs = new List<RealTimeValue>();
                    if (x != null && x.Count > 0)
                    {
                        foreach (var item in x)
                        {
                            RealTimeValue realTimeValue = new RealTimeValue();
                            var vl = ItemPool.ValuePool.TryGetValue(item, out realTimeValue);
                            if (vl && realTimeValue != null)
                                rtvs.Add(realTimeValue);
                        }
                        
                    }
                    return Ok(rtvs);
                });

                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 安全查找点表：内存 TagPool 只在引擎启动时构建，点表在UI增删改后会过期，
        /// 直接索引会抛 KeyNotFoundException。查不到时从点表定义文件重载一次再试。
        /// </summary>
        private static bool TryGetTag(string tagId, out DBTag tag)
        {
            tag = null;
            if (string.IsNullOrEmpty(tagId))
                return false;

            if (ItemPool.TagPool.TryGetValue(tagId, out tag))
                return true;

            if (!System.IO.File.Exists(pubDefine.TagDefine))
                return false;

            List<DBTag> tags = XmlHelper.XmlDeserializeFromFile<List<DBTag>>(pubDefine.TagDefine, Encoding.UTF8);
            if (tags != null)
            {
                DataImport.dicCannelTags.Clear();
                DataImport.ImportCannelTags(tags);
                ItemPool.TagPool = new System.Collections.Concurrent.ConcurrentDictionary<string, DBTag>(DataImport.dicTagPool);
            }

            return ItemPool.TagPool.TryGetValue(tagId, out tag);
        }

        /// <summary>
        /// 写数据函数
        /// </summary>
        /// <param name="TagID">写参数清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Data/WriteRealtimeValues")]
        public async Task<IActionResult> WriteRealtimeValues([FromBody] List<WriteValue> WriteValues)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    List<WriteValue> rvs = WriteValues;
                    List<RealTimeValue> rtvs = new List<RealTimeValue>();

                    if (ItemPool.Instance != null)
                    {
                        WriteValueQueue wvq = new WriteValueQueue();
                        foreach (var item in rvs)
                        {
                            if (item == null || string.IsNullOrWhiteSpace(item.ValueName))
                                continue;

                            wvq.Cannel = item.CannelID;
                            wvq.Datetime = System.DateTime.Now;

                            int datatype = 1;
                            string address = item.Address ?? "";
                            if (TryGetTag(item.ValueName, out DBTag tg) && tg != null)
                            {
                                datatype = tg.DataType;
                                address = tg.Address;
                                if (string.IsNullOrEmpty(item.CannelID))
                                    item.CannelID = tg.CannelID;
                            }

                            wvq.WriteValues.Add(new WriteValue()
                            {
                                CannelID = item.CannelID,
                                value = item.value,
                                Address = address,
                                DataType = datatype,
                                ValueName = item.ValueName
                            });
                        }

                        ItemPool.Instance.WriteData(wvq);
                    }
                    return Ok(rtvs);

                });

                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
