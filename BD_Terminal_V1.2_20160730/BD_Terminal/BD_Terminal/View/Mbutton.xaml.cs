using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD_Terminal.View
{
    /// <summary>
    /// Mbutton.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// Mbutton.xaml 的交互逻辑
    /// </summary>
    public partial class Mbutton : UserControl
    {
        // 委托状态声明
        public delegate void mClickHandler(object sender);

        // 状态定义
        public const bool Active = true;
        public const bool UnActive = false;
        // 点击缩放倍数
        private const double CLICK_ZOOM_PARA = 0.8;
        // 存放字体大小，初始化时设置
        private double mDefFontSize = 0;
        // 指示状态
        public bool mState = false;

        // 激活颜色
        private Color mUnActiveColor = Color.FromArgb(0xFF, 0xAD, 0xBC, 0xC9);
        private Color mActiveColor = Color.FromArgb(0xFF, 0x92, 0xCB, 0x59);
        private Color mSelectColor = Color.FromArgb(0xFF, 0x86, 0xC3, 0xF5);

        // 鼠标点击状态维持
        private const int ClickState_Free = 0;
        private const int ClickState_Down = 1;
        private int ClickState = ClickState_Free;

        // 事件声明
        private mClickHandler mClickEvent;

        public mClickHandler ClickEvent
        {
            set { mClickEvent += value; }
        }


        public bool MState
        {
            get { return mState; }
            set { mState = value; }
        }

        public string mContent
        {
            get { return (string)InLabel.Content; }
            set { InLabel.Content = value; }
        }

        public double mFontSize
        {
            get { return (double)InLabel.FontSize; }
            set { InLabel.FontSize = value; mDefFontSize = value; }
        }

        /// <summary>
        /// 按键初始化
        /// </summary>
        public Mbutton()
        {
            InitializeComponent();

            // 初始化外圆
            OutEill.Width = this.Width;
            OutEill.Height = this.Height;

            // 初始化事件圆
            EventEill.Width = this.Width;
            EventEill.Height = this.Height;

            // 初始化label
            InLabel.Width = this.Width;
            InLabel.Height = this.Height;

            
            //OutEill_MouseLeave(null, null);
            //Set_StateAuto(ref mState);
            // 获取字体大小
            mDefFontSize = InLabel.FontSize;
        }

        public void SetState(bool active)
        {
            mState = active;

            if (mState == Active)
            {
                OutEill.Stroke = new SolidColorBrush(mActiveColor);
                InLabel.Foreground = new SolidColorBrush(mActiveColor);

                // 重置按键
                ClickState = ClickState_Free;
            }
            else if (mState == UnActive)
            {
                OutEill.Stroke = new SolidColorBrush(mUnActiveColor);
                InLabel.Foreground = new SolidColorBrush(mUnActiveColor);
                
                // 重置按键
                ClickState = ClickState_Free;
            }        
        }

        // 鼠标进入区域
        private void OutEill_MouseEnter(object sender, MouseEventArgs e)
        {
            OutEill.Stroke = new SolidColorBrush(mSelectColor);
        }

        // 鼠标离开区域
        private void OutEill_MouseLeave(object sender, MouseEventArgs e)
        {
            InLabel.FontSize = mDefFontSize;

            if (mState == UnActive)
            {
                OutEill.Stroke = new SolidColorBrush(mUnActiveColor);
            }
            else if (mState == Active)
            {
                OutEill.Stroke = new SolidColorBrush(mActiveColor);
            }
        }
        // 鼠标按下
        private void OutEill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 设置label的字体大小
            InLabel.FontSize = mDefFontSize * CLICK_ZOOM_PARA;
            // 保证选中颜色
            OutEill.Stroke = new SolidColorBrush(mSelectColor);

            // 如果状态为空闲状态
            if (ClickState == ClickState_Free)
            {
                ClickState = ClickState_Down;
            }
        }
        // 鼠标松开
        private void OutEill_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 设置label的字体大小
            InLabel.FontSize = mDefFontSize;
            // 保证选中颜色
            OutEill.Stroke = new SolidColorBrush(mSelectColor);

            // 如果鼠标按下后又松开，触发点击事件
            if (ClickState == ClickState_Down)
            {
                ClickState = ClickState_Free;

                Set_StateAuto(ref mState);

                // 发出点击事件
                mClickEvent(this);
            }      
        }
        /// <summary>
        /// 设置激活状态
        /// </summary>
        public void Set_StateAuto(ref bool state)
        {
            if (state == Mbutton.Active)
            {
                // 设置状态
                state = Mbutton.UnActive;

                OutEill.Stroke = new SolidColorBrush(mUnActiveColor);
                InLabel.Foreground = new SolidColorBrush(mUnActiveColor);
            }
            else if (state == Mbutton.UnActive)
            {
                // 设置状态
                state = Mbutton.Active;

                OutEill.Stroke = new SolidColorBrush(mActiveColor);
                InLabel.Foreground = new SolidColorBrush(mActiveColor);
            }
            else
            {
                return;
            }
        }      
    }
}
