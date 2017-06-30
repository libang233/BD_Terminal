using BD_Terminal.Control;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using BD_Terminal.Model;

namespace BD_Terminal.View
{
    /// <summary>
    /// BaseInforPage.xaml 的交互逻辑
    /// </summary>
    public partial class BaseInfoPage : Page
    {
        // 数据模型
        private User_Model mModel;
        private User_Control mControl;

        // 一个label链表
        private List<Label> mDataInfoLabelList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <param name="control">控制模型</param>
        public BaseInfoPage(User_Model model, User_Control control)
        {
            InitializeComponent();

            // 设置模型
            mModel = model;
            // 设置控制器
            mControl = control;

            // 加载波特率列表
            combox_baudRate.ItemsSource = mModel.BaudRateArry;
            // 默认为38400的波特率
            combox_baudRate.SelectedIndex = User_Model.BAUDRATEARRY_38400_POS;

            // 初始化列表
            mDataInfoLabelList = new List<Label>();

            // 初始化信息列表
            InitBaseInfoList();

            // 设置默认图标
            btn_connect_com.Content = "Connect";

            // 取得当前可用的串口
            string[] str_availcoms = mModel.GetOnlineComName(); 

            if (str_availcoms != null)
            {
                commbox_com.ItemsSource = str_availcoms;
                //设置默认选中第一个
                commbox_com.SelectedIndex = 0;
            }
;
        }

        /// <summary>
        /// 析构函数，用于终止刷新线程
        /// </summary>
        ~BaseInfoPage()
        {

        }

        /// <summary>
        /// 用于注销当前子窗体信息
        /// </summary>
        public void Log_Off()
        {
            //设置图标
            btn_connect_com.Content = "Connect";
            
            // 串口关闭的时候清空显示
            btn_connect_com.Content = "Connect";
            for (int i = 0; i < mDataInfoLabelList.Count; i++)
            {
                mDataInfoLabelList[i].Content = null;
            }
            GpsState.SetUnactive();
        }

        /// <summary>
        /// 唤醒当前窗体
        /// </summary>
        public void Awaken()
        {
            // 设置图标
            btn_connect_com.Content = "DisConnect";
        }

        /// <summary>
        /// 初始化显示标签
        /// </summary>
        private void InitBaseInfoList()
        {
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_LONGTITUDE, label_longitude);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_LATITUDE, lable_latitude);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_UTC_TIME, lable_UTCtime);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_ALTITUDE, lable_altitude);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_ELLIPSOIDAL_HEIGHT, lable_Ellipsoidal_height);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_SATELLITE_NUM, lable_satellite);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_POSITION_ACC, lable_pos_acc);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_PSEUDORANGE, lable_pseudorange);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_PSEUDORANGERATE, lable_pseudorangeRate);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_GDOP, lable_GDOP);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_PDOP, lable_PDOP);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_GAS_COORDINATION_X, lable_GAS_coordination_X);
            mDataInfoLabelList.Insert(CustomDataModel.LABEL_GAS_COORDINAIION_Y, lable_GAS_coordination_Y);
            mDataInfoLabelList.Insert(CustomDataModel.STR_LONGTITUDE_STD, label_latstd); 
            mDataInfoLabelList.Insert(CustomDataModel.STR_LATITUDE_STD, label_longstd);
            
        }

        /// <summary>
        /// 更新线程
        /// </summary>
        public void UpdateUI_Thread()
        {
            // 更新UI
            this.Dispatcher.Invoke(new Action(() =>
            {
                // 扫描更新界面
                for (int i = 0; i < mDataInfoLabelList.Count; i++)
                {
                    // 是否可以更新
                    if (mModel.MCustomDataModel.DataBaseList[i] != null)
                    {
                        if (mModel.MCustomDataModel.DataBaseList[i].IsUpdate == true)
                        {
                            // 设置信息
                            mDataInfoLabelList[i].Content = mModel.MCustomDataModel.DataBaseList[i].Info;

                            mModel.MCustomDataModel.DataBaseList[i].IsUpdate = false;

                        }
                        else
                        {
                            // 如果在范围内
                            if (i >= CustomDataModel.LABEL_PSEUDORANGE && i <= CustomDataModel.LABEL_GDOP)
                            {
                                mDataInfoLabelList[i].Content = "----";
                            }
                        }
                    }
                }

                // 如果定位标志可更新
                if (mModel.MCustomDataModel.DataBaseList[CustomDataModel.POS_STATE].IsUpdate)
                {
                    // 获取id号
                    int id = Convert.ToInt32(mModel.MCustomDataModel.DataBaseList[CustomDataModel.POS_STATE].Info);

                    // 如果id正确
                    if (id == User_Model.GGA_POS_SUCCESSFUL_IDENTIFIER1 || id == User_Model.GGA_POS_SUCCESSFUL_IDENTIFIER2)
                    {
                        // 激活状态
                        GpsState.SetActive();
                    }
                    else
                    {
                        GpsState.SetUnactive();
                    }
                    mModel.MCustomDataModel.DataBaseList[CustomDataModel.POS_STATE].IsUpdate = false;
                }
            }));
        }

        /// <summary>
        /// 串口开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_connect_com_Click(object sender, MouseButtonEventArgs e)
        {
            if (mControl.SerialIsOpen())
            {
                mControl.SerialClose();        
            }
            else
            {
                string str = combox_baudRate.Text;
                if (str == null || str == "")
                {
                    MessageBox.Show("Please Set BaudRate");
                    return;
                }
                string name = commbox_com.Text;
                if (name == null || name == "")
                {
                    MessageBox.Show("Please Set Com");
                    return;
                }
                mControl.OpenSerialPort(name, int.Parse(str));
            }
        }

        private void commbox_com_MouseEnter(object sender, MouseEventArgs e)
        {
            // 取得当前可用的串口
            commbox_com.ItemsSource = mModel.GetOnlineComName();
        }

        private void WrapPanel_MouseEnterLongWarp(object sender, MouseEventArgs e)
        {
            LongStd_Grid.Visibility = Visibility.Visible;
        }

        private void WrapPanel_MouseLeaveLongWarp(object sender, MouseEventArgs e)
        {
            LongStd_Grid.Visibility = Visibility.Hidden;
        }

        private void WrapPanel_MouseEnterLatWarp(object sender, MouseEventArgs e)
        {
            LatStd_Grid.Visibility = Visibility.Visible;
        }

        private void WrapPanel_MouseLeaveLatWarp(object sender, MouseEventArgs e)
        {
            LatStd_Grid.Visibility = Visibility.Hidden;
        }
    }
}
