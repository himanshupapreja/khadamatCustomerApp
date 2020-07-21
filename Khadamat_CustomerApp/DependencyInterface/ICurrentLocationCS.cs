using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Khadamat_CustomerApp.DependencyInterface
{
    public interface ICurrentLocationCS
    {
        Task<Position> getLocation();
    }
}
