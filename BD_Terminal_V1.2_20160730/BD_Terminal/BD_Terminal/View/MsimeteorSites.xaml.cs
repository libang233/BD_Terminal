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
using System.Threading;


using BD_Terminal.Model;
using BD_Terminal.Control;

namespace BD_Terminal.View
{
    /// <summary>
    /// MeteorSites.xaml 的交互逻辑
    /// </summary>
    public partial class MsimeteorSites : Page
    {
        private const int OUT_ELLIPSE_DIAMETER = 150 +SateLiteIcon.SATELITE_ICON_RAD_DEF ;
        private const double OUT_ELLPSE_MARGIN_LEFT = 96.0-SateLiteIcon.SATELITE_ICON_COORDINATE_OFFSET;
        private const double CIRCLE_CENTER_X = OUT_ELLPSE_MARGIN_LEFT + OUT_ELLIPSE_DIAMETER / 2;
        private const double CIRCLE_CENTER_Y = OUT_ELLIPSE_DIAMETER / 2;
      
        // 数据源
        private List<SateLiteInfoItem> mDataList;

        // 标题颜色# FFADBCC9
        private Color mDrawColor = Color.FromArgb(0xFF, 0xAD, 0xBC, 0xC9);

        // 控制器
        private User_Control mControl;

        // 模型
        private User_Model mModel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">用户模型</param>
        public MsimeteorSites(User_Model model, User_Control control)
        {
            InitializeComponent();

            // 设置模型和控制器
            mControl = control;
            mModel = model;

            // 设置数据源
            mDataList = mModel.MCustomDataModel.SateLiteInfoList;

            // 初始化布局
            Panel_Init();
        }

        /// <summary>
        /// 析构函数，关闭线程
        /// </summary>
        ~MsimeteorSites()
        {

        }

        /// <summary>
        /// 用于注销当前子窗体信息
        /// </summary>
        public void Log_Off()
        {
            // 清除画面
            mCanvasDraw.Children.Clear();
        }

        /// <summary>
        /// 唤醒窗体
        /// </summary>
        public void Awaken()
        {
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void Panel_Init()
        {
            Ellipse mOutEllips = new Ellipse()
            {
                Width = OUT_ELLIPSE_DIAMETER,
                Height = OUT_ELLIPSE_DIAMETER,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT, 0, 0, 0),
                Stroke = new SolidColorBrush(mDrawColor),
                StrokeThickness = 2
            };

            Ellipse mInEllips = new Ellipse()
            {
                Width = mOutEllips.Width / 2,
                Height = mOutEllips.Height / 2,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT + mOutEllips.Width / 4, mOutEllips.Width / 4, 0, 0),
                Stroke = new SolidColorBrush(mDrawColor),
                StrokeThickness = 2
            };

            Label labeln = new Label()
            {
                Content = "N",
                FontSize = 10,
                Foreground = Brushes.Black,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT + mOutEllips.Width / 2 - 8, -3, 0, 0)
            };

            Label labels = new Label()
            {
                Content = "S",
                FontSize = 10,
                Foreground = Brushes.Black,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT + mOutEllips.Width / 2 - 8, OUT_ELLIPSE_DIAMETER - 17, 0, 0)
            };

            Label labelw = new Label()
            {
                Content = "W",
                FontSize = 10,
                Foreground = Brushes.Black,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT, mOutEllips.Width / 2 - 8, 0, 0)
            };

            Label labele = new Label()
            {
                Content = "E",
                FontSize = 10,
                Foreground = Brushes.Black,
                Margin = new Thickness(OUT_ELLPSE_MARGIN_LEFT + OUT_ELLIPSE_DIAMETER - 15, mOutEllips.Width / 2 - 10, 0, 0)
            };

            mCanvasPanel.Children.Add(mOutEllips);
            mCanvasPanel.Children.Add(mInEllips);
            mCanvasPanel.Children.Add(labeln);
            mCanvasPanel.Children.Add(labels);
            mCanvasPanel.Children.Add(labelw);
            mCanvasPanel.Children.Add(labele);
        }

        /// <summary>
        /// 更新线程
        /// </summary>
        public void UpdateUI_Thread()
        {
            int i;
            // 如果数据列表为空或含有数量为0
            if (mDataList != null && mDataList.Count != 0)
            {
                // 更新UI
                mCanvasDraw.Dispatcher.Invoke(new Action(() =>
                {
                    // 清除画布
                    mCanvasDraw.Children.Clear();
                    #region
                    for (i = 0; i < mDataList.Count; i++)
                    {

                        double azi = mDataList[i].Azi;
                        double elv = mDataList[i].Elv;

                        if (azi < CustomDataModel.GPS_SATELITE_AZI_LIMIT_LOW || azi > CustomDataModel.GPS_SATELITE_AZI_LIMIT_HIGH)
                        {
                            continue;
                        }

                        if (elv < CustomDataModel.GPS_SATELITE_ELV_LIMIT_LOW || elv == CustomDataModel.GPS_SATELITE_ELV_LIMIT_HIGH)
                        {
                            continue;
                        }

                        // 获取与圆心的连线长度
                        double len = Math.Cos(elv / 180 * Math.PI) * (OUT_ELLIPSE_DIAMETER / 2);
                        len = Math.Floor(len);
                        double x = len * Math.Sin(azi / 180 * Math.PI);
                        x = Math.Floor(x);
                        double y = len * Math.Cos(azi / 180 * Math.PI);
                        y = Math.Floor(y);
                        SateLiteIcon Satelite = new SateLiteIcon(mDataList[i].Label, mDataList[i].Azi, mDataList[i].Elv, mDataList[i].MColor)
                        {
                            Width = SateLiteIcon.SATELITE_ICON_RAD_DEF,
                            Height = SateLiteIcon.SATELITE_ICON_RAD_DEF,
                            Margin = new Thickness(CIRCLE_CENTER_X + x - SateLiteIcon.SATELITE_ICON_COORDINATE_OFFSET,
                                   (CIRCLE_CENTER_Y - y) - SateLiteIcon.SATELITE_ICON_COORDINATE_OFFSET, 0, 0)
                        };
                        mCanvasDraw.Children.Add(Satelite);
                    }
                    #endregion
                }));
            }
            else
            {
                mCanvasDraw.Dispatcher.Invoke(new Action(() =>
                {
                    mCanvasDraw.Children.Clear();
                }));
            }
        }   
    }
}
