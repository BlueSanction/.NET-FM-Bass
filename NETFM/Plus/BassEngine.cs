using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;

namespace NETFM
{
    /// <summary>
    /// Bass播放器/Bass player
    /// </summary>
    public class BassEngine : ISpectrumPlayer, ISoundPlayer, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// 待执行的命令/Command to execute
        /// </summary>
        private enum PendingOperation
        {
            /// <summary>
            /// 无
            /// </summary>
            None,
            /// <summary>
            /// 播放
            /// </summary>
            Play,
            /// <summary>
            /// 暂停
            /// </summary>
            Pause
        }
        /// <summary>
        /// BassEngine的唯一实例/ Unique instance of BassEngine
        /// </summary>
        private static BassEngine instance;
        /// <summary>
        /// 用于更新播放进度的计时器/Update playback timer
        /// </summary>
        //private readonly DispatcherTimer positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
        private readonly int maxFFT = -2147483644;
        /// <summary>
        /// 当播放结束时调用 Call at the end of the play
        /// </summary>
        private readonly SYNCPROC endTrackSyncProc;
        /// <summary>
        /// 播放码率/Play Rate
        /// </summary>
        private int sampleFrequency = 44100;
        /// <summary>
        /// 当前流的句柄/Stream Handle
        /// </summary>
        private int activeStreamHandle;
        /// <summary>
        /// 可以使用播放命令
        /// </summary>
        private bool canPlay;
        /// <summary>
        /// 可以使用暂停命令
        /// </summary>
        private bool canPause;
        /// <summary>
        /// 是否正在播放
        /// </summary>
        private bool isPlaying;
        /// <summary>
        /// 可以使用停止命令
        /// </summary>
        private bool canStop;
        /// <summary>
        /// 音频长度/Audio length
        /// </summary>
        private TimeSpan channelLength = TimeSpan.Zero;
        /// <summary>
        /// 当前播放进度
        /// </summary>
        private TimeSpan currentChannelPosition = TimeSpan.Zero;
        private bool inChannelSet;
        private bool inChannelTimerUpdate;
        /// <summary>
        /// 用于异步打开网络音频文件的线程/Thread used to open network audio files asynchronously
        /// </summary>
        private Thread onlineFileWorker;
        /// <summary>
        /// 待执行的命令，当打开网络上的音频时非常有用/Command to execute, It is very useful when opening the audio on the network！
        /// </summary>
        private BassEngine.PendingOperation pendingOperation = BassEngine.PendingOperation.None;
        /// <summary>
        /// 音量
        /// </summary>
        private double volume;
        /// <summary>
        /// 是否静音
        /// </summary>
        private bool isMuted;
        /// <summary>
        /// 保存正在打开的文件的地址，当短时间内多次打开网络文件时，这个字段保存最后一次打开的文件，可以使其他打开文件的操作失效
        /// </summary>
        private string openningFile = null;

        public string OpenningFile
        {
            get { return openningFile; }
            set { openningFile = value; }
        }
        private bool _disposed;
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 当播放完毕时发生/Happen to End play
        /// </summary>
        public event EventHandler TrackEnded;


