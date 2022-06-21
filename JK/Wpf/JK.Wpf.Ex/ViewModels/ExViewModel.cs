using JK.Wpf.Helpers;
using JK.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace JK.Wpf.Ex.ViewModels
{
    public class ExViewModel : DispatcherViewModel
    {
        public ExViewModel(Dispatcher dispatcher) : base(dispatcher)
        {

        }

        private string _Encrypt = string.Empty;
        public string Encrypt
        {
            get { return _Encrypt; }
            set { SetProperty(ref _Encrypt, value); }
        }

        private string _EncryptText = string.Empty;
        public string EncryptText
        {
            get { return _EncryptText; }
            set { SetProperty(ref _EncryptText, value); }
        }

        public ICommand EncryptCommand => new RelayCommand<string>(Encrypting);


        private string _Decrypt = string.Empty;
        public string Decrypt
        {
            get { return _Decrypt; }
            set { SetProperty(ref _Decrypt, value); }
        }

        private string _DecryptText = string.Empty;
        public string DecryptText
        {
            get { return _DecryptText; }
            set { SetProperty(ref _DecryptText, value); }
        }

        public ICommand DecryptCommand => new RelayCommand<string>(Decrypting);

        public void Encrypting(string pram)
        {
 
        }

        public void Decrypting(string pram)
        {

        }
    }
}
