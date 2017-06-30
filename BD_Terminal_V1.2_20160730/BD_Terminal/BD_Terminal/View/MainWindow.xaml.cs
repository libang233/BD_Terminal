using System;
using System.Windows;
using BD_Terminal.Model;
using BD_Terminal.Control;
using System.Threading;

namespace BD_Terminal.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseInfoPage baseinfopage;
        //ConfigPage configpage;
        TextOutPage textoutpage;
        //Msshistogram sates_signal_strengthpage;
        //MsimeteorSites msimetorsitespage;

        // 刷新线程
        private Thread UpdateThread;

        // 设置刷新时间
        private const int M_UPDATE_TIME = 50;

        // 用于线程停止和启动
        private volatile bool IsAllowRun = false;
        private AutoResetEvent autoRstEvt = new AutoResetEvent(false);

        public MainWindow(User_Model model, User_Control control)
        {
            InitializeComponent();
            
            // 实例化baseinfopage 
            baseinfopage = new BaseInfoPage(model, control);
            BaseInofrFrame.Content = baseinfopage;

            //// 实例化configpage
            //configpage = new ConfigPage(model, control);
            //configFrame.Content = configpage;

            // 实例化textoutpage
            textoutpage = new TextOutPage(model, control);
            EditFrame.Content = textoutpage;

            //// 卫星信噪比
            //sates_signal_strengthpage = new Msshistogram("SateLites Strength" , model, control);
            //SatelliteFrame.Content = sates_signal_strengthpage;

            //// 星位图
            //msimetorsitespage = new MsimeteorSites(model, control);
            //MeteorSitesFrame.Content = msimetorsitespage;

            // 创建线程
            UpdateThread = new Thread(new ThreadStart(UpdateUI_Thread));
            UpdateThread.IsBackground = true;
            UpdateThread.Start();

            
        }

        ~MainWindow()
        {
            UpdateThread.Abort();

            UpdateThread = null;
        }

        /// <summary>
        /// 窗口关闭后操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 注销窗体中的各部分
        /// </summary>
        public void Log_Off()
        {
            // 禁止运行
            IsAllowRun = false;

            this.Dispatcher.Invoke(new Action(() =>
            {
                baseinfopage.Log_Off();
                //configpage.Log_Off();
                textoutpage.Log_Off();
                //sates_signal_strengthpage.Log_Off();
                //msimetorsitespage.Log_Off();
            }));
        }

        /// <summary>
        /// 唤醒窗体中的各个部分
        /// </summary>
        public void Awaken()
        {
            // 允许运行
            IsAllowRun = true;

            // 解锁阻塞
            autoRstEvt.Set();

            this.Dispatcher.Invoke(new Action(() =>
            {
                baseinfopage.Awaken();
                //configpage.Awaken();
                textoutpage.Awaken();
                //sates_signal_strengthpage.Awaken();
                //msimetorsitespage.Awaken();
            }));
        }

        /// <summary>
        /// 界面更新线程
        /// </summary>
        private void UpdateUI_Thread()
        {
            while(true)
            {
                // 判断是否允许运行
                if (!IsAllowRun)
                {
                    // 等待线程
                    autoRstEvt.WaitOne();
                }

                // 基础页更新
                baseinfopage.UpdateUI_Thread();
                //// 配置页更新
                //configpage.UpdateUI_Thread();
                //// 星位图更新
                //msimetorsitespage.UpdateUI_Thread();
                //// 直方图更新
                //sates_signal_strengthpage.UpdateUI_Thread();
                // 输出栏更新
                textoutpage.UpdateUI_Thread();

                Thread.Sleep(M_UPDATE_TIME);
            }
        }
    }
}
