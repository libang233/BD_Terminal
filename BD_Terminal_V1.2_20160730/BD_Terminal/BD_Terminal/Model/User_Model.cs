
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BD_Terminal.Model
{
    public class User_Model
    {
        /*--------------------------------------Const-------------------------------------------*/
        // 需要解析的NMEA帧默认声明
        public const string NMEA_FRAME_TALKERID_GN = "GN";
        public const string NMEA_FRAME_TALKERID_GP = "GP";
        public const string NMEA_FRAME_TALKERID_BD = "BD";

        public const string NMEA_FRAME_CONTENTID_GGA = "GGA";
        public const string NMEA_FRAME_CONTENTID_RMC = "RMC";
        public const string NMEA_FRAME_CONTENTID_GSV = "GSV";
        public const string NMEA_FRAME_CONTENTID_GAS = "GAS";
        public const string NMEA_FRAME_CONTENTID_GSA = "GSA";
        public const string NMEA_FRAME_CONTENTID_HPD = "HPD";

        // GGA
        public const int NMEA_FRAME_GGA_POS_TIME = 0;
        public const int NMEA_FRAME_GGA_POS_LAT = 1;
        public const int NMEA_FRAME_GGA_POS_NS = 2;
        public const int NMEA_FRAME_GGA_POS_LONG = 3;
        public const int NMEA_FRAME_GGA_POS_EW = 4;
        public const int NMEA_FRAME_GGA_POS_QUALITY = 5;
        public const int NMEA_FRAME_GGA_POS_NUMSV = 6;
        public const int NMEA_FRAME_GGA_POS_HDOP = 7;
        public const int NMEA_FRAME_GGA_POS_ALT = 8;
        public const int NMEA_FRAME_GGA_POS_UALT = 9;
        public const int NMEA_FRAME_GGA_POS_SEP = 10;
        public const int NMEA_FRAME_GGA_POS_USEP = 11;
        public const int NMEA_FRAME_GGA_POS_DIFFAGE = 12;
        public const int NMEA_FRAME_GGA_POS_DIFFSTATION = 13;

        public const int GGA_POS_SUCCESSFUL_IDENTIFIER1 = 1;
        public const int GGA_POS_SUCCESSFUL_IDENTIFIER2 = 2;

        // RMC
        public const int NMEA_FRAME_RMC_POS_TIME = 0;
        public const int NMEA_FRAME_RMC_POS_STATUS = 1;
        public const int NMEA_FRAME_RMC_POS_LAT = 2;
        public const int NMEA_FRAME_RMC_POS_NS = 3;
        public const int NMEA_FRAME_RMC_POS_LONG = 4;
        public const int NMEA_FRAME_RMC_POS_EW = 5;
        public const int NMEA_FRAME_RMC_POS_SPD = 6;
        public const int NMEA_FRAME_RMC_POS_COG = 7;
        public const int NMEA_FRAME_RMC_POS_Date = 8;
        public const int NMEA_FRAME_RMC_POS_MV = 9;
        public const int NMEA_FRAME_RMC_POS_MVEW = 10;
        public const int NMEA_FRAME_RMC_POS_POSMODE = 11;
        public const int NMEA_FRAME_RMC_POS_NAVSTATUS = 11;

        //GSV
        public const int NMEA_FRAME_GSV_POS_NUMMSG = 0;
        public const int NMEA_FRAME_GSV_POS_MSGNUM = 1;
        public const int NMEA_FRAME_GSV_POS_NUMSV = 2;
        public const int NMEA_FRAME_GSV_POS_SATELLITE_SV = 3;
        public const int NMEA_FRAME_GSV_POS_SATELLITE_ELV = 4;
        public const int NMEA_FRAME_GSV_POS_SATELLITE_AZ = 5;
        public const int NMEA_FRAME_GSV_POS_SATELLITE_CNO = 6;
        public const int NMEA_FRAME_GSV_SATALITES_INFO_LEN = 4;
        public const int NMEA_FRAME_GSV_SIGNAL_ID_LEN = 1;
        public const int NMEA_FRAME_GSV_SIGNAL_STRENGTH_MAX = 60;

        //GSA
        public const int NMEA_FRAME_GSA_POS_OPMODE = 0;
        public const int NMEA_FRAME_GSA_POS_NAVMODE = 1;
        public const int NMEA_FRAME_GSA_POS_PDOP = 14;
        public const int NMEA_FRAME_GSA_POS_HDOP = 15;
        public const int NMEA_FRAME_GSA_POS_VDOP = 16;


        // GAS
        public const int NMEA_FRAME_GAS_POS_TIME  = 0;
        public const int NMEA_FRAME_GAS_POS_STATE = 1;
        public const int NMEA_FRAME_GAS_X_COODINATION = 2;
        public const int NMEA_FRAME_GAS_Y_COODINATION = 3;
        public const int NMEA_FRAME_GAS_PDOP = 6;
        public const int NMEA_FRAME_GAS_GAS_HEIGHT = 4;


        //HPD
        public const int NMEA_FRAME_HPD_POS_WEEK = 0;
        public const int NMEA_FRAME_HPD_POS_TIME = 1;
        public const int NMEA_FRAME_HPD_POS_HEADING = 2;
        public const int NMEA_FRAME_HPD_POS_PITCH = 3;
        public const int NMEA_FRAME_HPD_POS_TRACK = 4;
        public const int NMEA_FRAME_HPD_POS_LATITUDE = 5;
        public const int NMEA_FRAME_HPD_POS_LONGITUDE = 6;
        public const int NMEA_FRAME_HPD_POS_ALTITUDE = 7;
        public const int NMEA_FRAME_HPD_POS_VE = 8;
        public const int NMEA_FRAME_HPD_POS_VN = 9;
        public const int NMEA_FRAME_HPD_POS_VU = 10;
        public const int NMEA_FRAME_HPD_POS_AE = 11;
        public const int NMEA_FRAME_HPD_POS_AN = 12;
        public const int NMEA_FRAME_HPD_POS_AU = 13;
        public const int NMEA_FRAME_HPD_POS_BASELINE = 14;
        public const int NMEA_FRAME_HPD_POS_NSV1 = 15;
        public const int NMEA_FRAME_HPD_POS_NSV2 = 16;

        // 定位模式 position mode
        public const int POSITION_MODE_NULL = 0;
        public const int POSITION_MODE_GN = 1;
        public const int POSITION_MODE_GP = 2;
        public const int POSITION_MODE_BD = 3;

        // 所用状态声明
        public const int NMEA_FRAME_TYPE_SIZE = 6;
        public const int NMEA_FRAME_TYPE_GGA = 0;
        public const int NMEA_FRAME_TYPE_RMC = 1;
        public const int NMEA_FRAME_TYPE_GSV = 2;
        public const int NMEA_FRAME_TYPE_GAS = 3;
        public const int NMEA_FRAME_TYPE_GSA = 4;
        public const int NMEA_FRAME_TYPE_HPD = 5;
        public const int NMEA_FRAME_TYPE_NULL = NMEA_FRAME_TYPE_SIZE;

        // GSV错误接收限制数量
        private const int GSV_FRAME_NUM_COUNT_MAX = 6;

        // 接收机定位方式设置
        public const string ENABLE_GN = "$CCRMO,SPM,1*2F\r\n";  // 使能组合定位
        public const string ENABLE_GP = "$CCRMO,SPM,2*2C\r\n";  // 使能GP
        public const string ENABLE_BD = "$CCRMO,SPM,3*2D\r\n";  // 使能BD

        // 接收机输出的信息设置
        public const string ENABLE_GGA    = "$CCRMO,GGA,2,1.0*20\r\n";
        public const string DISABLE_GGA   = "$CCRMO,GGA,1,1.0*23\r\n";
        public const string ENABLE_RMC    = "$CCRMO,RMC,2,1.0*3D\r\n";
        public const string DISABLE_RMC   = "$CCRMO,RMC,1,1.0*3E\r\n";
        public const string ENABLE_GAS    = "$CCRMO,GAS,2,1.0*34\r\n";
        public const string DISABLE_GAS   = "$CCRMO,GAS,1,1.0*37\r\n";
        public const string ENABLE_GSV    = "$CCRMO,GSV,2,1.0*23\r\n";
        public const string DISABLE_GSV   = "$CCRMO,GSV,1,1.0*20\r\n";
        public const string ENABLE_GSA    = "$CCRMO,GSA,2,1.0*10\r\n";
        public const string DISABLE_GSA   = "$CCRMO,GSA,1,1.0*13\r\n";
        public const string ENABLE_BEBUG  = "$CCRMO,SAD,1*37\r\n";
        public const string DISABLE_BEBUG = "$CCRMO,SAD,0*36\r\n";


        // 经度计算数组
        private class CalStruct
        {
            // 用于计算经度和纬度的均方差数据量
            private const int CAL_STD_ARR_LENGTH = 60;

            private int Index;
            private bool IsArrFull;
            private double[] Arr;

            public CalStruct()
            {
                Index = 0;

                IsArrFull = false;

                Arr = new double[CAL_STD_ARR_LENGTH];

                for (int i = 0; i < CAL_STD_ARR_LENGTH; i++)
                {
                    Arr[i] = 0.0;
                }
            }

            /// <summary>
            /// 提供数据添加接口
            /// </summary>
            /// <param name="data"></param>
            public void AddData(double data)
            {
                Arr[Index++] = data;
                
                // 如果索引到达末尾
                if (Index >= CAL_STD_ARR_LENGTH)
                {
                    // 重置索引
                    Index = 0;

                    IsArrFull = true;
                }
            }

            /// <summary>
            ///  获取数组中的标准差
            /// </summary>
            /// <returns></returns>
            public double GetStdVal()
            {
                double Mean = 0.0;  // 平均值
                double Squre = 0.0; // 平方和平均
                int Length;

                // 如果数组满
                if (IsArrFull)
                {
                    for(int i = 0; i < Arr.Length; i++)
                    {
                        Mean += Arr[i];
                        Squre += (Arr[i] * Arr[i]);
                    }

                    Length = Arr.Length;
                }
                else
                {
                    for(int i = 0; i < Index; i++)
                    {
                        Mean += Arr[i];
                        Squre += (Arr[i] * Arr[i]);
                    }

                    Length = Index;
                }

                Mean /= ((double)Length);
                Squre /= ((double)Length);

                double StdVal = Math.Sqrt(Squre - Mean * Mean);

                return StdVal;
            }
        }

        private CalStruct LongtitudeDataArr = new CalStruct();
        private CalStruct LattitudeDataArr = new CalStruct();
        /*-----------------------------------PrivateData----------------------------------------*/
        #region

        // 提供给UI层的数据模型
        private CustomDataModel mCustomDataModel;

        // 帧缓冲
        private Gps_NmeaFrame[] mGpsMsgBuff;

        // 状态机相关声明
        enum NmeaFrame_ParseState
        {
            Null_State,
            Begin_State,
            Head_State,
            Data_State,
            Crc_State,
            End_State
        };
        // 临时用字符串缓冲
        private string mStrBuffer;
        // 主状态机
        private Dictionary<NmeaFrame_ParseState, Action<byte>> ParseMoniter;
        // 当前状态
        private NmeaFrame_ParseState Current_State;
        // 状态机计数器，用于产生复位
        private int ParseMoniterCnt = 0;
        // 临时指示帧类型
        private int mTmpFrameType = NMEA_FRAME_TYPE_NULL;
        // 校验位
        private byte Current_Crc = 0;

        // SerialPort相关定义
        private int mBaudRate;
        private string mPortName;
        private int mDataBits;
        private string[] mBaudRateArry = {
            "1200","2400","4800","9600","14400",
            "19200","28800","38400","57600","115200",
            "230400","460800"}; 

        public const int BAUDRATEARRY_1200_POS = 0;
        public const int BAUDRATEARRY_2400_POS = 1;
        public const int BAUDRATEARRY_4800_POS = 2;
        public const int BAUDRATEARRY_9600_POS = 3;
        public const int BAUDRATEARRY_14400_POS = 4;
        public const int BAUDRATEARRY_19200_POS = 5;
        public const int BAUDRATEARRY_28800_POS = 6;
        public const int BAUDRATEARRY_38400_POS = 7;
        public const int BAUDRATEARRY_57600_POS = 8;
        public const int BAUDRATEARRY_115200_POS = 9;
        public const int BAUDRATEARRY_230400_POS = 10;
        public const int BAUDRATEARRY_460800_POS = 11;

        #endregion
        /*------------------------------------Attribute-----------------------------------------*/
        #region
        /// <summary>
        /// 用于查询可用的串口号
        /// </summary>
        /// <returns>返回可用的串口</returns>
        public string[] GetOnlineComName()
        {
            mBaudRateArry = SerialPort.GetPortNames();
            return mBaudRateArry;
        }

        // 当前选择的波特率
        public int BaudRate
        {
            get { return mBaudRate; }
            set { mBaudRate = value; }
        }

        // 当前选择的端口
        public string PortName
        {
            get { return mPortName; }
            set { mPortName = value; }
        }

        // 当前设置的数据宽度
        public int DataBits
        {
            get { return mDataBits; }
            set { mDataBits = value; }
        }

        // 当前可用的波特率
        public string[] BaudRateArry
        {
            get { return mBaudRateArry; }
            set { mBaudRateArry = value; }
        }

        // 提供给UI层的数据模型
        public CustomDataModel MCustomDataModel
        {
            get { return mCustomDataModel; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mDataMod"></param>
        public User_Model()
        {
            // 初始化UI所使用的数据模型
            mCustomDataModel = new CustomDataModel(this);

            // 初始化帧缓冲
            mGpsMsgBuff = new Gps_NmeaFrame[NMEA_FRAME_TYPE_SIZE]
            {
                new Gps_NmeaFrame(),
                new Gps_NmeaFrame(),
                new Gps_NmeaFrame(),
                new Gps_NmeaFrame(),
                new Gps_NmeaFrame(),
                new Gps_NmeaFrame()
            };
            // 初始化解析状态机
            NmeaParseMoniterInit();

        }

        /*----------------------------------------------SerialPortDef----------------------------------------------------*/

        /// <summary>
        /// 自定义串口初始化
        /// </summary>
        public void CustomSerialPort_Init()
        {
            mDataBits = 8;
            mBaudRate = 9600;
        }

        /*----------------------------------------------NmeaParseDef-----------------------------------------------------*/
        /// <summary>
        /// 解析器接口，该接口用于解析NMEA帧
        /// </summary>
        /// <param name="datas">待解析的数据</param>
        /// <param name="length">待解析的数据长度</param>
        public void Parse_Bytes(byte[] datas, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Parse_Byte(datas[i]);
            }
        }

        /// <summary>
        /// 初始化解析状态机，初始化每个动作所执行的操作
        /// </summary>
        private void NmeaParseMoniterInit()
        {
            // 初始化状态机
            ParseMoniter = new Dictionary<NmeaFrame_ParseState, Action<byte>>();
            Current_State = NmeaFrame_ParseState.Null_State;

            //帧头状态
            ParseMoniter[NmeaFrame_ParseState.Head_State] = (byte data) =>
            {
                // 计算crc
                Current_Crc ^= data;

                // 如果获得分隔符
                if (data == Gps_NmeaFrame.NMEA_FRAME_SEPARATE_BYTE)
                {
                    string talkerid;
                    string contentid;

                    // 如果分析成功
                    if (Gps_NmeaFrame.GetHeadInfo(mStrBuffer, out talkerid, out contentid) == true)
                    {
                        // 清除缓冲
                        mStrBuffer = "";
                        // 清除计数器
                        ParseMoniterCnt = 0;

                        // 根据帧类型保存帧类型状态
                        if (contentid.Equals(NMEA_FRAME_CONTENTID_GGA))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_GGA;
                        }
                        else if (contentid.Equals(NMEA_FRAME_CONTENTID_RMC))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_RMC;
                        }
                        else if (contentid.Equals(NMEA_FRAME_CONTENTID_GSV))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_GSV;
                        }
                        else if (contentid.Equals(NMEA_FRAME_CONTENTID_GAS))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_GAS;
                        }
                        else if (contentid.Equals(NMEA_FRAME_CONTENTID_GSA))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_GSA;
                        }
                        else if (contentid.Equals(NMEA_FRAME_CONTENTID_HPD))
                        {
                            mTmpFrameType = NMEA_FRAME_TYPE_HPD;
                        }
                        else
                        {
                            Current_State = NmeaFrame_ParseState.Null_State;
                            return;
                        }
                        // 清除缓冲数组
                        mGpsMsgBuff[mTmpFrameType].Clear();

                        // 添加帧头到头部
                        mGpsMsgBuff[mTmpFrameType].AddHead(talkerid, contentid);

                        // 切换到下一个状态
                        Current_State = NmeaFrame_ParseState.Data_State;

                    } // 分析出错
                    else
                    {   // 复位状态机
                        Current_State = NmeaFrame_ParseState.Null_State;
                    }
                } // 如果没有获得分隔符
                else
                {
                    // 添加数据到临时缓冲
                    mStrBuffer += (char)data;
                }

                // 如果索引值超出限制，直接恢复状态机 
                if (ParseMoniterCnt++ > (Gps_NmeaFrame.NMEA_FRAME_HEAD_LEN - 1))
                {
                    Current_State = NmeaFrame_ParseState.Null_State;
                }
            };

            //帧数据状态
            ParseMoniter[NmeaFrame_ParseState.Data_State] = (byte data) =>
            {
                // 是否获取到了校验位指示符
                if (data == Gps_NmeaFrame.NMEA_FRAME_CRC_IDENTIFIER_BYTE)
                {
                    // 添加到缓冲区
                    mGpsMsgBuff[mTmpFrameType].AddData(mStrBuffer);
                    // 清除缓冲
                    mStrBuffer = "";
                    // 清除计数器
                    ParseMoniterCnt = 0;
                    // 跳转到CrcState
                    Current_State = NmeaFrame_ParseState.Crc_State;
                }
                else
                {
                    // 计算校验
                    Current_Crc ^= data;

                    // 是否为分隔符
                    if (data == Gps_NmeaFrame.NMEA_FRAME_SEPARATE_BYTE)
                    {
                        // 添加到缓冲区
                        mGpsMsgBuff[mTmpFrameType].AddData(mStrBuffer);
                        // 清楚缓冲区
                        mStrBuffer = "";
                    }
                    else
                    {
                        // 添加字符到缓冲
                        mStrBuffer += (char)data;
                    }
                }
                // 如果超出限制，直接复位状态机
                if (ParseMoniterCnt++ > Gps_NmeaFrame.NMEA_FRAME_DATA_LEN_MAX)
                {
                    Current_State = NmeaFrame_ParseState.Null_State;
                }
            };

            // 帧校验状态
            ParseMoniter[NmeaFrame_ParseState.Crc_State] = (byte data) =>
            {
                // 如果接收到了第一个结束字符
                if (data == Gps_NmeaFrame.NMEA_FRAME_END_IDENTIFIER1_BYTE)
                {
                    byte crc;
                    // 获取crc
                    crc = Convert.ToByte(mStrBuffer, 16);

                    if (crc == Current_Crc)
                    {
                        mGpsMsgBuff[mTmpFrameType].AddCrc(mStrBuffer);
                        Current_State = NmeaFrame_ParseState.End_State;
                        return;
                    }
                }
                else
                {
                    mStrBuffer += (char)data;
                }

                // 如果超出限制，复位状态机 
                if (ParseMoniterCnt++ > Gps_NmeaFrame.NMEA_FRAME_DATA_CRC_LEN)
                {
                    Current_State = NmeaFrame_ParseState.Null_State;
                }
            };

            // 帧结束状态
            ParseMoniter[NmeaFrame_ParseState.End_State] = (byte data) =>
            {
                // 如果接收到第二个字符
                if (data == Gps_NmeaFrame.NMEA_FRAME_END_IDENTIFIER2_BYTE)
                {
                    // 处理当前帧
                    SelfDeal(mGpsMsgBuff[mTmpFrameType], mTmpFrameType);
                    // 此处需要清除帧缓冲
                    mGpsMsgBuff[mTmpFrameType].Clear();
                }
                Current_State = NmeaFrame_ParseState.Null_State;
            };
        }

        /// <summary>
        /// 解析单字节入口函数
        /// </summary>
        /// <param name="data">待解析的字节</param>
        private void Parse_Byte(byte data)
        {
            // 如果接收到起始字节
            if (Gps_NmeaFrame.NMEA_FRAME_HEAD_BYTE == data)
            {
                // 清除帧类型指示
                mTmpFrameType = NMEA_FRAME_TYPE_NULL;
                // 清空临时缓冲字符串
                mStrBuffer = "";
                // 复位计数器
                ParseMoniterCnt = 0;
                // 清除CRC校验
                Current_Crc = 0;
                // 添加字符到缓冲
                mStrBuffer += (char)data;
                // 切换到下一个状态
                Current_State = NmeaFrame_ParseState.Head_State;
            }
            else
            {   // 如果当前状态不为空状态
                if (Current_State != NmeaFrame_ParseState.Null_State)
                {
                    ParseMoniter[Current_State](data);
                }
            }
        }

        /// <summary>
        /// 对帧缓冲进行处理
        /// </summary>
        /// <param name="frame">帧缓冲</param>
        /// <param name="index">帧的索引号</param>
        private void SelfDeal(Gps_NmeaFrame frame, int type)
        {
            string content_frame = frame.GetFrame();

            // 添加信息到队列中
            mCustomDataModel.Rev_Msg_Queue.Enqueue(content_frame);

            if (frame.TalkerId == "GP")
            {
                mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GP_AVAILABLE] = true;
            }
            else if (frame.TalkerId == "BD")
            {
                mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GB_AVAILABLE] = true;
            }
            else if (frame.TalkerId == "GN")
            {
                mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GP_AVAILABLE] = true;
                mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GB_AVAILABLE] = true;
            }

            // 将结果组
            try
            {
                // 根据结果进行赋值
                switch (type)
                {
                    case NMEA_FRAME_TYPE_GGA:
                        // GGA帧处理
                        #region
                        mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GGA_AVAILABLE] = true;

                        // utc
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_UTC_TIME].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_TIME];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_UTC_TIME].IsUpdate = true;

                        // 纬度
                        string str_lat = frame.DataSrc[NMEA_FRAME_GGA_POS_LAT];

                        if (str_lat.Length > 5)
                        {
                            try
                            {
                                double val = Convert.ToDouble(str_lat.Substring(0, 2)) + Convert.ToDouble(str_lat.Substring(2, (str_lat.Length - 2))) / 60.0;
                                LattitudeDataArr.AddData(val);
                            }
                            catch
                            { }
                            
                            str_lat = str_lat.Insert(2, "°");
                            str_lat += "'";
                        }
                        // 存储纬度
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_LATITUDE].Info = str_lat;
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_LATITUDE].IsUpdate = true;
                        // 存储纬度std
                        mCustomDataModel.DataBaseList[CustomDataModel.STR_LATITUDE_STD].Info = LattitudeDataArr.GetStdVal().ToString("#.##E+00") + "°";
                        mCustomDataModel.DataBaseList[CustomDataModel.STR_LATITUDE_STD].IsUpdate = true;
                        // 经度
                        string str_long = frame.DataSrc[NMEA_FRAME_GGA_POS_LONG];
                        if(str_long.Length > 5)
                        {
                            try
                            {
                                double val = Convert.ToDouble(str_long.Substring(0, 3)) + Convert.ToDouble(str_long.Substring(3, (str_long.Length - 3))) / 60.0;
                                LongtitudeDataArr.AddData(val);
                            }
                            catch
                            { }


                            str_long = str_long.Insert(3, "°");
                            str_long += "'";
                        }
                        // 存储经度
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_LONGTITUDE].Info = str_long;
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_LONGTITUDE].IsUpdate = true;
                        // 存储经度std
                        mCustomDataModel.DataBaseList[CustomDataModel.STR_LONGTITUDE_STD].Info = LongtitudeDataArr.GetStdVal().ToString("#.##E+00") + "°";
                        mCustomDataModel.DataBaseList[CustomDataModel.STR_LONGTITUDE_STD].IsUpdate = true;

                        // 高度
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_ALTITUDE].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_ALT];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_ALTITUDE].IsUpdate = true;

                        // 椭球高度
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_ELLIPSOIDAL_HEIGHT].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_SEP];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_ELLIPSOIDAL_HEIGHT].IsUpdate = true;

                        // 卫星数量
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_SATELLITE_NUM].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_NUMSV];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_SATELLITE_NUM].IsUpdate = true;

                        // 定位精度
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_POSITION_ACC].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_HDOP];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_POSITION_ACC].IsUpdate = true;

                        // 定位状态
                        mCustomDataModel.DataBaseList[CustomDataModel.POS_STATE].Info = frame.DataSrc[NMEA_FRAME_GGA_POS_QUALITY];
                        mCustomDataModel.DataBaseList[CustomDataModel.POS_STATE].IsUpdate = true;

                        break;
                    #endregion
                    case NMEA_FRAME_TYPE_RMC:
                        mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_RMC_AVAILABLE] = true;
                        break;
                    case NMEA_FRAME_TYPE_GSV:
                        mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GSV_AVAILABLE] = true;
                        // GSV帧处理函数
                        #region
                        // 预估当前帧中的卫星个数
                        int NumSatelite = frame.DataSrc.Count - NMEA_FRAME_GSV_POS_SATELLITE_SV;

                        // 获取卫星数量
                        int numsv = Convert.ToInt32(frame.DataSrc[NMEA_FRAME_GSV_POS_NUMSV]);

                        // 如果卫星数为0,则直接返回
                        if (numsv == 0)
                        {
                            break;
                        }

                        // 判断帧是否正确，不正确直接跳出
                        if (NumSatelite % NMEA_FRAME_GSV_SATALITES_INFO_LEN != 0)
                        {   
                            // 该处对NMEA4.1极其以上做出兼容，末尾端有一个字符
                            if ((NumSatelite - NMEA_FRAME_GSV_SIGNAL_ID_LEN) % NMEA_FRAME_GSV_SATALITES_INFO_LEN != 0)
                            {
                                break;
                            }
                        }

                        // 计算卫星个数
                        NumSatelite /= NMEA_FRAME_GSV_SATALITES_INFO_LEN;

                        // 帧头字符串
                        string headlabel = "";

                        // 如果为北斗卫星
                        if (frame.TalkerId.Equals(NMEA_FRAME_TALKERID_BD))
                        {
                            // 生成标签
                            headlabel = "B";
                        }// 否则为GPS卫星
                        else
                        {
                            headlabel = "G";
                        }

                        for (int i = 0; i < NumSatelite; i++)
                        {
                            // 清空字符串
                            string label = "";

                            // 生成label
                            label = headlabel + frame.DataSrc[NMEA_FRAME_GSV_POS_SATELLITE_SV + i * NMEA_FRAME_GSV_SATALITES_INFO_LEN];

                            // 生成值
                            double val = 0.0;
                            double elv = 0.0;
                            double azi = 0.0;

                            // 将内容转换出来
                            try
                            {
                                val = Convert.ToDouble(frame.DataSrc[NMEA_FRAME_GSV_POS_SATELLITE_CNO + i * NMEA_FRAME_GSV_SATALITES_INFO_LEN]);
                            }
                            catch
                            {
                                val = 0.0;
                            }
                            
                            try
                            {
                                elv = Convert.ToDouble(frame.DataSrc[NMEA_FRAME_GSV_POS_SATELLITE_ELV + i * NMEA_FRAME_GSV_SATALITES_INFO_LEN]);
                            }
                            catch
                            {
                                elv = CustomDataModel.GPS_SAT_ANG_INVALID;
                            }

                            try
                            {
                                azi = Convert.ToDouble(frame.DataSrc[NMEA_FRAME_GSV_POS_SATELLITE_AZ + i * NMEA_FRAME_GSV_SATALITES_INFO_LEN]);
                            }
                            catch
                            {
                                azi = CustomDataModel.GPS_SAT_ANG_INVALID;
                            }

                            // 生成一个对象
                            SateLiteInfoItem item = new SateLiteInfoItem(label, val, NMEA_FRAME_GSV_SIGNAL_STRENGTH_MAX, azi, elv);

                            // 添加数据到链表中
                            mCustomDataModel.SateLiteStrengthData_Add_Intelligence(item);
                        }
                        break;
                    #endregion
                    case NMEA_FRAME_TYPE_GAS:
                        mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_GAS_AVAILABLE] = true;

                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_GAS_COORDINATION_X].Info = frame.DataSrc[NMEA_FRAME_GAS_X_COODINATION];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_GAS_COORDINAIION_Y].Info = frame.DataSrc[NMEA_FRAME_GAS_Y_COODINATION];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_PDOP].Info = frame.DataSrc[NMEA_FRAME_GAS_PDOP];
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_GAS_COORDINATION_X].IsUpdate = true;
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_GAS_COORDINAIION_Y].IsUpdate = true;
                        mCustomDataModel.DataBaseList[CustomDataModel.LABEL_PDOP].IsUpdate = true;
                        break;
                    case NMEA_FRAME_TYPE_GSA:
                        // PDOP
                        //mCustomDataModel.DataBaseList[CustomDataModel.LABEL_PDOP].Info = frame.DataSrc[NMEA_FRAME_GSA_POS_PDOP];
                        //mCustomDataModel.DataBaseList[CustomDataModel.LABEL_PDOP].IsUpdate = true;
                        break;
                    case NMEA_FRAME_TYPE_HPD:
                        mCustomDataModel.Gps_Receiver_State[CustomDataModel.GPS_REV_STATE_HPD_AVAILABLE] = true;



                        break;

                    default: break;
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.ToString());
                return;
            }
        }
    }
}
