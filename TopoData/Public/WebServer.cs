
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//***************************************************************************************
//Description: Web API service helper class for .net 8 +.
//Author: 杜金旺 Jim DU
//Data: 2026.11.16
//****************************************************************************************
namespace auDASLib
{
    public class WebAPIBuilder
    {
        /// <summary>
        /// 启动 WebAPI 服务
        /// </summary>
        /// <param name="uris">URI列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        public static async Task StartService(CancellationTokenSource cancellationToken)
        {
            List<string> x = new List<string>();
            //x.Add("http://127.0.0.1:8083");
            // x.Add("https://127.0.0.1:8084");
            x.Add(ServerCfg.Instance.RestUri1);
            x.Add(ServerCfg.Instance.RestUri2);
            Task.Run(async () =>
            {
                try
                {
                    var builder = WebApplication.CreateBuilder();

                    // 添加服务项（Controllers，Swagger, CORS等）
                    builder.Services.AddControllers();
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();

                    var app = builder.Build();

                    // 开发环境中添加 Swagger UI 支持
                    if (app.Environment.IsDevelopment())
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }

                    app.UseHttpsRedirection(); // HTTPS 重定向
                    app.UseAuthorization();


                    //foreach (var r in uris)
                    //{
                    //    app.Urls.Add(r);
                    //}
                    app.Urls.Add(x[0]);

                    //app.MapControllers();
                    //app.MapDefaultControllerRoute();
                    //app.Run();

                    app.MapControllers(); // 映射控制器路由
                    // 启动 Web 服务器
                    await app.RunAsync(cancellationToken.Token);
                }
                catch (OperationCanceledException)
                {
                    //Colorful.Console.WriteLine($"[{System.DateTime.Now:yyyy-MM-dd HH:mm:ss}] WebAPI Server stopped by request", Color.Yellow);
                }
                catch (Exception ex)
                {
                   // Colorful.Console.WriteLine($"[{System.DateTime.Now:yyyy-MM-dd HH:mm:ss}] WebAPI Server critical error: {ex.Message}", Color.Red);
                    //Colorful.Console.WriteLine(ex.StackTrace, Color.DarkRed);
                    //g_WebAPIIsRunning = false;
                }
            }, cancellationToken.Token);
        }

    }
}
