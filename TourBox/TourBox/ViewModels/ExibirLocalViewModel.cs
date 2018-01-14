using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourbox.Helpers;
using TourBox.Helpers;
using TourBox.Models;
using TourBox.Plugins;
using TourBox.Services;
using Xamarin.Forms;

namespace TourBox.ViewModels
{
    class ExibirLocalViewModel : BaseViewModel
    {
        private LocaisViewModel _localViewModel;
        public LocaisViewModel LocalViewModel
        {
            get { return _localViewModel; }
            set
            {
                _localViewModel = value;
                OnPropertyChanged();
            }
        }


        private LocalModel _local;
        public LocalModel Local
        {
            get { return _local; }
            set
            {
                _local = value;
                OnPropertyChanged();
            }
        }

        private object _photo;
        public object Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoadingPhoto;
        public bool IsLoadingPhoto
        {
            get { return _isLoadingPhoto; }
            set
            {
                _isLoadingPhoto = value;
                OnPropertyChanged();
            }
        }

        private Stream _photoStream;
        internal static readonly object Instance;

        //public ICommand CompartilharComand
        //    => new Command(async () => await Compartilhar());

        public ICommand GetPhotoCommand
            => new Command(async () => await GetPhoto());

        public ExibirLocalViewModel(LocalModel localModel)
        {
            Local = localModel;
            //LoadDefaultPhoto();
            GetPhotoCommand.Execute(null);
        }

        private async Task GetPhoto()
        {
            if (IsLoadingPhoto)
                return;

            if (string.IsNullOrEmpty(Local.PhotoName))
                return;

            try
            {
                IsLoadingPhoto = true;
                var bytes = await AzureStorageService.Instance.DownloadFile(Local.PhotoName);
                if (bytes == null)
                    throw new Exception("Nenhuma imagem para carregar!");

                Photo = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(bytes);
                });
            }
            catch (Exception e)
            {
                LogHelper.Instance.AddLog(e);
            }
            finally
            {
                IsLoadingPhoto = false;
            }
        }
    }
}


