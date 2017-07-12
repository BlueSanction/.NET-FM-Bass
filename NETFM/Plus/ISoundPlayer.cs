using System;
using System.ComponentModel;

namespace NETFM
{
    public interface ISoundPlayer : INotifyPropertyChanged
    {
        bool IsPlaying
        {
            get;
        }
    }
}
