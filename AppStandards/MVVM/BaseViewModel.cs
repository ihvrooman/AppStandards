using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppStandards.MVVM
{
    /// <summary>
    /// Base ViewModel used in MVVM applications. Implements <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public class BaseViewModel : PropertyChangedHelper
    {
    }
}