        public delegate void TEventHandler(object sender, string  e);
        /// <summary>
        /// 当打开音频文件失败时发生/Happen to OpenError
        /// </summary>
        public event TEventHandler OpenFailed;
        /// <summary>
        /// 当打开音频文件成功时发生/Happen to OpenSucceeded
        /// </summary>
        public event EventHandler OpenSucceeded;
        /// <summary>
        /// 获取BassEngine的唯一实例
        /// </summary>
        public static BassEngine Instance
        {
            get
            {
                if (BassEngine.instance == null)
                {
                    BassEngine.instance = new BassEngine(null);
                }
                return BassEngine.instance;
            }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public TimeSpan ChannelLength
        {
            get
            {
                return this.channelLength;
            }
            protected set
            {
                TimeSpan t = this.channelLength;
                this.channelLength = value;
                if (t != this.channelLength)
                {
                    this.NotifyPropertyChanged("ChannelLength");
                }
            }
        }
        /// <summary>
        /// 播放器指针位置
        /// </summary>
        public TimeSpan ChannelPosition
        {
            get
            {
                positionTimer_Tick();
                return this.currentChannelPosition;
            }
            set
            {
                if (!this.inChannelSet)
                {
                    this.inChannelSet = true;
                    TimeSpan t = this.currentChannelPosition;
                    TimeSpan t2 = value;
                    if (t2 > this.ChannelLength)
                    {
                        t2 = this.ChannelLength;
                    }
                    if (t2 < TimeSpan.Zero)
                    {
                        t2 = TimeSpan.Zero;
                    }
                    if (!this.inChannelTimerUpdate)
                    {
                        Bass.BASS_ChannelSetPosition(this.ActiveStreamHandle, Bass.BASS_ChannelSeconds2Bytes(this.ActiveStreamHandle, t2.TotalSeconds));
                    }
                    this.currentChannelPosition = t2;
                    if (t != this.currentChannelPosition)
                    {
                        this.NotifyPropertyChanged("ChannelPosition");
                    }
                    this.inChannelSet = false;
                }
            }
        }

        public void PlayTo(double k) 
        {
            Bass.BASS_ChannelSetPosition(activeStreamHandle, Bass.BASS_ChannelSeconds2Bytes(activeStreamHandle, k));
        }
        /// <summary>
        /// 当前流的句柄
        /// </summary>
        public int ActiveStreamHandle
        {
            get
            {
                return this.activeStreamHandle;
            }
            protected set
            {
                int num = this.activeStreamHandle;
                this.activeStreamHandle = value;
                if (num != this.activeStreamHandle)
                {
                    if (this.activeStreamHandle != 0)
                    {
                        this.SetVolume();
                    }
                    this.NotifyPropertyChanged("ActiveStreamHandle");
                }
            }
        }
        /// <summary>
        /// 可以使用播放命令
        /// </summary>
        public bool CanPlay
        {
            get
            {
                return this.canPlay;
            }
            protected set
            {
                bool flag = this.canPlay;
                this.canPlay = value;
                if (flag != this.canPlay)
                {
                    this.NotifyPropertyChanged("CanPlay");
                }
            }
        }
        /// <summary>
        /// 可以使用暂停命令
        /// </summary>
        public bool CanPause
        {
            get
            {
                return this.canPause;
            }
            protected set
            {
                bool flag = this.canPause;
                this.canPause = value;
                if (flag != this.canPause)
                {
                    this.NotifyPropertyChanged("CanPause");
                }
            }
        }
        /// <summary>
        /// 可以使用停止命令
        /// </summary>
        public bool CanStop
        {
            get
            {
                return this.canStop;
            }
            protected set
            {
                bool flag = this.canStop;
                this.canStop = value;
                if (flag != this.canStop)
                {
                    this.NotifyPropertyChanged("CanStop");
                }
            }
        }
        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return this.isPlaying;
            }
            protected set
            {
                bool flag = this.isPlaying;
                this.isPlaying = value;
                if (flag != this.isPlaying)
                {
                    this.NotifyPropertyChanged("IsPlaying");
                }
                //this.positionTimer.IsEnabled = value;
            }
        }
        /// <summary>
        /// 设置或获取音量值
        /// </summary>
        public double Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = value / 1000;
                this.SetVolume();
                this.NotifyPropertyChanged("Volume");
            }
        }

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsMuted
        {
            get
            {
                return this.isMuted;
            }
            set
            {
                if (this.isMuted != value)
                {
                    this.isMuted = value;
                    this.SetVolume();
                    this.NotifyPropertyChanged("IsMuted");
                }
            }
        }
        /// <summary>
        /// 设备（空代表默认设备）
        /// </summary>
        public DeviceInfo? Device
        {
            get;
            private set;
        }
        //static BassEngine()
        //{
        //    //BassNet.Registration("545153113@qq.com", "2X213310160022");
        //    //string path;
        //    //if (Utils.Is64Bit)
        //    //{
        //    //    path = Path.Combine(Path.GetDirectoryName(typeof(BassEngine).Assembly.GetModules()[0].FullyQualifiedName), "x64");
        //    //}
        //    //else
        //    //{
        //    //    path = Path.Combine(Path.GetDirectoryName(typeof(BassEngine).Assembly.GetModules()[0].FullyQualifiedName), "x86");
        //    //}
        //    //Bass.LoadMe(path);
        //    //Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, true);

        //    //string[] loadedPlugIns = Directory.GetFiles(path, "*.dll");

        //    //foreach (string plugin in loadedPlugIns)
        //    //{
        //    //    Bass.BASS_PluginLoad(plugin);
        //    //}

        //    //Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT, 15000);     
        //}

        /// <summary>
        /// 初始化播放器设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        private BassEngine(DeviceInfo? deviceInfo = null)
        {
            this.Initialize(deviceInfo);
            this.endTrackSyncProc = new SYNCPROC(this.EndTrack);
        }

        /// <summary>
        /// 释放播放器设备
        /// </summary>
        ~BassEngine()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 结果播放器设备
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放播放器设备
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this.onlineFileWorker != null)
                    {
                        this.onlineFileWorker.Abort();
                        this.onlineFileWorker = null;
                    }
                }
                Bass.BASS_Free();
                Bass.FreeMe();
                this._disposed = true;
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        /// <summary>
        /// 显式初始化
        /// </summary>
        public static void ExplicitInitialize(DeviceInfo? deviceInfo = null)
        {
            if (BassEngine.instance == null)
            {
                BassEngine.instance = new BassEngine(deviceInfo);
            }
        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public static DeviceInfo[] GetDeviceInfos()
        {
            List<DeviceInfo> list = new List<DeviceInfo>();
            List<BASS_DEVICEINFO> list2 = Bass.BASS_GetDeviceInfos().ToList<BASS_DEVICEINFO>();
            foreach (BASS_DEVICEINFO current in list2)
            {
                if (current.IsEnabled && !string.Equals(current.name, "No sound", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(current.name, "Default", StringComparison.CurrentCultureIgnoreCase))
                {
                    list.Add(new DeviceInfo
                    {
                        ID = current.id,
                        Name = current.name,
                        Driver = current.driver
                    });
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 更换设备
        /// </summary>
        public void ChangeDevice(DeviceInfo? device)
        {
            int num = BassEngine.FindDevice(device, false);
            int num2 = Bass.BASS_GetDevice();
            if (num2 != num)
            {
                if (!Bass.BASS_GetDeviceInfo(num).IsInitialized)
                {
                    IntPtr win = IntPtr.Zero;
                    //if (Application.Current.MainWindow != null)
                    //{
                    //    win = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
                    //}
                    if (!Bass.BASS_Init(num, this.sampleFrequency, BASSInit.BASS_DEVICE_DEFAULT, win))
                    {
                        throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                    }
                }
                if (this.activeStreamHandle != 0)
                {
                    if (!Bass.BASS_ChannelSetDevice(this.activeStreamHandle, num))
                    {
                        throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                    }
                }
                if (!Bass.BASS_SetDevice(num2))
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
                if (!Bass.BASS_Free())
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
                if (!Bass.BASS_SetDevice(num))
                {
                    throw new Exception(Bass.BASS_ErrorGetCode().ToString());
                }
            }
            this.Device = device;
        }

        /// <summary>
        /// 设置声道,低于16为左声道，高于16为右声道，16为立体声/Set channel,Less than 16 for Left Channel,Greater than 16 for Right Channel,16 for Stereo
        /// </summary>
        /// <param name="value"></param>
        public void ChannelSounrd(int value)
        {
            Bass.BASS_ChannelGetLevel(value);
        }
        /// <summary>
        /// 获取FFT采样数据，返回512个浮点采样数据
        /// </summary>
        /// <returns></returns>
        public float[] GetFFTData()
        {
            float[] fft = new float[1024];
            Bass.BASS_ChannelGetData(this.ActiveStreamHandle, fft, (int)BASSData.BASS_DATA_FFT1024);
            return fft;
        }
        /// <summary>
        /// 停止当前音频，并释放资源
        /// </summary>
        public void Stop()
        {
            if (this.canStop)
            {
                this.ChannelPosition = TimeSpan.Zero;
                if (this.ActiveStreamHandle != 0)
                {
                    Bass.BASS_ChannelStop(this.ActiveStreamHandle);
                    Bass.BASS_ChannelSetPosition(this.ActiveStreamHandle, this.ChannelPosition.TotalSeconds);
                }
                this.IsPlaying = false;
                this.CanStop = false;
                this.CanPlay = false;
                this.CanPause = false;
            }           
            this.FreeCurrentStream();
            this.pendingOperation = BassEngine.PendingOperation.None;
        }
        /// <summary>
        /// 暂停当前音频
        /// </summary>
        public void Pause()
        {
            if (this.IsPlaying && this.CanPause)
            {
                Bass.BASS_ChannelPause(this.ActiveStreamHandle);
                this.IsPlaying = false;
                this.CanPlay = true;
                this.CanPause = false;
                this.pendingOperation = BassEngine.PendingOperation.None;
            }
            else
            {
                this.pendingOperation = BassEngine.PendingOperation.Pause;
            }
        }
        /// <summary>
        /// 播放当前音频
        /// </summary>
        public void Play()
        {
            if (this.CanPlay)
            {
                this.PlayCurrentStream();
                this.IsPlaying = true;
                this.CanPause = true;
                this.CanPlay = false;
                this.CanStop = true;
                this.pendingOperation = BassEngine.PendingOperation.None;
            }
            else
            {
                this.pendingOperation = BassEngine.PendingOperation.Play;
            }
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="filename">文件名</param>
        public void OpenFile(string filename)
        {
            this.openningFile = filename;
            this.Stop();
            this.pendingOperation = BassEngine.PendingOperation.None;
            int num = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_DEFAULT);
            if (filename.IndexOf("http://")>=0)
            {
                num = Bass.BASS_StreamCreateURL(filename,0,BASSFlag.BASS_DEFAULT,null,IntPtr.Zero);
            }
            if (num != 0)
            {
                this.ActiveStreamHandle = num;
                this.ChannelLength = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(this.ActiveStreamHandle, Bass.BASS_ChannelGetLength(this.ActiveStreamHandle, BASSMode.BASS_POS_BYTES)));
                BASS_CHANNELINFO bASS_CHANNELINFO = new BASS_CHANNELINFO();
                Bass.BASS_ChannelGetInfo(this.ActiveStreamHandle, bASS_CHANNELINFO);
                this.sampleFrequency = bASS_CHANNELINFO.freq;
                int num2 = Bass.BASS_ChannelSetSync(this.ActiveStreamHandle, BASSSync.BASS_SYNC_END, 0L, this.endTrackSyncProc, IntPtr.Zero);
                if (num2 == 0)
                {
                    throw new ArgumentException("Error establishing End Sync on file stream.", "path");
                }
                this.CanPlay = true;
                this.RaiseOpenSucceededEvent();
                switch (this.pendingOperation)
                {
                    case BassEngine.PendingOperation.Play:
                        this.Play();
                        break;
                    case BassEngine.PendingOperation.Pause:
                        this.Pause();
                        break;
                }
            }
            else
            {
                this.RaiseOpenFailedEvent(filename);
            }
        }

        /// <summary>
        /// 更新播放进度
        /// </summary>
        private void positionTimer_Tick()
        {
            if (this.ActiveStreamHandle == 0)
            {
                this.ChannelPosition = TimeSpan.Zero;
            }
            else
            {
                this.inChannelTimerUpdate = true;
                this.ChannelPosition = TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(this.ActiveStreamHandle, Bass.BASS_ChannelGetPosition(this.ActiveStreamHandle, BASSMode.BASS_POS_BYTES)));
                this.inChannelTimerUpdate = false;
            }
        }
        /// <summary>
        /// 查找设备的序号
        /// </summary>
        /// <param name="device">要查找的设备</param>
        /// <param name="returnDefault">当找不到设备时，是否返回默认设备的序号</param>
        /// <returns></returns>
        private static int FindDevice(DeviceInfo? device, bool returnDefault = false)
        {
            int result;
            if (device.HasValue)
            {
                int num = -1;
                BASS_DEVICEINFO[] devices = Bass.BASS_GetDeviceInfos();
                IEnumerable<int> source =
                    from d in devices
                    where d.id != null && d.id == device.Value.ID
                    select Array.IndexOf<BASS_DEVICEINFO>(devices, d);
                if (source.Count<int>() == 1)
                {
                    num = source.First<int>();
                }
                if (num == -1)
                {
                    source =
                        from d in devices
                        where d.name == device.Value.Name
                        select Array.IndexOf<BASS_DEVICEINFO>(devices, d);
                    if (source.Count<int>() == 1)
                    {
                        num = source.First<int>();
                    }
                }
                if (num == -1)
                {
                    source =
                        from d in devices
                        where d.driver == device.Value.Driver
                        select Array.IndexOf<BASS_DEVICEINFO>(devices, d);
                    if (source.Count<int>() == 1)
                    {
                        num = source.First<int>();
                    }
                }
                if (num == -1 && returnDefault)
                {
                    result = BassEngine.FindDefaultDevice();
                }
                else
                {
                    if (num == -1)
                    {
                        throw new Exception("找不到此设备：" + device.Value.Name);
                    }
                    result = num;
                }
            }
            else
            {
                result = BassEngine.FindDefaultDevice();
            }
            return result;
        }

        /// <summary>
        /// 返回默认设备的序号
        /// </summary>
        /// <returns></returns>
        public static int FindDefaultDevice()
        {
            BASS_DEVICEINFO[] array = Bass.BASS_GetDeviceInfos();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].IsDefault)
                {
                    return i;
                }
            }
            throw new Exception("没有默认设备");
        }

        /// <summary>
        /// 初始化BassEngine
        /// </summary>
        private void Initialize(DeviceInfo? device = null)
        {
            this.IsPlaying = false;
            IntPtr win = NETFM.hd;

            BassNet.Registration("545153113@qq.com", "2X213310160022");
            //检测播放组件版本
            if (Utils.HighWord(Bass.BASS_GetVersion()) != Bass.BASSVERSION)
            {
                MessageBox.Show("Wrong Bass Version!");
            }

            //初始化播放组件
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, NETFM.hd))
            {
                MessageBox.Show("BASS初始化错误" + Bass.BASS_ErrorGetCode().ToString());
            }
            else
            {
                string path =Application.StartupPath+ "\\Plugin";
                //string path = Path.Combine(Path.GetDirectoryName(typeof(BassEngine).Assembly.GetModules()[0].FullyQualifiedName), "Plugin");
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, true);
                string[] loadedPlugIns = Directory.GetFiles(path, "*.dll");
                foreach (string plugin in loadedPlugIns)
                {
                    Bass.BASS_PluginLoad(plugin);
                }
                Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT, 15000);     
            }

            //int i = BassEngine.FindDevice(device, true);
            //i = -1;
            ////-1, 44100
            //if (!Bass.BASS_Init(i, this.sampleFrequency, BASSInit.BASS_DEVICE_DEFAULT, win))
            //{
            //    BASSError code = Bass.BASS_ErrorGetCode();
            //    int num = Bass.BASS_GetDeviceCount();
            //    for (i = -1; i < num; i++)
            //    {
            //        if (i != 0 && Bass.BASS_Init(i, this.sampleFrequency, BASSInit.BASS_DEVICE_DEFAULT, win))
            //        {
            //            break;
            //        }
            //    }
            //    if (i == num)
            //    {
            //        throw new BassInitializationFailureException(code);
            //    }
            //}
            //if (!device.HasValue && i == BassEngine.FindDefaultDevice())
            //{
            //    this.Device = null;
            //}
            //else
            //{
            //    BASS_DEVICEINFO bASS_DEVICEINFO = Bass.BASS_GetDeviceInfo(Bass.BASS_GetDevice());
            //    this.Device = new DeviceInfo?(new DeviceInfo
            //    {
            //        Driver = bASS_DEVICEINFO.driver,
            //        Name = bASS_DEVICEINFO.name,
            //        ID = bASS_DEVICEINFO.id
            //    });
            //}    
        }
        /// <summary>
        /// 播放当前流
        /// </summary>
        private void PlayCurrentStream()
        {
            if (this.ActiveStreamHandle != 0 && Bass.BASS_ChannelPlay(this.ActiveStreamHandle, false))
            {
                BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                Bass.BASS_ChannelGetInfo(this.ActiveStreamHandle, info);
            }
            else
            {
                Debug.WriteLine("Error={0}", new object[]
				{
					Bass.BASS_ErrorGetCode()
				});
            }
        }
        /// <summary>
        /// 释放当前流
        /// </summary>
        private void FreeCurrentStream()
        {
            if (this.onlineFileWorker != null)
            {
                this.onlineFileWorker.Abort();
                this.onlineFileWorker = null;
            }
            if (this.ActiveStreamHandle != 0)
            {
                if (!Bass.BASS_StreamFree(this.ActiveStreamHandle))
                {
                    Debug.WriteLine("BASS_StreamFree失败：" + Bass.BASS_ErrorGetCode());
                }
                this.ActiveStreamHandle = 0;
            }
        }
        /// <summary>
        /// 设置音量
        /// </summary>
        private void SetVolume()
        {
            if (this.ActiveStreamHandle != 0)
            {
                float value = this.IsMuted ? 0f : ((float)this.Volume);
                Bass.BASS_ChannelSetAttribute(this.ActiveStreamHandle, BASSAttribute.BASS_ATTRIB_VOL, value);
            }
        }
        /// <summary>
        /// 播放完毕
        /// </summary>
        private void EndTrack(int handle, int channel, int data, IntPtr user)
        {
            this.Stop();
            this.RaiseTrackEndedEvent();
        }

        /// <summary>
        /// 引发播放完毕事件
        /// </summary>
        private void RaiseTrackEndedEvent()
        {
            if (this.TrackEnded != null)
            {
                this.TrackEnded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 引发打开音频文件失败事件
        /// </summary>
        private void RaiseOpenFailedEvent(string sb)
        {
            if (this.OpenFailed != null)
            {
                this.OpenFailed(this, sb);
            }
        }
        /// <summary>
        /// 引发打开音频文件成功事件
        /// </summary>
        private void RaiseOpenSucceededEvent()
        {
            if (this.OpenSucceeded != null)
            {
                this.OpenSucceeded(this, EventArgs.Empty);
            }
        }
        public bool GetFFTData(float[] fftDataBuffer)
        {
            return Bass.BASS_ChannelGetData(this.ActiveStreamHandle, fftDataBuffer, this.maxFFT) > 0;
        }
        public int GetFFTFrequencyIndex(int frequency)
        {
            return Utils.FFTFrequency2Index(frequency, 4096, this.sampleFrequency);
        }


        //public static void GetImage() 
        //{
          
        //  int channel = Bass.BASS_StreamCreateFile("E:\\My\\Dream.Machine\\【Music】\\音乐\\[梦飞船]不值得.mp3", 0, 0, BASSFlag.BASS_SAMPLE_FLOAT);
        //  IntPtr tag = Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_MUSIC_SAMPLE);
        //  string[] tags = Utils.IntPtrToArrayNullTermUtf8(tag);
        //  if (tags != null)
        //  {
        //      foreach (string t in tags)
        //      {
        //          MessageBox.Show(t);
        //      }
        //  }
        //}
    }
}
