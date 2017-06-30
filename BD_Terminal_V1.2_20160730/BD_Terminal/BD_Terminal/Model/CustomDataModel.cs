using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace BD_Terminal.Model
{
    /// <summary>
    /// 自定义数据模型
    /// </summary>
    public class CustomDataModelItem
    {
        private string info;
        private bool isUpdate;

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        public bool IsUpdate
        {
            get { return isUpdate; }
            set { isUpdate = value; }
        }
    }

    /// <summary>
    /// 用户数据模型定义类，该类主要为数据模型和UI的中间层，该类用于存储相关刷新数据模型
    /// </summary>
    public class CustomDataModel
    {
        /*--------------------------------------Const-------------------------------------------*/
        public const int LABEL_LONGTITUDE = 0;          // 经度
        public const int LABEL_LATITUDE = 1;            // 纬度
        public const int LABEL_UTC_TIME = 2;            // utc time
        public const int LABEL_ALTITUDE = 3;            // 海拔高度 
        public const int LABEL_ELLIPSOIDAL_HEIGHT = 4;  // 椭球高度
        public const int LABEL_SATELLITE_NUM = 5;       // 卫星数
        public const int LABEL_POSITION_ACC = 6;        // 定位精度
        public const int LABEL_PSEUDORANGE = 7;         // 伪距
        public const int LABEL_PSEUDORANGERATE = 8;     // 伪距率
        public const int LABEL_GDOP = 9;                // GDOP
        public const int LABEL_PDOP = 10;               // PDOP
        public const int LABEL_GAS_COORDINATION_X = 11; // 高斯X坐标
        public const int LABEL_GAS_COORDINAIION_Y = 12; // 高斯Y坐标
        public const int STR_LONGTITUDE_STD = 13;       // 经度标准差字符串
        public const int STR_LATITUDE_STD = 14;         // 纬度标准差字符串   
        public const int TEXT_BOOK = 15;                // textout
        public const int POS_MOD = 16;                  // 定位模式
        public const int POS_STATE = 17;                // 定位标志
        public const int LIST_ITEMS_MAX = 18;           // 链表的长度

        // 线程扫描时间
        private const int DATA_MONITER_THREAD_PERIOD = 2000;

        // 卫星角度无效值
        public const double GPS_SAT_ANG_INVALID = 500.0;

        // 卫星仰角范围
        public const double GPS_SATELITE_ELV_LIMIT_LOW = 0.0;
        public const double GPS_SATELITE_ELV_LIMIT_HIGH = 90.0;

        // 卫星方位角范围
        public const double GPS_SATELITE_AZI_LIMIT_LOW = 0.0;
        public const double GPS_SATELITE_AZI_LIMIT_HIGH = 360.0;

        /*-----------------------------------PrivateData----------------------------------------*/
        // 存放基础数据模型
        private List<CustomDataModelItem> mDataBaseList;

        // 存放卫星信息的列表
        private List<SateLiteInfoItem> mSateLiteInfoList;

        // 整个数据监控线程
        private Thread mDataMoniterThread = null;

        // 用户模型
        private User_Model mModel;

        // 线程和主线程访问限制锁
        private static object SateLitesInfoListLock = new object();

        // 查询变量
        public bool[] Gps_Receiver_State = { false, false, false, false, false, false, false };

        public const int GPS_REV_STATE_GP_AVAILABLE = 0;
        public const int GPS_REV_STATE_GB_AVAILABLE = 1;
        public const int GPS_REV_STATE_GGA_AVAILABLE = 2;
        public const int GPS_REV_STATE_RMC_AVAILABLE = 3;
        public const int GPS_REV_STATE_GAS_AVAILABLE = 4;
        public const int GPS_REV_STATE_GSV_AVAILABLE = 5;
        public const int GPS_REV_STATE_HPD_AVAILABLE = 6;

        // 消息
        public Queue<string> Rev_Msg_Queue = new Queue<string>();


        /// <summary>
        /// 恢复类的初始状态
        /// </summary>
        public void Reset()
        {
            mSateLiteInfoList.Clear();
        }

        /*------------------------------------Attribute-----------------------------------------*/
        public List<SateLiteInfoItem> SateLiteInfoList
        {
            get { return mSateLiteInfoList; }
            set { mSateLiteInfoList = value; }
        }

        public List<CustomDataModelItem> DataBaseList
        {
            get { return mDataBaseList; }
            set { mDataBaseList = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomDataModel(User_Model mModel)
        {
            // 初始化链表
            mDataBaseList = new List<CustomDataModelItem>();
            mSateLiteInfoList = new List<SateLiteInfoItem>();
            this.mModel = mModel;
            // 初始化数据链表
            for (int i = 0; i < CustomDataModel.LIST_ITEMS_MAX; i++)
            {
                mDataBaseList.Insert(i, new CustomDataModelItem());
            }

            // 创建监控线程
            mDataMoniterThread = new Thread(Data_Moniter);
            // 设置为后台线程
            mDataMoniterThread.IsBackground = true;
            // 启动线程
            mDataMoniterThread.Start();
        }

        /// <summary>
        /// 析构函数，释放其相关内容
        /// </summary>
        ~CustomDataModel()
        {
            // 关闭线程
            mDataMoniterThread.Abort();

            // 从内存注销
            mDataMoniterThread = null;
            mSateLiteInfoList = null;
            mDataBaseList = null;
        }

        /// <summary>
        /// 智能动态更新数据
        /// </summary>
        /// <param name="item"></param>
        public void SateLiteStrengthData_Add_Intelligence(SateLiteInfoItem item)
        {
            // 上锁
            lock(SateLitesInfoListLock)
            {
                // 迭代元素
                for (int i = 0; i < mSateLiteInfoList.Count; i++)
                {
                    if (item.Label.Equals(mSateLiteInfoList[i].Label))
                    {
                        mSateLiteInfoList[i].Cno = item.Cno;  // 更新数值
                        mSateLiteInfoList[i].Azi = item.Azi;
                        mSateLiteInfoList[i].Elv = item.Elv;

                        mSateLiteInfoList[i].RstLifeCycle();  // 重置生命周期

                        item = null; // 释放掉内存

                        return; // 直接返回
                    }
                }
                if (item.Label.Contains("B"))
                {
                    item.MColor = item.mBDColor;
                }
                // 如果为新元素，则添加到链表中
                mSateLiteInfoList.Add(item);
            }
        }

        /// <summary>
        /// 数据监控器,如果元素的生命周期到了则移除
        /// </summary>
        public void Data_Moniter()
        {
            while (true)
            {
                // 上锁
                lock (SateLitesInfoListLock)
                {
                    // 如果列表中数据不为空
                    if (mSateLiteInfoList.Count != 0)
                    {
                        // 使用倒序进行扫描，移除元素后不会掉数
                        for (int i = mSateLiteInfoList.Count - 1; i >= 0; i--)
                        {
                            // 如果生命周期已到
                            if (mSateLiteInfoList[i].GetLifeState() == false)
                            {
                                // 丢弃该项
                                mSateLiteInfoList.RemoveAt(i);
                            }
                        }
                    }
                }
                //该线程执行周期为预定义设置
                Thread.Sleep(DATA_MONITER_THREAD_PERIOD);
            }
        }

    }
}
