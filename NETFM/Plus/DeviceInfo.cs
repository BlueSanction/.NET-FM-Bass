﻿using System;

namespace NETFM
{
    [Serializable]
    public struct DeviceInfo
    {
        public string Driver
        {
            get;
            set;
        }
        public string ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
