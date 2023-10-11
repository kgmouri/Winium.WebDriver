using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenQA.Selenium.Winium
{
    public interface IFindsWiniumElement
    {
        WiniumElement FindElement(string mechanism, string value);

        ReadOnlyCollection<WiniumElement> FindElements(string mechanism, string value);
    }
}
