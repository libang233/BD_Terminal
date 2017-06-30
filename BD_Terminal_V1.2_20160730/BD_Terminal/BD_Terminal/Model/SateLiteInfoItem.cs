using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace BD_Terminal.Model
{
    /// <summary>
    /// 用于存储单颗卫星的相关信息，每个卫星有一个生命周期参数，用于提供给线程进行扫描。
    /// </summary>
    public class SateLiteInfoItem
    {
        /*--------------------------------------Const-------------------------------------------*/
        public const int LIFE_CYCLE_LEN = 4; // 存储最大生命周期


        /*-----------------------------------PrivateData----------------------------------------*/
        private string mLabel;               // 当前名称
        private double mCno;                 // 信号强度值
        private double mCnoMax;              // 信号强度最大值
        private int mLifeCycle;              // 生命周期

        private double mAzi;                 // 卫星方位角
        private double mElv;                 // 卫星仰角
        private Color mColor = Color.FromArgb(0xFF, 0x86, 0xC3, 0xF5); // 默认颜色
        public  Color mBDColor = Color.FromArgb(255, 0, 255, 0); // 北斗卫星颜色 绿色;

        /*------------------------------------Attribute-----------------------------------------*/
        #region
        // 设置描绘颜色
        public Color MColor
        {
            get { return mColor; }
            set { mColor = value; }
        }

        // 设置卫星的信息
        public string Label
        {
            get { return mLabel; }
            set { mLabel = value; }
        }

        // 用于返回信号强度
        public double Cno
        {
            get { return mCno; }
            set { mCno = value; }
        }

        // 用于返回信号的最大强度
        public double CnoMax
        {
            get { return mCnoMax; }
            set { mCnoMax = value; }
        }

        // 用于设置或返回方位角度值
        public double Azi
        {
            get { return mAzi; }
            set { mAzi = value; }
        }

        // 用于设置或返回俯仰角度
        public double Elv
        {
            get { return mElv; }
            set { mElv = value; }
        }

        #endregion
        /*------------------------------------PublicFuc-----------------------------------------*/
        #region
        /// <summary>
        /// 构造函数，初始化相关卫星信息
        /// </summary>
        /// <param name="label">卫星信息</param>
        /// <param name="cno">卫星信号强度</param>
        /// <param name="cnomax">卫星信号强度最大值</param>
        public SateLiteInfoItem(string label, double cno, double cnomax)
        {
            // 设置卫星的相关信息
            mLabel = label;
            Cno = cno;
            CnoMax = cnomax;

            // 重置生命周期
            RstLifeCycle();
        }

        /// <summary>
        /// 构造函数，初始化相关卫星信息
        /// </summary>
        /// <param name="label">卫星id</param>
        /// <param name="val">卫星的信号强度</param>
        /// <param name="valmax">卫星信号强度最大值</param>
        /// <param name="azi">卫星方位角</param>
        /// <param name="elv">卫星仰角</param>
        public SateLiteInfoItem(string label, double cno, double cnomax, double azi, double elv)
        {
            // 设置卫星的相关信息
            mLabel = label;
            Cno = cno;
            CnoMax = cnomax;
            mAzi = azi;
            mElv = elv;

            // 重置生命周期
            RstLifeCycle();
        }

        /// <summary>
        /// 重置生命周期
        /// </summary>
        public void RstLifeCycle()
        {
            // 重置生命周期
            mLifeCycle = LIFE_CYCLE_LEN;
        }

        /// <summary>
        /// 用于返回该对象的生命状态，使用外部线程检查生命状态并返回，如果生命状态为0，则丢弃该对象
        /// </summary>
        /// <returns>生命状态</returns>
        public bool GetLifeState()
        {
            if (mLifeCycle-- == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
