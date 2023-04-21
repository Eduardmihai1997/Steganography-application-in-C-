using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageSecret.Services.SteganographyService;

namespace ImageSecret.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private BitmapImage _image;
        private string _message;
        private readonly ISteganographyService _steganographyService;

        public RelayCommand SaveImageCommand { get; }

        // Add this line to the constructor

        // Prepare the commands, inject the ISteganographyService service
        public MainViewModel(ISteganographyService steganographyService)
        {
            _steganographyService = steganographyService;
            LoadImageCommand = new RelayCommand(LoadImage);
            EncodeCommand = new RelayCommand(Encode, CanExecuteEncodeOrDecode);
            DecodeCommand = new RelayCommand(Decode, () => Image != null);
            SaveImageCommand = new RelayCommand(SaveImage, () => Image != null);
        }

        // Saves image
        private void SaveImage()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png",
                FileName = "EncodedImage.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_image));
                    encoder.Save(fileStream);
                }
            }
        }

        public RelayCommand LoadImageCommand { get; }
        public RelayCommand EncodeCommand { get; }

        public RelayCommand DecodeCommand { get; }

        public BitmapImage Image
        {
            get => _image;
            set
            {
                SetProperty(ref _image, value);
                DecodeCommand.NotifyCanExecuteChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
                EncodeCommand.NotifyCanExecuteChanged();
                DecodeCommand.NotifyCanExecuteChanged();
            }
        }

        // Load image
        private void LoadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage loadedImage = new BitmapImage();
                loadedImage.BeginInit();
                loadedImage.CacheOption = BitmapCacheOption.OnLoad;
                loadedImage.UriSource = new Uri(openFileDialog.FileName);
                loadedImage.EndInit();
                loadedImage.Freeze();

                Image = loadedImage;
            }
        }

        //Encode the image
        private void Encode()
        {
            var encodedImage = _steganographyService.Encode(Image, Message);
            Image = encodedImage;
            SaveImageCommand.NotifyCanExecuteChanged();
            MessageBox.Show("Message encoded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Decode the image
        private void Decode()
        {
            try
            {
                var decodedMessage = _steganographyService.Decode(Image);
                MessageBox.Show($"Decoded message: {decodedMessage}", "Decoded Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No encoded message", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Check if can encode or decode
        private bool CanExecuteEncodeOrDecode()
        {
            return Image != null && !string.IsNullOrWhiteSpace(Message);
        }
    }
}