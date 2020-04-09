using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Khadamat_CustomerApp.DependencyInterface
{
    public interface IFirebaseTokenUpdate
    {
        Task<string> GetNewFirebaseToken();
    }
}
