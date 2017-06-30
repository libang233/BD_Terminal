using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD_Terminal.Model
{
    /// <summary>
    /// GPS的NMEA帧类，可用于存放Nmea信息。
    /// </summary>
    class Gps_NmeaFrame
    {
        /*--------------------------------------Const-------------------------------------------*/
        #region
        // NMEA帧中默认信息定义
        public const int NMEA_FRAME_HEAD_LEN = 6;
        public const int NMEA_FRAME_TALKERID_POS = 1;
        public const int NMEA_FRAME_CONTENT_POS = 3;
        public const int NMEA_FRAME_DATA_LEN_MAX = 120;
        public const int NMEA_FRAME_DATA_CRC_LEN = 2;
        public const byte NMEA_FRAME_HEAD_BYTE = (byte)'$';
        public const byte NMEA_FRAME_SEPARATE_BYTE = (byte)',';
        public const byte NMEA_FRAME_CRC_IDENTIFIER_BYTE = (byte)'*';
        public const byte NMEA_FRAME_END_IDENTIFIER1_BYTE = (byte)'\r';
        public const byte NMEA_FRAME_END_IDENTIFIER2_BYTE = (byte)'\n';
        #endregion
        /*-----------------------------------PrivateData----------------------------------------*/
        #region
        // Talker Id
        private string mTalkerId;
        // Content Id
        private string mContentId;
        // 帧里面的数据
        private List<string> mDataSrc;
        // 校验
        private string mCrc;
        #endregion
        /*------------------------------------Attribute-----------------------------------------*/
        #region
        public List<string> DataSrc
        {
            get { return mDataSrc; }
        }

        public string TalkerId
        {
            get { return mTalkerId; }
        }

        public string ContentId
        {
            get { return mContentId; }
        }

        public string Crc
        {
            get { return mCrc; }
        }
        #endregion
        /*------------------------------------PublicFuc-----------------------------------------*/
        #region
        public Gps_NmeaFrame()
        {
            // 初始化内容列表
            mDataSrc = new List<string>();
            mTalkerId = "";
            mContentId = "";
            mCrc = "";
        }
        /// <summary>
        /// 添加帧头
        /// </summary>
        /// <param name="head">帧头字符串</param>
        public void AddHead(string talkerid, string contentid)
        {
            mTalkerId = talkerid;
            mContentId = contentid;
        }

        /// <summary>
        /// 获取NMEA帧头的信息
        /// </summary>
        /// <param name="head">标准的NMEA帧头，如$GNGGA</param>
        /// <param name="talkerid">取出帧头Talkerid，如GN</param>
        /// <param name="contentid">取出帧头Contentid，如GGA</param>
        /// <returns></returns>
        public static bool GetHeadInfo(string head, out string talkerid, out string contentid)
        {
            // 清空数组
            talkerid = "";
            contentid = "";

            // 如果长度不符合
            if (head.Length != Gps_NmeaFrame.NMEA_FRAME_HEAD_LEN) return false;

            // 转换为byte数组
            byte[] headbytes = System.Text.Encoding.ASCII.GetBytes(head);

            // 是否合法
            if (headbytes[0] == Gps_NmeaFrame.NMEA_FRAME_HEAD_BYTE)
            {
                talkerid += (char)headbytes[1];
                talkerid += (char)headbytes[2];

                contentid += (char)headbytes[3];
                contentid += (char)headbytes[4];
                contentid += (char)headbytes[5];

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">数据字符串</param>
        public void AddData(string data)
        {
            mDataSrc.Add(data);
        }

        /// <summary>
        /// 添加Crc
        /// </summary>
        /// <param name="crc">crc值</param>
        public void AddCrc(string crc)
        {
            mCrc = crc;
        }

        /// <summary>
        /// 清除帧内容
        /// </summary>
        public void Clear()
        {
            mDataSrc.Clear();

            mTalkerId = "";
            mContentId = "";
        }

        /// <summary>
        /// 获取完整帧的组成
        /// </summary>
        /// <returns></returns>
        public string GetFrame()
        {
            string strframe = "";

            strframe += (char)Gps_NmeaFrame.NMEA_FRAME_HEAD_BYTE;
            strframe += mTalkerId;
            strframe += mContentId;

            foreach (string item in mDataSrc)
            {
                strframe += (char)Gps_NmeaFrame.NMEA_FRAME_SEPARATE_BYTE;
                strframe += item;
            }
            strframe += (char)Gps_NmeaFrame.NMEA_FRAME_CRC_IDENTIFIER_BYTE;
            strframe += mCrc;
            strframe += (char)Gps_NmeaFrame.NMEA_FRAME_END_IDENTIFIER1_BYTE;
            strframe += (char)Gps_NmeaFrame.NMEA_FRAME_END_IDENTIFIER2_BYTE;

            return strframe;
        }
        #endregion
    }
}
