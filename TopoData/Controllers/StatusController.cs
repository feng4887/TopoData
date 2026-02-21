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
    public class StatusController : ControllerBase
    {
        /// <summary>
        /// 读数据函数
        /// </summary>
        /// <param name="TagID">TagID清单</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Status/GetServerStatus")]
        public async Task<IActionResult> GetServerStatus()
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    return Ok(new ServerStatus() 
                    {  
                        isTtrail        = !HiTopoServer.MainClass.g_AppGood,
                        TotalTrailHours = HiTopoServer.MainClass.g_TotalTrailHours,
                        AppRunTimeOut = HiTopoServer.MainClass.g_AppRunTimeOut, 
                        AppStartTime  = HiTopoServer.MainClass.g_AppStartTime
                    });
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
