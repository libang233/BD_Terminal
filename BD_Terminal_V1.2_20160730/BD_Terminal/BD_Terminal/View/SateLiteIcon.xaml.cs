using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// SateLiteIcon.xaml 的交互逻辑
    /// </summary>
    public partial class SateLiteIcon : UserControl
    {
        public const int SATELITE_ICON_RAD_DEF = 8;
        public const float SATELITE_ICON_COORDINATE_OFFSET = SATELITE_ICON_RAD_DEF / 2;
        
        // 默认信息
        private double mAziInfo;
        private double mElvInfo;
        private Color mColor;
        private string mLabel;

        /// <summary>
        /// 初始化相关信息
        /// </summary>
        /// <param name="label">卫星的标签</param>
        /// <param name="aziinfo">卫星方位信息</param>
        /// <param name="elvinfo">卫星仰角信息</param>
        /// <param name="coler">颜色描述</param>
        public SateLiteIcon(string label, double aziinfo, double elvinfo, Color coler)
        {
            InitializeComponent();

            mAziInfo = aziinfo;
            mElvInfo = elvinfo;
            mColor = coler;
            mLabel = label;

            Satelite.Stroke = new SolidColorBrush(mColor);


            SateIdLabel.Content = mLabel;
            SateIdLabel.Foreground = new SolidColorBrush(mColor);

        }

        public SateLiteIcon()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 响应鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Satelite_MouseEnter(object sender, MouseEventArgs e)
        {
            SateInfoLabel.Content = "ID: " + mLabel + "\r\n" + "Azi: " + mAziInfo.ToString() +
                "\r\n" + "Elv: " + mElvInfo.ToString();

            SateInfoLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 响应鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Satelite_MouseLeave(object sender, MouseEventArgs e)
        {
            SateInfoLabel.Visibility = Visibility.Hidden;
        }
    }
}
