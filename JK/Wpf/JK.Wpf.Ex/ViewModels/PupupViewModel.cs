using JK.Wpf.Helpers;
using JK.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace JK.Wpf.Ex.ViewModels
{
    public class PupupViewModel : DispatcherViewModel
    {
        public PupupViewModel(Dispatcher dispatcher) : base(dispatcher)
        {

        }

        private string _Msg = string.Empty;
        public string Msg
        {
            get { return _Msg; }
            set { SetProperty(ref _Msg, value); }
        }

        public ICommand ToastCommand => new RelayCommand<string>(ToastAction);

        public ICommand AlertCommand => new RelayCommand<string>(AlertAction);

        public void ToastAction(string pram)
        {
 
        }

        public void AlertAction(string pram)
        {

        }
    }
}
