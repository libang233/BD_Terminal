
using BD_Terminal.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD_Terminal.Model;
using System.Threading;

namespace BD_Terminal.Control
{

    public class User_Control
    {
        /*--------------------------------------Const-------------------------------------------*/
        private const int MSERIALPORT_REVBYTES_THRESHOLD = 10;

        // 模型层
        private User_Model mModel;
        // 写文件
        private StreamWriter mFileWriter;
        // 串口类
        private SerialPort mSerialPort;

        // 控制器监控线程
        // 用于线程停止和启动，初始化禁止运行
        private AutoResetEvent autoRstEvt = new AutoResetEvent(false);
        private volatile bool IsAllowRun = false;
        private Thread ControlMoniterThread;

        public enum UserControlEventType
        {
            SerialPortOpened,
            SerialPortClosed
        };

        // 控制器事件
        public delegate void User_Control_EventHandler(UserControlEventType type);
        public event User_Control_EventHandler User_Control_Event;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">MVC模型</param>
        /// <param name="customDataModel"></param>
        public User_Control(User_Model model)
        {
            // 初始化串口
            mSerialPort = new SerialPort();
        
            // 设置模型
            mModel = model;

            // 设置串口接收接口
            mSerialPort.DataReceived += MSerialPort_DataReceived;
            // 设置接收触发接口
            mSerialPort.ReceivedBytesThreshold = MSERIALPORT_REVBYTES_THRESHOLD;

            ControlMoniterThread = new Thread(MoniterThreadHandler);
            ControlMoniterThread.IsBackground = true;
            ControlMoniterThread.Start();
        }
        


        /// <summary>
        /// 析构函数，该函数中关闭串口，文件流，并释放资源
        /// </summary>
        ~User_Control()
        {
            // 关闭串口
            SerialClose();
            //mSerialPort.Dispose();
            // 关闭文件
            CloseFile();
        }

        /// <summary>
        /// 串口数据接收接口
        /// </summary>
        /// <param name="sender">发送该消息的类</param>
        /// <param name="e">发送的参数</param>
        private void MSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // 获取待读取的数据长度
                int len = mSerialPort.BytesToRead;

                // 定义一个数组
                byte[] ReadBuff = new byte[len];

                //将数据读取出来
                mSerialPort.Read(ReadBuff, 0, len);

                // 传入给模型进行解析
                mModel.Parse_Bytes(ReadBuff, len);
            }
            catch { }           
        }

        /// <summary>
        /// 用于打开串口
        /// </summary>
        /// <param name="portname">串口名称</param>
        /// <param name="baudRate">串口波特率</param>
        public void OpenSerialPort(string portname, int baudRate)
        {
            try
            {
                // 设置串口波特率
                mSerialPort.BaudRate = baudRate;
                // 设置端口名称
                mSerialPort.PortName = portname;
                // 停止位
                mSerialPort.StopBits = StopBits.One;
                // 数据位宽8
                mSerialPort.DataBits = 8;
                // 无校验
                mSerialPort.Parity = Parity.None;
                // 打开串口
                mSerialPort.Open();

                // 允许运行
                IsAllowRun = true;
                autoRstEvt.Set();

                // 发出事件，串口打开
                User_Control_Event(UserControlEventType.SerialPortOpened);
            }
            catch (UnauthorizedAccessException)
            {
                System.Windows.MessageBox.Show("串口已被打开，请选择其他串口!", "提示");
            }
            catch { }
        }

        /// <summary>
        /// 返回串口状态
        /// </summary>
        /// <returns></returns>
        public bool SerialIsOpen()
        {
           return mSerialPort.IsOpen; 
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void SerialClose()
        {
            try
            {
                // 关闭串口
                mSerialPort.Close();
            }
            catch { }

            // 重置资源
            mModel.MCustomDataModel.Reset();
            // 停止运行
            IsAllowRun = false;
            // 发送事件，串口关闭
            User_Control_Event(UserControlEventType.SerialPortClosed);
        }

        /// <summary>
        /// 从串口发送数据
        /// </summary>
        /// <param name="str"></param>
        public void Serialwrite(string str)
        {
            if(!mSerialPort.IsOpen)
            {
                return;
            }

            try
            {
                mSerialPort.Write(str);
            }
            catch (Exception)
            {
                SerialClose();
            }
        }

        /// <summary>
        /// 从串口发送数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="size"></param>
        public void SerialWrite(char[] buf, int size)
        {
            try
            {
                mSerialPort.Write(buf, 0, size);
            }
            catch (Exception)
            {
                // 关闭串口
                SerialClose();
                // 释放资源
                // mSerialPort.Dispose();
            }
        }

        /// <summary>
        /// 使能写文件
        /// </summary>
        /// <param name="filepath"></param>
        public void OpenFile(string filepath)
        {
            // 打开文件
            mFileWriter = new StreamWriter(filepath, true);
        }

        /// <summary>
        /// 关闭文件流
        /// </summary>
        public void CloseFile()
        {
            // 关闭文件
            if (mFileWriter != null)
            {
                mFileWriter.Close();

                mFileWriter = null;
            }
        }

        /// <summary>
        /// 向文件写入一段字符串
        /// </summary>
        /// <param name="str"></param>
        public void WriteToFile(string str)
        {
            // 如果文件打开 则写文件
            try
            {
                if (mFileWriter != null)
                {
                    mFileWriter.WriteLine(str);
                }
            }
            catch { }  
        }

        /// <summary>
        /// 系统监控线程
        /// </summary>
        private void MoniterThreadHandler()
        {
            while(true)
            {
                // 根据状态执行或挂起线程   
                if (!IsAllowRun)
                {
                    autoRstEvt.WaitOne();  // 等待串口打开
                }

                // 如果串口关闭了
                if (!mSerialPort.IsOpen)
                {
                    SerialClose();
                }

                Thread.Sleep(200);
            }
        }
    }
}
