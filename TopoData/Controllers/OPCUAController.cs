using Microsoft.AspNetCore.Mvc;
using auDASLib;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace auDAServer.Controllers
{
    public class OPCUAController : ControllerBase
    {

        /// <summary>
        /// 读数据函数
        /// </summary>
        /// <param name="uri">opc ua uri</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/OPCUA/GetUAEndpoints")]
        public async Task<IActionResult> GetUAEndpoints(string uri)
        {
            try
            {
                var ret = await Task.Run(() =>
                {
                    List< UACfg > lcfg = new List< UACfg >();
                    lcfg = UAOperator.GetUACfgs(uri);
                    return lcfg;
                });

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
