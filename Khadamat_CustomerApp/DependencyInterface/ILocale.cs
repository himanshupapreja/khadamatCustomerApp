using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.DependencyInterface
{
    public interface ILocale
    {
        string GetCurrent();

        void SetLocale();
        string SetLocale(string culturevalue);
    }
}
