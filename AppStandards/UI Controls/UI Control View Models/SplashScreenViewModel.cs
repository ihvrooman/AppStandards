using AppStandards.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStandards.UI_Controls.UI_Control_View_Models
{
    internal class SplashScreenViewModel : BaseViewModel
    {
        #region Fields
        private IAppInfo _appInfo;
        #endregion

        #region Properties
        public IAppInfo AppInfo { get { return _appInfo; } set { _appInfo = value; RaisePropertyChangedEvent(); } }
        #endregion

        public SplashScreenViewModel(IAppInfo appInfo)
        {
            AppInfo = appInfo;
        }
    }
}
