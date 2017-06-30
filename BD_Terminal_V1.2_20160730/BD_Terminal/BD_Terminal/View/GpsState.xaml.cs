using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BD_Terminal.View
{
    /// <summary>
    /// GpsState.xaml 的交互逻辑
    /// </summary>
    public partial class GpsState : UserControl
    {
        private bool isActive;
        private BitmapImage bi3Active;
        private BitmapImage bi3Unactive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public GpsState()
        {
            InitializeComponent();
            bi3Active = new BitmapImage();
            bi3Active.BeginInit();
            bi3Active.UriSource = new Uri("/Resources/gps_active.png", UriKind.Relative);
            bi3Active.EndInit();

            bi3Unactive = new BitmapImage();
            bi3Unactive.BeginInit();
            bi3Unactive.UriSource = new Uri("/Resources/gps_unactive.png", UriKind.Relative);
            bi3Unactive.EndInit();
            isActive = false;
        }

        public void SetActive()
        {
           if (!isActive)
            {
                mImage.Source = null; // 将原来对象置为null
                mImage.Source = bi3Active;
                isActive = true;
            }
        }

        public void SetUnactive()
        {
            if(isActive)
            {
              //  mImage.Source = null;
                mImage.Source = bi3Unactive;
                isActive = false;
            }
        }
    }
}
