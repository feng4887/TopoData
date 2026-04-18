using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace auDAServer.Controllers
{
    public class CommandController : ControllerBase
    {
        /// <summary>
        /// 打开设备[FromBody] string connectionID
        /// </summary>
        /// <param name="connectionID">参数名清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Command/StartCannel")]
        public async Task<IActionResult> StartCannel()
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    Console.WriteLine("connectionID");
                    return Ok();
                    //bool started = CameraOperator.Instance.OpenAndStart();
                    //Colorful.Console.WriteLine($"[{ System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Start camera command ...",Color.Green);
                    //if (started)
                    //{
                    //    return new CamContentResult(JsonHelper.Serialize<auCamContent>(new auCamContent()
                    //    {
                    //        Code = 200,
                    //        IsSucess = true,
                    //        Message = "Start camera sucessfully ...",
                    //        Data = null
                    //    }), "application/json");
                    //}

                    //else
                    //{
                    //    return new CamContentResult(JsonHelper.Serialize<auCamContent>(new auCamContent()
                    //    {
                    //        Code = 100,
                    //        IsSucess = false,
                    //        Message = "Start camera failled..."
                    //    }), "application/json");

                    //}
                });

                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="connectionID">参数名清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Command/StopCannel")]
        public async Task<IActionResult> StopCannel([FromBody] string connectionID)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    return Ok();
                    //bool started = CameraOperator.Instance.OpenAndStart();
                    //Colorful.Console.WriteLine($"[{ System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Start camera command ...",Color.Green);
                    //if (started)
                    //{
                    //    return new CamContentResult(JsonHelper.Serialize<auCamContent>(new auCamContent()
                    //    {
                    //        Code = 200,
                    //        IsSucess = true,
                    //        Message = "Start camera sucessfully ...",
                    //        Data = null
                    //    }), "application/json");
                    //}

                    //else
                    //{
                    //    return new CamContentResult(JsonHelper.Serialize<auCamContent>(new auCamContent()
                    //    {
                    //        Code = 100,
                    //        IsSucess = false,
                    //        Message = "Start camera failled..."
                    //    }), "application/json");

                    //}
                });

                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="connectionID">参数名清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Command/AddCannel")]
        public async Task<IActionResult> AddConnection([FromBody] string connectionID)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    return Ok();
                });

                return ret;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="connectionID">参数名清单</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Command/AddCannel")]
        public async Task<IActionResult> DeletConnection([FromBody] string connectionID)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    return Ok();
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
