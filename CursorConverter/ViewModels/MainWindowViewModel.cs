using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CursorConverter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CursorConverter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        MainViewModel myMainViewModel;
        
        [ObservableProperty]
        public ViewModelBase? contentViewModel;

        public MainWindowViewModel()
        {
            myMainViewModel = new MainViewModel();
            ContentViewModel = myMainViewModel;
        }
    }
}
