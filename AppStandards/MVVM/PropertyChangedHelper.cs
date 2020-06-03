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
    /// Implementation of <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class PropertyChangedHelper : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged members
        /// <summary>
        /// The <see cref="PropertyChangedEventHandler"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. If the property name is not specified, it will be resolved automatically.</param>
        protected void RaisePropertyChangedEvent([CallerMemberName]string propertyName = "")
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
