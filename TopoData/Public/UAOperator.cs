#region Reference
using Opc.Ua;
using Opc.Ua.Client;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
#endregion

//=============================================================================
//说明：OPC UA 操作类
//----------------------------------------------------------------------------- 
//作者：杜金旺 Jinwang DU
//日期：2022-11-26
//版本：1.0
//=============================================================================

namespace auDASLib
{
    public class UAOperator 
    {
        #region 构造函数
        public UAOperator(object para)
        {
            myUAParamter = para as UACfg;
        }

        ~UAOperator()
        {
            DisonnectServer();
        }

        #endregion //构造函数

        /// <summary>
        /// Fields & deifination
        /// </summary>
        #region Fields

        SugarDao sd;
        SqlSugarClient _sugarClient;

        protected string lastErrorMsg = "";
        protected string lastErrorMsg_ConnectServer = "";
        protected string lastErrorMsg_GetEndpoint = "";
        
        public int ActiveWorkorderCount = 0;
        ///<summary>
        /////every 20 seconds check status
        ///<summary>
        public int ConnectionMonitorWindow = 20000;

        /// <summary>
        /// 写操作代理 
        /// </summary>
        public delegate WriteResult WriteMethodCaller(object name);

        /// <summary>
        /// 点量表
        /// </summary>
        private List<DBTag> myDt = new List<DBTag>();

        private ConnctDevice myConnctDevice = new ConnctDevice();
        private int mySampleInterval = 0;
        private int mySampleQty = 0;
        private UACfg myUAParamter;


        /// <summary>
        /// 订阅
        /// </summary>
        private Subscription mySubscription;
        private Session mySession;
        private List<MonitoredItem> myMonitoredItems;

        ///// <summary>
        ///// Opc客户端的核心类
        ///// </summary>
        private UAClientHelperAPI myClientHelperAPI;

        private EndpointDescription mySelectedEndpoint;

        /// <summary>
        /// A 是变量tagID
        /// </summary>
        private List<string> A = new List<string>();

        /// <summary>
        /// B是OPC node节点
        /// </summary>
        private List<string> B = new List<string>();

        /// <summary>
        /// A_X 是第三方 变量tagID
        /// </summary>
        private List<string> A_X = new List<string>();

        /// <summary>
        /// B_X是第三方OPC node节点
        /// </summary>
        private List<string> B_X = new List<string>();

        /// <summary>
        /// TagID and Registed ID dictionary for registed Read  and Write
        /// </summary>
        public Dictionary<string, string> dicTagItems = new Dictionary<string, string>();

        /// <summary>
        /// TagID 拆分
        /// </summary>
        private List<List<string>> ADic = new List<List<string>>();

        /// <summary>
        /// OPC Node节点拆分
        /// </summary>
        private List<List<string>> BDic = new List<List<string>>();

        /// <summary>
        /// 注册节点拆分
        /// </summary>
        private List<List<string>> CDic = new List<List<string>>();

        /// <summary>
        /// 第三方TagID 拆分
        /// </summary>
        private List<List<string>> ADic_X = new List<List<string>>();

        /// <summary>
        /// 第三方OPC Node节点拆分
        /// </summary>
        private List<List<string>> BDic_X = new List<List<string>>();

        /// <summary>
        /// 第三方注册节点拆分
        /// </summary>
        private List<List<string>> CDic_X = new List<List<string>>();

        //private Int16 itemCount;
        private readonly object sessionLock = new object();

        #endregion

        #region ClearScreen
        /// <summary>
        /// Console Line counter
        /// </summary>
        protected int ConLine = 0;

        protected void ClearScreen()
        {
#if DEBUG
            ConLine++;
            if (ConLine > 40)
            {
                ConLine = 0;
                Console.Clear();
            }
#endif
        }
        #endregion

        #region Connection

