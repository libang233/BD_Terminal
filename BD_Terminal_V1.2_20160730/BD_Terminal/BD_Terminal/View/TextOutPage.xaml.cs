using BD_Terminal.Control;
using Microsoft.Win32;
using System;
using System.Windows.Controls;


using BD_Terminal.Model;
using System.Text;

namespace BD_Terminal.View
{
    /// <summary>
    /// TextOutPage.xaml 的交互逻辑
    /// </summary>
    public partial class TextOutPage : Page
    {
        private User_Control mControl;
        private User_Model mModel;

        // 滚动允许标志位
        private bool IsEnableScrollToEnd = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="control"></param>
        /// <param name="model"></param>
        public TextOutPage(User_Model model, User_Control control)
        {
            InitializeComponent();
            mControl = control;
            mModel = model;
        }

        /// <summary>
        /// 析构函数，用于关闭线程，释放内存
        /// </summary>
        ~TextOutPage()
        {
        }

        /// <summary>
        /// 用于注销当前子窗体信息
        /// </summary>
        public void Log_Off()
        {
        }

        /// <summary>
        /// 唤醒窗体
        /// </summary>
        public void Awaken()
        {
        }

        /// <summary>
        /// 线程更新函数
        /// </summary>
        public void UpdateUI_Thread()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // 如果有可更新项
                if (mModel.MCustomDataModel.Rev_Msg_Queue.Count != 0)
                {
                    string item = mModel.MCustomDataModel.Rev_Msg_Queue.Dequeue();

                    DateTime datetimenow = DateTime.Now;

                    string strout = datetimenow.Hour.ToString() + ':' + datetimenow.Minute.ToString() + ':' + datetimenow.Second.ToString() +
                        "  " + item;

                    // 添加内容到显示框
                    textBox.AppendText(strout);

                    // 如果使能了写文件
                    mControl.WriteToFile(strout);

                    // 是否需要滚动
                    if (IsEnableScrollToEnd)
                    {
                        textBox.ScrollToEnd();
                    }
                }
            }));
        }

        
        private void Btn_ClearOperation(object sender)
        {
            textBox.Text = "";

            // 该按钮不需要状态显示
            ((Mbutton)sender).SetState(false);
        }

        private void Btn_StopRollOperation(object sender)
        {
            IsEnableScrollToEnd = !IsEnableScrollToEnd;
        }

        private void Btn_Save_To_File(object sender)
        {
            if (ControlSaveFile.mContent.ToString() == "SAVE")
            {
                // 打开文件保存对话框
                SaveFileDialog filedialog = new SaveFileDialog();
                // 设置默认后缀名
                filedialog.DefaultExt = ".txt";
                // 自动添加后缀名
                filedialog.AddExtension = true;
                // 记忆上一次打开的路径
                filedialog.RestoreDirectory = true;
                Nullable<bool> result = filedialog.ShowDialog();
                // 如果选择成功
                if (result == true)
                {
                    string filePath = filedialog.FileName;
                    mControl.OpenFile(filePath);
                    ControlSaveFile.mContent = "STOP";
                    ControlSaveFile.SetState(true);
                }
                else
                {
                    ControlSaveFile.mContent = "SAVE";
                    ControlSaveFile.SetState(false);
                }
            }
            else
            {
                mControl.CloseFile();
                ControlSaveFile.mContent = "SAVE";
                ControlSaveFile.SetState(false);
            }
        }
    }
}
