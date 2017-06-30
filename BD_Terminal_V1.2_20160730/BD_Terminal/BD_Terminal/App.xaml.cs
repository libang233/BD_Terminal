using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using BD_Terminal.Control;
using BD_Terminal.Model;
using BD_Terminal.View;

namespace BD_Terminal
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private User_Control mControl;
        private User_Model mModel;
        private MainWindow mMainWindow;

        /// <summary>
        /// 构造函数，程序启动入口
        /// </summary>
        public App()
        {
            // 初始化数据模型
            mModel = new User_Model();

            // 初始化控制器
            mControl = new User_Control(mModel);

            // 初始化界面
            mMainWindow = new MainWindow(mModel, mControl);

            // 添加控制器委托
            mControl.User_Control_Event += mControlEventDeal;

            // 显示窗口
            mMainWindow.Show();
        }

        public void mControlEventDeal(User_Control.UserControlEventType type)
        {
            if (type == User_Control.UserControlEventType.SerialPortOpened)
            {
                // 唤醒
                mMainWindow.Awaken();
            }
            else if(type == User_Control.UserControlEventType.SerialPortClosed)
            {
                // 注销
                mMainWindow.Log_Off();  
            }  
        }
    }
}