        /// <summary>
        /// 获取所有的Endpoint
        /// </summary>
        /// <param name="discoveryUrl">The local discovery URL for the discovery server</param>
        /// <returns></returns>
        public static List<UACfg> GetUACfgs(string discoveryUrl)
        {
            UAClientHelperAPI myClientHelperAPI = new UAClientHelperAPI();
            List<UACfg> lcfg = new List<UACfg>();
            try
            {
                
                 ApplicationDescriptionCollection servers = myClientHelperAPI.FindServers(discoveryUrl);
                foreach (ApplicationDescription ad in servers)
                {
                    foreach (string url in ad.DiscoveryUrls)
                    {
                        EndpointDescriptionCollection endpoints = myClientHelperAPI.GetEndpoints(url);
                        foreach (EndpointDescription ep in endpoints)
                        {
                            UACfg cfg = new UACfg();
                            string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);
                            cfg.securityPolicy = securityPolicy;
                            cfg.SecurityMode = ep.SecurityMode.ToString();
                            cfg.ApplicationName = ad.ApplicationName.ToString();
                            lcfg.Add(cfg);
                            //endpointListView.Items.Add("[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]").Tag = ep;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
                //MessageBox.Show(ex.Message, "Error");
            }
            return lcfg;

        }

        /// <summary>
        /// 获取单个Endpoint
        /// </summary>
        /// <param name="UAParamter"></param>
        /// <returns></returns>
        public EndpointDescription GetEndpoint(UACfg UAParamter)
        {
            //The local discovery URL for the discovery server
            string discoveryUrl = UAParamter.EndpointUrl;
            EndpointDescription ed = new EndpointDescription();
            try
            {
                ApplicationDescriptionCollection servers = myClientHelperAPI.FindServers(discoveryUrl);
                foreach (ApplicationDescription ad in servers)
                {
                    foreach (string url in ad.DiscoveryUrls)
                    {
                        EndpointDescriptionCollection endpoints = myClientHelperAPI.GetEndpoints(url);
                        foreach (EndpointDescription ep in endpoints)
                        {
                            string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);
                            if ((UAParamter.SecurityMode == ep.SecurityMode.ToString()) && (UAParamter.securityPolicy == securityPolicy.ToString()))
                            {
                                ed = ep;

                                ClearScreen();
                                Console.WriteLine("[Conn(" + myConnctDevice.CannelID.ToString() + ")]OPC UA Found :[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]");
                                break;
                            }
                            //endpointListView.Items.Add("[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]").Tag = ep;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "]" + "[Conn(" + myConnctDevice.CannelID.ToString() + ")]OPC UA Get Endpoint failed: " + ex.Message);
                Console.WriteLine(); Console.WriteLine();
#endif
                string msg = ex.Message;
                //if (msg != lastErrorMsg_GetEndpoint)
                //    AppLogMessage.InsertLog(sd, AppLogAppDefine.dasServer,
                //ex.Message + ex.StackTrace, (int)AppLogPriority.Error,"UAOperator");
                //lastErrorMsg_GetEndpoint = ex.Message;
                //if (ServerCfg.Instance.EnableLog4)
                //   StdLog4.Log4Helper.Instance.Error("[Conn(" + myConnctDevice.CannelID.ToString() + ")][OPC UA] GetEndpoint()", ex);
            }
            return ed;
        }



        /// <summary>
        /// OPC UA connected
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if ((mySession != null && !mySession.Disposed) && mySession.KeepAliveStopped == false)
                    return true;
                else
                    return false;
            }
        }

        public void DisonnectServer()
        {
            if (myClientHelperAPI != null)
            {
                //EngineStopFlag = true;
                Unsubscribe_Items();

                //if (Redis != null)
                //    Redis.Dispose();

                myClientHelperAPI.Disconnect();
                mySession = null;
                myMonitoredItems = null;
                if (mySubscription != null)
                    mySubscription.Dispose();

#if DEBUG
                ClearScreen();
                //Console.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "]" + "OPC UA server lost connection  :(");
                //Console.ForegroundColor = ConsoleColor.White;
#endif
            }

        }

        /// <summary>
        /// 连接 UA server
        /// </summary>
        public async Task ConnectServer()
        {
            //Check if sessions exists; 
            if (IsConnected)
            {
#if DEBUG
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"OPC UA {myConnctDevice.CannelID.ToString()} connected :)");
                Console.ForegroundColor = ConsoleColor.Green;
#endif
                return;
            }
            else
                
            {
                //DisonnectServer();

                lock (sessionLock)
                { 
                    DisonnectServer();
                }
                try
                {
                    myClientHelperAPI = new UAClientHelperAPI();
                    mySelectedEndpoint = GetEndpoint(myUAParamter);

                    string pw = myUAParamter.Psw;
                    string user = myUAParamter.User;
                    bool usePsw = myUAParamter.UseUserLogIn;
                    string sessionID = "this.CannelID";
                    mySession = myClientHelperAPI.Session;
                    //Register mandatory events (cert and keep alive)
                    //-----------------------
                    myClientHelperAPI.KeepAliveNotification += new KeepAliveEventHandler(Notification_KeepAlive);
                    myClientHelperAPI.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);

                    //Check for a selected endpoint
                    if (mySelectedEndpoint != null/* && mySession != null*/)
                    {
                        //-----------------------
                        //Call connect
                        //*注意更新*
                        //
                        //-----------------------
                        
                        await myClientHelperAPI.Connect(mySelectedEndpoint, usePsw, user, pw, sessionID);

                        //Extract the session object for further direct session interactions
                        mySession = myClientHelperAPI.Session;

                        if (mySession != null)
                        {
                            ClearScreen();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"OPCUA  {myConnctDevice.CannelID.ToString()} connected successfully :) :)");
                            Task.Delay(10);
                            //Thread.Sleep(10);
                            Console.WriteLine("*");
                            Task.Delay(10).Wait();
                            Console.WriteLine("*");
                            Task.Delay(10).Wait();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Task.Delay(100).Wait();
                        }
                    }
                    else
                    {
                        // MessageBox.Show("在连接服务器前倾选择一个 endpoint 。", "错误");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    //if (lastErrorMsg != ex.Message && ServerCfg.Instance.EnableLog4)
                    //    LogHelper.Instance.Error("[OPC UA] ConnectUAServer():" + ex.Message);

                    string msg = ex.Message;
                    //if (msg != lastErrorMsg_ConnectServer) 
                     //   AppLogMessage.InsertLog(sd, AppLogAppDefine.dasServer,
                      //      ex.Message + ex.StackTrace, (int)AppLogPriority.Error, "UAOperator");
                    lastErrorMsg_ConnectServer = ex.Message;
                    //if (ServerCfg.Instance.EnableLog4)
                    //    StdLog4.Log4Helper.Instance.Error(, ex);
                }
            }
        }

