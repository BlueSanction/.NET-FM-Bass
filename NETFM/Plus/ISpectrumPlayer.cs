using System;
using System.ComponentModel;

namespace NETFM
{
    public interface ISpectrumPlayer : ISoundPlayer, INotifyPropertyChanged
    {
        bool GetFFTData(float[] fftDataBuffer);
        int GetFFTFrequencyIndex(int frequency);
    }
}
