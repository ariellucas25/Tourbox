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
    public class DetailsViewModel : BaseViewModel
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

        public ICommand SaveCommand
            => new Command(async () => await Save());

        public ICommand DeleteCommand
            => new Command(async () => await Delete());

        public ICommand ChangePhotoCommand
            => new Command(async () => await ChangePhoto());

        public ICommand GetPhotoCommand
            => new Command(async () => await GetPhoto());

        public DetailsViewModel(LocalModel localModel)
        {
            Local = localModel;
            LoadDefaultPhoto();
            GetPhotoCommand.Execute(null);
        }

        public void LoadDefaultPhoto()
        {
            Photo = "profile.png";
            _photoStream = null;
        }

        private async Task Save()
        {
            if (IsBusy)
                return;

            Exception exception = null;

            try
            {
                IsBusy = true;

                if (string.IsNullOrEmpty(Local.Nome))
                    throw new Exception("O nome é obrigatório!");

                if (string.IsNullOrEmpty(Local.Descricao))
                    throw new Exception("A Descrição é obrigatória!");

                if (string.IsNullOrEmpty(Local.Endereco))
                    throw new Exception("O endereço é obrigatório!");


                if (_photoStream != null)
                {
                    if (string.IsNullOrEmpty(Local.PhotoName))
                        Local.PhotoName = Guid.NewGuid().ToString();

                    await AzureStorageService.Instance.UploadFile(_photoStream, Local.PhotoName);
                }

                await AzureMobileService.Instance.SaveLocal(Local);
            }
            catch (Exception e)
            {
                exception = e;
                LogHelper.Instance.AddLog(e);
            }
            finally
            {
                IsBusy = false;
                
            }

            if (exception != null)
            {
                await MessageHelper.Instance.ShowMessage(
                    "An error has occurred",
                    exception.Message,
                    "Ok"
                );
                return;
            }

            
            await NavigationHelper.Instance.GoBack();
            await MessageHelper.Instance.ShowMessage("Adicionado!", "Você adicionou um novo destino!", "Ok");
            
            //await NavigationHelper.Instance.GoToLocaisPage();
            //await NavigationHelper.GetMainPage().PopAsync();

        }

        private async Task Delete()
        {
            if (IsBusy)
                return;

            if (string.IsNullOrEmpty(Local.Id))
                return;

            var delete = await MessageHelper.Instance.ShowAsk(
                "Deletar",
                "Deseja deletar o local?",
                "Sim",
                "Não"
            );

            if (delete == false)
                return;

            Exception exception = null;

            try
            {
                IsBusy = true;

                if (string.IsNullOrEmpty(Local.PhotoName) == false)
                    await AzureStorageService.Instance.DeleteFile(Local.PhotoName);

                await AzureMobileService.Instance.DeleteLocal(Local);
            }
            catch (Exception e)
            {
                exception = e;
                LogHelper.Instance.AddLog(e);
            }
            finally
            {
                IsBusy = false;
            }

            if (exception != null)
            {
                await MessageHelper.Instance.ShowMessage(
                    "An error has occurred",
                    exception.Message,
                    "Ok"
                );
                return;
            }

            await NavigationHelper.Instance.GoBack();
        }

        private async Task ChangePhoto()
        {
            var textTakePicture = "Nova imagem";
            var textOpenGallery = "Abrir Galeria";
            var textCancel = "Cancelar";
            var textDelete = "Deletar";

            var actions = new string[] { textTakePicture, textOpenGallery };

            var response = await MessageHelper.Instance.ShowOptions(
                "O que você deseja fazer?",
                textCancel,
                textDelete,
                actions
            );

            if (response == textCancel)
                return;

            Exception exception = null;

            try
            {
                if (response == textOpenGallery)
                {
                    if (await MediaService.Instance.IsPickPhotoSupported() == false)
                        throw new Exception("Não foi possível abrir a galera.");

                    var file = await MediaService.Instance.PickPhotoAsync();
                    if (file != null)
                    {
                        Photo = ImageSource.FromFile(file.Path);
                        _photoStream = file.GetStream();
                    }
                }

                if (response == textTakePicture)
                {
                    if (await MediaService.Instance.IsCameraAvailable() == false)
                        throw new Exception("A câmera não está disponível no seu dispositivo!");

                    var file = await MediaService.Instance.TakePhotoAsync();
                    if (file != null)
                    {
                        Photo = ImageSource.FromFile(file.Path);
                        _photoStream = file.GetStream();
                    }
                }

                if (response == textDelete)
                {
                    await AzureStorageService.Instance.DeleteFile(Local.PhotoName);
                    Local.PhotoName = null;
                    LoadDefaultPhoto();
                }
            }
            catch (Exception e)
            {
                exception = e;
                LogHelper.Instance.AddLog(e);
            }

            if (exception != null)
            {
                await MessageHelper.Instance.ShowMessage(
                    "Algo deu errado :/",
                    exception.Message,
                    "Ok"
                );
                return;
            }
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