        #endregion //Connection

        /// <summary>
        /// Global OPC UA event handlers
        /// </summary>
        #region OpcEventHandlers
        private void Notification_ServerCertificate(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate), cert, e);
            //    return;
            //}

            try
            {
                //Search for the server's certificate in store; if found -> accept
                X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509CertificateCollection certCol = store.Certificates.Find(X509FindType.FindByThumbprint, e.Certificate.Thumbprint, true);
                store.Close();
                if (certCol.Capacity > 0)
                {
                    ClearScreen();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"OPC UA {myConnctDevice.CannelID.ToString()} disconnected  ...");
                    Console.ForegroundColor = ConsoleColor.Green;
                    e.Accept = true;
                }

                //Show cert dialog if cert hasn't been accepted yet
                else
                {
                    if (!e.Accept)
                    {
                        ClearScreen();
                        e.Accept = true;
                        X509Store store2 = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                        store2.Open(OpenFlags.ReadWrite);
                        store2.Add(e.Certificate);
                        store2.Close();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Server Certificate Registed ...");
                        Console.ForegroundColor = ConsoleColor.Green;
                        //if (ServerCfg.Instance.EnableLog4)
                        //    StdLog4.Log4Helper.Instance.Info("[Conn(" + myConnctDevice.CannelID.ToString() + ")][OPC UA] Notification_ServerCertificate() Server Certificate Registed ...");
                    }
                }
            }
            catch (Exception ex)
            {
                // if (ServerCfg.Instance.EnableLog4)
                //     StdLog4.Log4Helper.Instance.Info("[Conn(" + myConnctDevice.CannelID.ToString() + ")][OPC UA] Notification_ServerCertificate()", ex);
            }
        }

        /// <summary>
        /// Auto connection can be here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notification_KeepAlive(Opc.Ua.Client.ISession sender, KeepAliveEventArgs e)
        {
            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, mySession))
                {
                    return;
                }

                // check for disconnected session.
                if (!ServiceResult.IsGood(e.Status))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Server {myConnctDevice.CannelID.ToString()} Certificate Registed ...");
                    Console.ForegroundColor = ConsoleColor.Green;
                    // try reconnecting using the existing session state
                    lock (sessionLock)
                    { 
                        mySession.Reconnect();                   
                    }

                }
            }
            catch (Exception ex)
            {
                //if (ServerCfg.Instance.EnableLog4)
                //    StdLog4.Log4Helper.Instance.Error("[OPC UA] Notification_KeepAlive()", ex);
            }
        }

        #endregion

        #region 工作入口

        /// <summary>
        /// 监控Monitor Connection
        /// </summary>
        Task tsk_MonitorConnection;

        /// <summary>
        /// 监控连接状态
        /// </summary>
        public async Task MonitorConnection(CancellationTokenSource cancellationToken = null)
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    //-------------------------------------------------------------------
                    //未授权停止工作
                    //-------------------------------------------------------------------
                    //if (!Program.AppGood && (System.DateTime.Now - Program.AppStartTime).Hours >= Program.AppRunHours)
                    //{
                    //    break;
                    //}

                    if (!IsConnected)
                    {
                        //for (int i = 0; i < _ThreadPool.Count; i++)
                        //{
                        //    if (_ThreadPool[i] != null)
                        //    {
                        //        _ThreadPool[i].Abort();
                        //        _ThreadPool[i] = null;
                        //    }
                        //}
                        //_ThreadPool.Clear();
                        //Redis = PubModels.RedisManager.GetClient();
                        lock (sessionLock)
                        { 
                              DisonnectServer();                      
                        }

                        await Task.Delay(100, cancellationToken.Token);

                        await ConnectServer();

                        lock (sessionLock)
                        {
                            if (IsConnected)
                            {
                                ClearScreen();
                                Console.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "] - [" + myConnctDevice.CannelID.ToString() + "]  Reconnected ...................................");
                                BeginRead();
                            }
                        }
                    }

                    //every ? seconds check status
                    //Thread.Sleep(ConnectionMonitorWindow);
                    await Task.Delay(ConnectionMonitorWindow, cancellationToken.Token);
                }
            });
        }

        public void Stop()
        {

            DisonnectServer();
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        /// <param name="tags">变量列表</param>
        /// <param name="SampleInterval">采样时间间隔</param>
        /// <param name="SampleQty">采样数量</param>
        /// <param name="connID"></param>
        public async void Start(ConnctDevice cd, List<DBTag> tags, int SampleInterval, int SampleQty)
        {
            this.sd = new SugarDao();
            this._sugarClient = sd.GetInstance();

            myDt = tags;
            myConnctDevice = cd;

            if (SampleInterval > 5000)
                SampleInterval = 5000;

            if (SampleInterval < 100)
                SampleInterval = 100;

            mySampleInterval = SampleInterval;
            mySampleQty = SampleQty;

            //_pool = new DefaultObjectPool<RealTimeValue>(policy);

            await ConnectServer();

            Task.Delay(500).Wait();

            if (IsConnected)
                BeginRead();

            //New thread to monitor status
            if (tsk_MonitorConnection == null)
                tsk_MonitorConnection = MonitorConnection();
            //MonitorWriteQueue();
        }

        /// <summary>
        /// 读数据入口
        /// </summary>
        protected void BeginRead()
        {

            if (myUAParamter.Subscribe == true)
                BeginReadbySubscribeTags(); //订阅方式读
            else
                BeginReadbyRegistTags();    //按注册方式读
        }

        /// <summary>
        /// 监控发送缓冲区
        /// </summary>
        public async Task MonitorWriteQueue()
        {
            // ServiceStack.Redis.IRedisClient Redis = new PubModels.RedisHelper().GetClient();
            await Task.Run(() =>
            {
                while (true)
                {
                    //-------------------------------------------------------------------
                    //未授权停止工作
                    //-------------------------------------------------------------------
                    //if (!Program.AppGood && (System.DateTime.Now - Program.AppStartTime).Hours >= Program.AppRunHours)
                    //    break;

                    try
                    {
                        int QCount = 0;// Redis.GetListCount(ServerCfg.Instance.WriteQueKey);

                        if (QCount > 0)
                        {
                            tmStatus.WriteLock = true;

                            for (int i = 0; i < QCount; i++)
                            {

                                //string queue = Redis.DequeueItemFromList(ServerCfg.Instance.WriteQueKey);
                                //WriteValueQueue write = null;
                                //if (!string.IsNullOrEmpty(queue))
                                //    write = JsonHelper.DeSerialize<WriteValueQueue>(queue);
                                //else
                                //{
                                //    Thread.Sleep(473);
                                //    continue;
                                //}

                                //if (write != null)
                                //{
                                //    WriteData(write);
                                //    Thread.Sleep(50);
                                //}
                                //else
                                //    continue;
                            }
                            tmStatus.WriteLock = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (lastErrorMsg != ex.Message && ServerCfg.Instance.EnableLog4)
                        //    LogHelper.Instance.Error(ex.Message);
                        //string msg = ex.Message;
                        //if (msg != lastErrorMsg)
                        //    AppLogMessage.InsertLog(sd, AppLogAppDefine.dasServer,
                        //    ex.Message + ex.InnerException, (int)AppLogPriority.Error, "UAOperator");
                        //lastErrorMsg = ex.Message;

#if DEBUG

#endif
                        // Redis = new PubModels.RedisHelper().GetClient();
                    }
                    finally
                    {
                        Thread.Sleep(333);
                    }

                }
            });
        }

        /// <summary>
        /// 读数据入口-注册tag，自动读写模式
        /// </summary>
        protected void BeginReadbyRegistTags()
        {
            List<string> ppNames = new List<string>();
            List<string> ppAddress = new List<string>();
            List<List<string>> ppDicNames = new List<List<string>>();
            List<List<string>> ppDicAddress = new List<List<string>>();
            List<List<string>> ppDicRegist = new List<List<string>>();

            A.Clear();
            B.Clear();
            ADic.Clear();
            BDic.Clear();
            CDic.Clear();

            A_X.Clear();
            B_X.Clear();
            ADic_X.Clear();
            BDic_X.Clear();
            CDic_X.Clear();

            //PhaseMapPool.Clear();

            if (myDt.Count > 0)
            {
                ClearScreen();

            }
        }

        /// <summary>
        /// 读数据入口-订阅模式
        /// </summary>
        protected void BeginReadbySubscribeTags()
        {

            List<string> items = new List<string>();

            items.AddRange(myDt.Select(it=> it.Address).ToList());
            Subscribe_Items(items);

        }

        #endregion

        #region Regist Tag Operation



        #region ASYNC read tags

        object o = new object();
        int linecount = 0;

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="IDs">注册的Node ID</param>
        /// <param name="window">采样间隔</param>
        /// <param name="cls">类别 0-内部；1-保留；2-外部点</param>
        public async void GetData(List<List<string>> IDs, int window, int cls)
        {
            // ServiceStack.Redis.IRedisClient myRedis = PubModels.RedisManager.GetClient();

            if (IDs.Count <= 0)
                return;

            //if (cls == 0)
            //    ManageStepPhaseTags(431);

            Dictionary<string, RealTimeValue> dic = new Dictionary<string, RealTimeValue>();

            await Task.Run(() =>
            {
                Console.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "][" + myConnctDevice.CannelID.ToString() + "]OPC UA Thread -[" + Thread.CurrentThread.ManagedThreadId.ToString() + "] Started ...................................");
                while (IsConnected)
                // while (true)
                {
                    //-------------------------------------------------------------------
                    //未授权停止工作
                    //-------------------------------------------------------------------
                    //if (!Program.AppGood && (System.DateTime.Now - Program.AppStartTime).Hours >= Program.AppRunHours)
                    //    break;

                    lock (o)
                    {
                        ClearScreen();

                        //<Read Groups>
                        for (int m = 0; m < IDs.Count; m++)
                        {
                            try
                            {
                                dic.Clear();
#if DEBUG
                                linecount++;
                                DateTime ds1 = System.DateTime.Now;
                                Console.WriteLine("[" + ds1.ToString("yyyy-MM-dd hh:mm:ss.fff") + "][" + myConnctDevice.CannelID.ToString() + "]OPC UA Thread -[" + Thread.CurrentThread.ManagedThreadId.ToString() + "] R ---->");
#endif

                                DateTime ds2 = System.DateTime.Now;
                                //OPC  read
                                List<dynamic> values = RgReadTags(IDs[m]);

#if DEBUG
                                // Console.WriteLine("class "+cls.ToString() + ": "+values.Count);

                                ds2 = System.DateTime.Now;
                                Console.WriteLine("[" + ds2.ToString("yyyy-MM-dd hh:mm:ss.fff") + "][" + myConnctDevice.CannelID.ToString() + "]OPC UA Thread -[" + Thread.CurrentThread.ManagedThreadId.ToString() + "]RF <---");
#endif

                                //<Set Redis>
                                if (values.Count > 0)// Read sucessfully ,good quality
                                {


                                    //if ((dic.Count > 0) && (cls != 1))
                                    //    myRedis.SetAll(dic);
                                }
                                else// Read faild ,bad quality
                                {

                                    //if ((dic.Count > 0) && (cls != 1))
                                    //    myRedis.SetAll(dic);
                                }



                                Task.Delay(window).Wait();
                                //Thread.SpinWait(20000);

#if DEBUG
                                Console.WriteLine("[" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "][" + myConnctDevice.CannelID.ToString() + "]OPC UA Thread -[" + Thread.CurrentThread.ManagedThreadId.ToString() + "]Sleep:" + window.ToString());
                                if (linecount >= 40)
                                {
                                    Console.Clear();
                                    Console.BufferHeight = 800;
                                    linecount = 0;
                                }
#endif
                            }
                            catch (Exception ex)
                            {
                                dic.Clear();

                            }
                        }
                        //<Read Groups>
                    }
                }
            });

            // myRedis.Dispose();
            // myRedis = null;
        }

        /// <summary>
        /// 注册读操作
        /// </summary>
        /// <param name="ValuePool"></param>
        private List<dynamic> RgReadTags(List<string> IDs)
        {
            List<dynamic> values = new List<dynamic>();

            return values;
        }


        #endregion //ASYNC read tags


        #region ASYNC Write tags

        /*
        /// <summary>
        /// 写操作
        /// </summary>
        public WriteResult WriteData(WriteValueQueue wvs)
        {
            //List<string> misstags = new List<string>();

            //-------------------------------------------------------------------
            //未授权停止工作
            //-------------------------------------------------------------------
            //if (!Program.AppGood && (System.DateTime.Now - Program.AppStartTime).Hours >= Program.AppRunHours)
            //    return new WriteResult() {  count = 0, ErrorMessage = "Software license error."};

            WriteResult wr = WriteTags(wvs);
            try
            {
                //<Log write data>
                //if (ServerCfg.Instance.SaveCsvLog_W && (!string.IsNullOrEmpty(ServerCfg.Instance.SaveCsvLogPath)))
                //{
                //    CSVHelper.WriteToCSV(ServerCfg.Instance.SaveCsvLogPath, "OPCUA Subscribe WriteTags", new List<string>() { "WinCCWriteTags", "Count: " + wvs.WriteValues.Count.ToString(), "------", "Redis Dequeue write", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "WriteLog" });
                //}
                //</Log data>

            }
            catch (System.AccessViolationException ex)
            {
                //if (ServerCfg.Instance.EnableLog4 && LastErrorMsg != "[OPCUA Subscribe] WriteData() " + ex.Message)
                //    StdLog4.Log4Helper.Instance.Error("[OPCUA Subscribe] WriteData()", ex);
                //LastErrorMsg = "[OPCUA Subscribe] WriteData() "+ ex.Message;

            }
            catch (System.NullReferenceException ex)
            {
                //if (ServerCfg.Instance.EnableLog4 && LastErrorMsg != "[OPCUA Subscribe] WriteData() " + ex.Message)
                //    StdLog4.Log4Helper.Instance.Error("[OPCUA Subscribe] WriteData()", ex);
                LastErrorMsg = "[OPCUA Subscribe] WriteData() " + ex.Message;
            }
            catch (Exception ex)
            {
                //if (ServerCfg.Instance.EnableLog4 && LastErrorMsg != "[OPCUA Subscribe] WriteData() " + ex.Message)
                //    StdLog4.Log4Helper.Instance.Error("[OPCUA Subscribe] WriteData()", ex);
                LastErrorMsg = "[OPCUA Subscribe] WriteData() " + ex.Message;
            }

            return wr;
        }
        */

        /// <summary>
        /// OPC UA 写操作
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>

        /*
        protected WriteResult ASYNC_WriteTags(object i)
        {
            List<string> misstags = new List<string>();
            //WriteValues xx = (WriteValues)i;
            WriteValueQueue xx = (WriteValueQueue)i;

            //1 分钟以前写操作会过期
            if (System.DateTime.Now.Subtract(xx.Datetime).TotalMinutes > 1)
                return new WriteResult() { count = xx.WriteValues.Count, ErrorMessage = "Write timeout ..." ,lastError = 20 };

            List<string> IdL = new List<string>();//UA node ID list
            List<dynamic> VlL = new List<dynamic>(); // Value list
            List<Type> types = new List<Type>(); // gypd list
            foreach (var item in xx.WriteValues)
            {
                //<Get Registed tags>
                //if (ItemPool.Instance.TagPool.ContainsKey(item.ValueName))
                //{
                //    string id = "";
                //    if (myUAParamter.Subscribe == false)
                //    {
                //        id = ItemPool.Instance.TagPool[item.ValueName].RegID;
                //    }
                //    else
                //    {
                //        id = item.Address;
                //    }

                //    IdL.Add(id);
                //    dynamic value = item.value;
                //    VlL.Add(value);
                //    types.Add(ItemPool.Instance.TagPool[item.ValueName].type);

                //}
                //else
                //    misstags.Add(item.ValueName);
                //</Get Registed tags>
            }

            string exc = "";
            int    errcode = 0;
            string errstring = "";
            string losttags = "";
            try
            {
                foreach (var m in misstags)
                {
                    losttags = m + ";";
                }

                //Write 
                errstring = _WriteTags(VlL, IdL,types);

                if (!String.IsNullOrEmpty(losttags))
                    errstring = errstring + "  LostTags[" + misstags.Count.ToString() + "]:"+ losttags;

                if (!string.IsNullOrEmpty(errstring))
                {
                   // if (ServerCfg.Instance.EnableLog4)
                   //     StdLog4.Log4Helper.Instance.Error("[OPC UA] ASYNC_WriteTags(): " + errstring, new Exception());
                }
                else
                {
                    //更新内存 phase 参数
                    foreach (var v in xx.WriteValues)
                    {
                        RealTimeValue rv = new RealTimeValue() { ValueName = v.ValueName, RealValue = v.value, Timestamp = DateTime.Now, Quality = 192 };
                        ValuePool.AddOrUpdate(v.ValueName, rv, (x, y) => rv);
                      //  if(Redis != null)
                      //      Redis.Set(v.ValueName, rv);
                    }
                }

                if (!string.IsNullOrEmpty(errstring))
                { errstring = "[" + errstring + "]"; }

            }
            catch (Exception ex)
            {
                exc = "UA Exception [" + ex.Message + "|" + errcode.ToString() + " ] ";
                errcode = 10;
                //if (ServerCfg.Instance.EnableLog4)
                  //  StdLog4.Log4Helper.Instance.Error("[Conn(" + myConnctDevice.CannelID.ToString() + ")][OPC UA] ASYNC_WriteTags()", ex);
            }

            return new WriteResult() { count = IdL.Count, ErrorMessage = errstring  + exc , lastError = errcode };
        }
        */

        /// <summary>
        /// 注册写操作
        /// </summary>
        /// <param name="values"></param>
        /// <param name="Ids"></param>
        /// <returns>错误代码</returns>
        private string _WriteTags(List<dynamic> values, List<String> Ids, List<Type> Types = null)
        {
            string errorcode = "";
            try
            {
                lock (new object())
                {

                    Dictionary<NodeId, IEnumerable<string>> toWrite = new Dictionary<NodeId, IEnumerable<string>>();
                    for (int i = 0; i < Ids.Count; i++)
                    {
                        //Create a nodeId
                        NodeId nodeId = new NodeId(Ids[i]);
                        List<string> value = new List<string>();
                        value.Add(string.Format($"{values[i]}"));
                        toWrite.Add(nodeId, value);
                    }

                }
   
            }
            catch (Exception ex)
            {

 
            }
            return errorcode;
        }


        
        #endregion

        #endregion //Regist Tag Operation

        #region Subscribe Operation
        private async void Subscribe_Items(List<string> items)
        {
            await Task.Run(async () =>
            {
                try
                {
                    //use different item names for correct assignment at the notificatino event
                    //string monitoredItemName = "myItem" + itemCount.ToString();
                    if (mySubscription == null)
                    {
                        mySubscription = myClientHelperAPI.Subscribe(1000, "Subscribe_" + myConnctDevice.CannelID.ToString());
                    }
                    myMonitoredItems = myClientHelperAPI.AddMonitoredItemsBySplit(mySubscription, items, 1);
                    myClientHelperAPI.ItemChangedNotification += new MonitoredItemNotificationEventHandler(Notification_MonitoredItem);

                    await Task.Delay(400);
                }
                catch
                { }

            });
        }

        void Unsubscribe_Items()
        {
            if (mySubscription != null)
                myClientHelperAPI.RemoveSubscription(mySubscription);

            mySubscription = null;
            if (myMonitoredItems != null)
                myMonitoredItems.Clear();
            //timerRefreshOnlineTags.Stop();
        }

        private object _o = new object ();
        private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            //-------------------------------------------------------------------
            //未授权停止工作
            //-------------------------------------------------------------------
            // if (!Program.AppGood && (System.DateTime.Now - Program.AppStartTime).Hours >= Program.AppRunHours)
            //{
            //    Unsubscribe_Items();
            //    return;
            //}

            //await Task.Run(() =>
            //{
            MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
            if (notification == null)
            {
                return;
            }

            lock (_o)
            {
                //<Set Redis>
                try
                {
                    string a = Utils.Format("{0}", notification.Value.WrappedValue.ToString());
                    dynamic aa = notification.Value.WrappedValue.Value;
                    string b = "Item name: " + monitoredItem.DisplayName;
                    string x = "Source timestamp: " + notification.Value.SourceTimestamp.ToString();
                    string y = "Server timestamp: " + notification.Value.ServerTimestamp.ToString();

                    StatusCode z = notification.Value.StatusCode;
                    int status = 0;
                    if (StatusCode.IsGood(notification.Value.StatusCode))
                    {
                        status = 192;
                    }
                    else if (StatusCode.IsBad(notification.Value.StatusCode))
                    {
                        status = 0;
                    }
                    else
                    {
                        status = 64;
                    }

                    Dictionary<string, RealTimeValue> dic = new Dictionary<string, RealTimeValue>();


                    //if (dic.Count > 0)
                    //    Redis.SetAll(dic);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
    //                if (msg != lastErrorMsg)
    //                AppLogMessage.InsertLog(sd, AppLogAppDefine.dasServer,
    //ex.Message + ex.InnerException, (int)AppLogPriority.Error,"UAOperator");
                    lastErrorMsg = ex.Message;
       
                }
                //</Set Redis>
            }

            //});
        }
        #endregion //

    }
}
