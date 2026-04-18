using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopoData.model
{
    public class ServerStatus
    {
        public bool isTtrail { get; set; } = false;
        public int TotalTrailHours { get; set; } = 5;
        public bool AppRunTimeOut { get; set; } = false;
        /// <summary>
        /// App 启动时间
        /// </summary>
        public DateTime AppStartTime { get; set; } = DateTime.Now;
    }
}
