using JK.Wpf.Ex.Enums;
using JK.Wpf.Ex.Models;
using JK.Wpf.Helpers;
using JK.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace JK.Wpf.Ex.ViewModels
{
    public class MainViewModel : DispatcherViewModel
    {
        public MainViewModel(Dispatcher dispatcher) : base(dispatcher)
        {
            ViewModels = new Dictionary<NavType, DispatcherViewModel>
            {
                { NavType.Ex, new ExViewModel(Dispatcher) },
                { NavType.Pupup, new PupupViewModel(Dispatcher)},
            };

            ObservableCollection<NavItem> menuItems = new ObservableCollection<NavItem>
            {
                new NavItem { NavName = "Ex", NavType = NavType.Ex, NavImage = @"/Resources/Images/ex.png", IsChecked = true },

                new NavItem { NavName = "Popup", NavType = NavType.Pupup, NavImage = @"/Resources/Images/in.png", IsChecked = false },
            };

            MenuItemsCollection = new CollectionViewSource { Source = menuItems };

            SourceCollection = MenuItemsCollection.View;

            ClickNav(NavType.Ex);
        }

        private CollectionViewSource MenuItemsCollection;

        private Dictionary<NavType, DispatcherViewModel> ViewModels = new Dictionary<NavType, DispatcherViewModel>();

        private ICollectionView _SourceCollection = null;
        public ICollectionView SourceCollection
        {
            get { return _SourceCollection; }
            set { SetProperty(ref _SourceCollection, value); }

        }

        private NavType _CurrentNavType = NavType.Ex;
        public NavType CurrentNavType
        {
            get { return _CurrentNavType; }
            set { SetProperty(ref _CurrentNavType, value); }

        }

        private object _CurrentViewModel = null;
        public object CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }

        }

        public ICommand NavCommand => new RelayCommand<NavType>(ClickNav);

        private void ClickNav(NavType type)
        {
            CurrentNavType = type;

            CurrentViewModel = ViewModels[type];
        }

        public ICommand ExitCommand => new RelayCommand<object>(Exit);

        private void Exit(object ob)
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}
