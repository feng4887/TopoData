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
                            wvq.Cannel = item.CannelID;
                            wvq.Datetime = System.DateTime.Now;
                            int datatype = 10;
                            if (ItemPool.ValuePool.ContainsKey(item.ValueName))
                            {
                                datatype = ItemPool.TagPool[item.ValueName].DataType;
                            }
                            else
                                datatype = 1;
                            wvq.WriteValues.Add(new WriteValue()
                            {
                                CannelID = item.CannelID,
                                value = item.value,
                                //Address = item.Address,
                                Address = ItemPool.TagPool[item.ValueName].Address,
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
