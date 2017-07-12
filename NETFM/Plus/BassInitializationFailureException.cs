using System;
using System.Text;
using Un4seen.Bass;

namespace NETFM
{
    public class BassInitializationFailureException : Exception
    {
        public BASSError Code
        {
            get;
            private set;
        }
        public static string GetErrorMessage(BASSError code)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("音频组件初始化失败： ");
            string value;
            if (code <= BASSError.BASS_ERROR_FORMAT)
            {
                switch (code)
                {
                    case BASSError.BASS_ERROR_UNKNOWN:
                    case BASSError.BASS_OK:
                    case BASSError.BASS_ERROR_FILEOPEN:
                        break;
                    case BASSError.BASS_ERROR_MEM:
                        value = "内存不足。";
                        goto IL_9E;
                    case BASSError.BASS_ERROR_DRIVER:
                        value = "没有可用的设备驱动，设备可能正在使用。";
                        goto IL_9E;
                    default:
                        if (code == BASSError.BASS_ERROR_FORMAT)
                        {
                            value = "设备不支持此格式。";
                            goto IL_9E;
                        }
                        break;
                }
            }
            else
            {
                if (code == BASSError.BASS_ERROR_ALREADY)
                {
                    value = "设备已经初始化。";
                    goto IL_9E;
                }
                switch (code)
                {
                    case BASSError.BASS_ERROR_NO3D:
                        value = "无法初始化3D支持。";
                        goto IL_9E;
                    case BASSError.BASS_ERROR_NOEAX:
                        break;
                    case BASSError.BASS_ERROR_DEVICE:
                        value = "无效的设备。";
                        goto IL_9E;
                    default:
                        if (code == BASSError.BASS_ERROR_DX)
                        {
                            value = "未安装DirectX。";
                            goto IL_9E;
                        }
                        break;
                }
            }
            value = "未知错误。";
            IL_9E:
            stringBuilder.Append(value);
            return stringBuilder.ToString();
        }
        public BassInitializationFailureException(BASSError code)
            : base(BassInitializationFailureException.GetErrorMessage(code))
        {
            this.Code = code;
        }
    }
}
