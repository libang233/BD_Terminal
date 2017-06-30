using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using BD_Terminal.Model;
using BD_Terminal.Control;

namespace BD_Terminal.View
{
    /// <summary>
    /// Histogram.xaml 的交互逻辑
    /// </summary>
    public partial class Msshistogram : Page
    {
        // 默认标题栏占用的高度
        private const double MXLABEL_HEIGHT = 25.0;
        private const double MTITLE_HEIGHT = 30.0;
        private const double MTITLE_WIDTH = 100.0;
        private const double MTITLE_FONTSIZE = 15.0;

        // 最多绘制的直方图个数
        private const double HISTOGRAM_ITEM_NUM_MAX = 25.0;

        // x轴绘制偏移
        private const double X_AXIS_OFFSET = 0;

        // Title
        private string strTitle;
        double fItemWidth = 0.0;

        // 自定义控件
        private Canvas mCanvas;
        private Label mTitle;

        // 绘制数据
        private List<SateLiteInfoItem> mDataList;

        // 标题颜色# FFADBCC9
        private Color mTitleColor = Color.FromArgb(0xFF, 0xAD, 0xBC, 0xC9);
        // 控制器
        User_Control mControl;
        // 模型
        User_Model mModel;

        // 设置刷新时间
        private const int M_UPDATE_TIME = 50;

        /*-----------------------------------------------------------------------------------
        Attribute
        -------------------------------------------------------------------------------------*/
        public string StrTitle
        {
            get { return strTitle; }
            set { strTitle = value; }
        }

        /*-----------------------------------------------------------------------------------
        PublicFuc
        -------------------------------------------------------------------------------------*/
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strTitle">标题</param>
        /// <param name="list">待绘制的数据源</param>
        public Msshistogram(string title, User_Model model, User_Control control)
        {
            InitializeComponent();

            strTitle = title;
            mControl = control;
            mModel = model;

            mDataList = mModel.MCustomDataModel.SateLiteInfoList;
            Panel_Init();
        }

        /// <summary>
        /// 析构函数，终止刷新线程，释放内存
        /// </summary>
        ~Msshistogram()
        {

        }

        /// <summary>
        /// 用于注销当前子窗体信息
        /// </summary>
        public void Log_Off()
        {
            // 清除画面
            mCanvas.Children.Clear();
        }

        /// <summary>
        /// 唤醒窗体
        /// </summary>
        public void Awaken()
        {
        }

        /*-----------------------------------------------------------------------------------
        PrivateFuc
        -------------------------------------------------------------------------------------*/
        /// <summary>
        /// 用于子界面的初始化
        /// </summary>
        private void Panel_Init()
        {
            // 初始化标题栏
            mTitle = new Label()
            {
                Width = this.Width,
                Height = MTITLE_HEIGHT,
                Margin = new Thickness(0, 0, 0, this.Height - MTITLE_HEIGHT),
                FontSize = MTITLE_FONTSIZE,
                //HorizontalContentAlignment = HorizontalAlignment.Left,
                Content = strTitle,
                Foreground = new SolidColorBrush(mTitleColor)
            };

            // 初始化画布
            mCanvas = new Canvas()
            {
                Width = this.Width,
                Height = this.Height - MTITLE_HEIGHT,
                Margin = new Thickness(0, MTITLE_HEIGHT, 0, 0)
            };

            // 添加到Grid中
            mGrid.Children.Add(mCanvas);
            mGrid.Children.Add(mTitle);

            // 初始化坐标
            // 绘制y坐标
            mGrid.Children.Add(new Line
            {
                X1 = 1,
                Y1 = MTITLE_HEIGHT,
                X2 = 1,
                Y2 = this.Height,
                Stroke = new SolidColorBrush(mTitleColor),
                StrokeThickness = 2
            });

            // 绘制x坐标
            mGrid.Children.Add(new Line
            {
                X1 = 0,
                Y1 = this.Height - 1,
                X2 = this.Width,
                Y2 = this.Height - 1,
                Stroke = new SolidColorBrush(mTitleColor),
                StrokeThickness = 2
            });

            // 计算每个项的宽度
            fItemWidth = this.Width / (double)HISTOGRAM_ITEM_NUM_MAX;
        }

        /// <summary>
        /// 线程绘制函数,用于实时绘制直方图
        /// </summary>
        public void UpdateUI_Thread()
        {
            // 如果数据列表为空或含有数量为0
            if (mDataList != null && mDataList.Count != 0)
            {
                // 定义变量
                int i = 0;
                double ftmp_h = 0.0;

                // 更新UI
                mCanvas.Dispatcher.Invoke(new Action(() =>
                {
                    // 清除画布
                    mCanvas.Children.Clear();

                    // 如果串口关闭则不绘制信噪比直方图
                    if (mControl.SerialIsOpen())
                    {
                        // 迭代绘制
                        for (i = mDataList.Count - 1; i >= 0; i--)
                        {
                            ftmp_h = (mCanvas.Height / mDataList[i].CnoMax) * mDataList[i].Cno;

                            // 绘制矩形
                            mCanvas.Children.Add(new Rectangle
                            {
                                Height = ftmp_h,
                                Width = fItemWidth,
                                Stroke = Brushes.BlueViolet,
                                StrokeThickness = 0.0,
                                Fill = new SolidColorBrush(mDataList[i].MColor),
                                Margin = new Thickness(i * fItemWidth + X_AXIS_OFFSET, mCanvas.Height - ftmp_h, mCanvas.Width - (i * fItemWidth), 0)
                            });

                            // 绘制标题
                            mCanvas.Children.Add(new Label
                            {
                                FontSize = 8,
                                Width = fItemWidth,
                                Height = MXLABEL_HEIGHT,
                                Content = mDataList[i].Label,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                Margin = new Thickness(i * fItemWidth + X_AXIS_OFFSET, mCanvas.Height, mCanvas.Width - (i * fItemWidth), 0)
                            });

                            // 如果柱状项顶部无法容纳label，则放入到柱状内部
                            double tmp_height = (ftmp_h >= (mCanvas.Height - MXLABEL_HEIGHT)) ? (mCanvas.Height - ftmp_h) : (mCanvas.Height - ftmp_h - MXLABEL_HEIGHT);

                            // 绘制值
                            mCanvas.Children.Add(new Label
                            {
                                FontSize = 8,
                                Width = fItemWidth,
                                Height = MXLABEL_HEIGHT,
                                Content = mDataList[i].Cno,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                Margin = new Thickness(i * fItemWidth + X_AXIS_OFFSET, tmp_height, mCanvas.Width - (i * fItemWidth), 0)
                            });

                            // 绘制分界线 
                            mCanvas.Children.Add(new Line
                            {
                                X1 = (i + 1) * fItemWidth + X_AXIS_OFFSET,
                                Y1 = mCanvas.Height,
                                X2 = (i + 1) * fItemWidth + X_AXIS_OFFSET,
                                Y2 = mCanvas.Height - ftmp_h,
                                Stroke = Brushes.Black,
                                StrokeThickness = 0.5
                            });
                        }
                    }
                }));
            }
            else
            {
                mCanvas.Dispatcher.Invoke(new Action(() =>
                {
                    // 清除画布
                    mCanvas.Children.Clear();
                }));
            }
        }  
    }
}
