using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.Server.Chat
{
    /// <summary>
    /// ChatサーバのStartup
    /// </summary>
    public class Startup : IHostedService
    {
   
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Startup(IConfiguration configuration)
        {
        }
       
        
        /// <summary>
        /// スタート
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
//                Log.Info($"GameServer Start ( port: {Env.Config.GameServerPort} )");
            });
        } 
       
        
        /// <summary>
        /// 停止
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { });
        }
        
    }
}