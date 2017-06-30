using System;
using System.Windows.Controls;
using BD_Terminal.Model;
using BD_Terminal.Control;

namespace BD_Terminal.View
{
    /// <summary>
    /// ConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigPage : Page
    {
        // 刷新分频
        private const int DIV_UPDATE_TIME = 30;
        // 刷新时间种
        private int TimeCount = DIV_UPDATE_TIME;
        // 点击保持状态
        private int ClickHoldState_LifeCyc = 3;
        // 用户模型
        private User_Model mModel;
        // 用户控制器
        private User_Control mControl;

        // 用于点击后状态保持计数器
        private int ClickHoldStateCount = 0;

        // 用于封装mButton类
        private const int MBUTTON_NUM_MAX = 7;
        private Mbutton[] mButtonList = new Mbutton[MBUTTON_NUM_MAX];

        // 点击事件
        private bool IsSelfClick = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="control">控制模型</param>
        /// <param name="mode">数据模型</param>
        public ConfigPage(User_Model mode, User_Control control)
        {
            InitializeComponent();
            // 设置模型
            mModel = mode;
            // 设置控制器
            mControl = control;

            // 初始化mButton列表
            mButtonList[CustomDataModel.GPS_REV_STATE_GP_AVAILABLE] = ControlGP;
            mButtonList[CustomDataModel.GPS_REV_STATE_GB_AVAILABLE] = ControlGB;
            mButtonList[CustomDataModel.GPS_REV_STATE_GGA_AVAILABLE] = ControlGGA;
            mButtonList[CustomDataModel.GPS_REV_STATE_RMC_AVAILABLE] = ControlRMC;
            mButtonList[CustomDataModel.GPS_REV_STATE_GAS_AVAILABLE] = ControlGAS;
            mButtonList[CustomDataModel.GPS_REV_STATE_GSV_AVAILABLE] = ControlGSV;
            mButtonList[MBUTTON_NUM_MAX - 1] = ControlDebug;
        }


        /// <summary>
        /// 用于注销当前子窗体信息
        /// </summary>
        public void Log_Off()
        {
            // 注销界面
            for(int i = 0; i < MBUTTON_NUM_MAX - 1; i++)
            {
                mButtonList[i].SetState(Mbutton.UnActive);
                // 刷新缓冲
                mModel.MCustomDataModel.Gps_Receiver_State[i] = false;
            }
        }

        /// <summary>
        /// 唤醒窗体
        /// </summary>
        public void Awaken()
        {
        }

        /// <summary>
        /// 更新线程
        /// </summary>
        public void UpdateUI_Thread()
        {
            // 如果没有点击事件，则进行正常刷新
            if (--TimeCount == 0)
            {
                if (!IsSelfClick)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        // 更新按键状态
                        for (int i = 0; i < MBUTTON_NUM_MAX - 1; i++)
                        {
                            // 查询状态
                            if (mModel.MCustomDataModel.Gps_Receiver_State[i])
                            {
                                mModel.MCustomDataModel.Gps_Receiver_State[i] = false;
                                mButtonList[i].SetState(true);
                            }
                            else
                            {
                                mButtonList[i].SetState(false);
                            }
                        }
                    }));
                } // 如果点击，则让线程休眠4s再进行刷新
                else
                {
                    // 将缓冲全部刷新
                    for (int i = 0; i < MBUTTON_NUM_MAX - 1; i++)
                    {
                        mModel.MCustomDataModel.Gps_Receiver_State[i] = false;
                    }

                    if (ClickHoldStateCount-- <= 0)
                    {
                        // 点击保持解除
                        IsSelfClick = false;
                    }
                }
                TimeCount = DIV_UPDATE_TIME;
            }
        }

        /// <summary>
        /// 活跃点击事件
        /// </summary>
        private void Active_Click()
        {
            if (IsSelfClick)
            {
                // 更新点击保持时间
                ClickHoldStateCount = ClickHoldState_LifeCyc;
            }
            else
            {
                IsSelfClick = true;

                // 更新点击保持时间
                ClickHoldStateCount = ClickHoldState_LifeCyc;
            }
        }


        /// <summary>
        /// 响应鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mClickDeal(object sender)
        {
            // 按键按下
            Active_Click();

            // 如果模式按键按下
            if (((Mbutton)sender).Name == "ControlGP" || ((Mbutton)sender).Name == "ControlGB")
            {
                // 如果俩个按键都激活
                if (ControlGP.mState == Mbutton.Active && ControlGB.mState == Mbutton.Active)
                {
                    mControl.Serialwrite(User_Model.ENABLE_GN);

                    return;
                }

                // 如果只有GP激活
                if (ControlGP.mState == Mbutton.Active)
                {
                    mControl.Serialwrite(User_Model.ENABLE_GP);

                    return;
                }

                // 如果只有GB激活
                if (ControlGB.mState == Mbutton.Active)
                {
                    mControl.Serialwrite(User_Model.ENABLE_BD);

                    return;
                }
            }

            if (((Mbutton)sender).Name == "ControlGGA")
            {
                if (ControlGGA.MState == Mbutton.UnActive)
                {
                    mControl.Serialwrite(User_Model.DISABLE_GGA);
                    
                }
                else
                {
                    mControl.Serialwrite(User_Model.ENABLE_GGA);
                }
                return;
            }

            if (((Mbutton)sender).Name == "ControlRMC")
            {
                if (ControlRMC.MState == Mbutton.UnActive)
                {
                    mControl.Serialwrite(User_Model.DISABLE_RMC); 
                }
                else
                {
                    mControl.Serialwrite(User_Model.ENABLE_RMC);
                }
                return;
            }

            if (((Mbutton)sender).Name == "ControlGAS")
            {
                if (ControlGAS.MState == Mbutton.UnActive)
                {
                    mControl.Serialwrite(User_Model.DISABLE_GAS); 
                }
                else
                {
                    mControl.Serialwrite(User_Model.ENABLE_GAS);
                }
                return;
            }

            if (((Mbutton)sender).Name == "ControlGSV")
            {
                if (ControlGSV.MState == Mbutton.UnActive)
                {
                    mControl.Serialwrite(User_Model.DISABLE_GSV);
                }
                else
                {
                    mControl.Serialwrite(User_Model.ENABLE_GSV);
                }
                return;
            }

            if (((Mbutton)sender).Name == "ControlDebug")
            {
                if (mControl.SerialIsOpen())
                {
                    if (ControlDebug.mState == false)
                    {
                        mControl.Serialwrite(User_Model.DISABLE_BEBUG);
                    }
                    else
                    {
                        mControl.Serialwrite(User_Model.ENABLE_BEBUG);
                    }
                }
            }
        }
    }
}
