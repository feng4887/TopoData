using System;
using System.Collections.Generic;
using auDASLib;
namespace TopoData.model
{

    // 定义事件参数，事件参数类必须继承自EventArs类，这是.net框架定义的类 
    public class woEventArgs : EventArgs
    {
        public auDASLib.Model.pWO Message { get; set; }

        public woEventArgs(auDASLib.Model.pWO message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// 通讯和设备状态时间参数
    /// </summary>
    public class StatEventArgs : EventArgs
    {
        /// <summary>
        /// 1-Comunication Driver status; 2-DB status
        /// </summary>
        public int Type { get; set; }
        public string Message { get; set; }

        public StatEventArgs(string message, int type)
        {
            Message = message;
            Type = type;
        }
    }

    public class CannelEventArgs : EventArgs
    {
        /// <summary>
        /// 1-Comunication Driver; 2- store cfg 3-Database Cfg
        /// </summary>
        public int Type { get; set; }
        public string Message { get; set; }

        public CannelEventArgs(string message, int type)
        {
            Message = message;
            Type = type;
        }
    }

    public class DBTagEventArgs : EventArgs
    {
        /// <summary>
        /// 1-OPC UA; 2-Profinet
        /// </summary>
        public DeviceType CannelType { get; set; }

        /// <summary>
        /// 1 - Add Tag; 2- Delete Tag ；3 - Update Tag
        /// </summary>
        public int ActionType { get; set; }
        public string Message { get; set; }
        public List<DBTag> Tags { get; set; }

        public DBTagEventArgs(string message, DeviceType cannelType, List<DBTag> tags, int actionType = 1)
        {
            Message = message;
            CannelType = cannelType;
            Tags = tags;
            ActionType = actionType;
        }
    }
}
