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
        [ObservableProperty]
        private Methods.ChosenFormat _myChosenFormat = Methods.ChosenFormat.Ani;


        [ObservableProperty]
        private string _outFolder = Directory.GetCurrentDirectory().ToString();

        [ObservableProperty]
        private string _logText = "";

        public ObservableCollection<String> ListOfFiles { get; set; }

        public Methods.ChosenFormat[] OutputChosenFormatList { get; }

        public MainWindowViewModel()
        {
            ListOfFiles = new ObservableCollection<String>(new List<String>());
            OutputChosenFormatList = EnumHelper.GetEnumValues<Methods.ChosenFormat>().ToArray();
            MyChosenFormat = Methods.ChosenFormat.Ico;
        }


        [RelayCommand]
        public async Task OpenFile(CancellationToken token)
        {
            ErrorMessages?.Clear();
            try
            {
                var files = await DoOpenFilePickerAsync();
                if (files != null)
                {
                    ListOfFiles.Clear();
                    foreach (var file in files)
                    {
                        string tempstr = file.TryGetLocalPath();
                        ListOfFiles.Add(tempstr);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }


        private async Task<IReadOnlyList<IStorageFile>?> DoOpenFilePickerAsync()
        {

            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose icons to convert",
                AllowMultiple = true
            });

            if (files?.Count >= 1)
            {
                return files;
            }
            else
            {
                return null;
            }
        }


        [RelayCommand]
        public async Task OpenOutFolder(CancellationToken token)
        {
            ErrorMessages?.Clear();
            try
            {
                var folder = await DoOpenFolderPickerOneAsync();
                if (folder != null)
                {
                    OutFolder = folder.TryGetLocalPath();
                }

            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }


        [RelayCommand]
        public async Task OpenFolder(CancellationToken token)
        {
            ErrorMessages?.Clear();
            try
            {
                var folders = await DoOpenFolderPickerAsync();
                if (folders != null)
                {

                    ListOfFiles.Clear();
                    foreach (var folder in folders)
                    {
                        List<String> files = Methods.AllFiles(folder.TryGetLocalPath());

                        foreach (var file in files) { ListOfFiles.Add(file); } //no addrange() because it's observable
                    }
                }

            }
            catch (Exception e)
            {
                ErrorMessages?.Add(e.Message);
            }
        }

        private async Task<IStorageFolder?> DoOpenFolderPickerOneAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var result = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Choose output folder for saving the cursors",
                AllowMultiple = false
            });
            if (result?.Count >= 1)
            {
                //Console.WriteLine(result[0].TryGetLocalPath());
                return result[0];
            }
            else
            {
                return null;
            }
        }


        private async Task<IReadOnlyList<IStorageFolder>?> DoOpenFolderPickerAsync()
        {

            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var result = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Choose output folder for saving the cursors",
                AllowMultiple = true
            });
            if (result?.Count >= 1)
            {
                //Console.WriteLine(result[0].TryGetLocalPath());
                return result;
            }
            else
            {
                return null;
            }
        }


        [RelayCommand]
        private async void StartConversion()
        {
            List<string> locallist = ListOfFiles.ToList();
            Methods.ChosenFormat chosenFormat = MyChosenFormat;
            Methods.ExecutionStarts(locallist, OutFolder, chosenFormat);
            LogText = "Finished";
        }
    }
}
