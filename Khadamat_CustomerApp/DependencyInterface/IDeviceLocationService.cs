using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.DependencyInterface
{
    public interface IDeviceLocationService
    {
        bool CheckDeviceLocation();
        void OpenDeviceSetting();
    }
}
