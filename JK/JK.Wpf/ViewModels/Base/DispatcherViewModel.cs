using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JK.Wpf.ViewModels
{
    public class DispatcherViewModel : BaseViewModel
    {
        /// <summary>
        /// Dispatcher
        /// </summary>
        public Dispatcher Dispatcher { get; }

        public DispatcherViewModel(Dispatcher dispatcher)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));

            Dispatcher = dispatcher;
        }
    }
}
