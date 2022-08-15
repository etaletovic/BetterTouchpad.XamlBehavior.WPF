using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace TestApp
{
    public class MainViewModel : ObservableObject
    {
        private string message = string.Empty;

        public ICommand HorizontalScrollGridCommand { get; set; }

        public ICommand VerticalScrollGridCommand { get; set; }

        public ICommand HorizontalScrollCommand { get; set; }

        public ICommand VerticalScrollCommand { get; set; }

        public string Message { get => message; set => SetProperty(ref message, value); }

        public MainViewModel()
        {
            Message = string.Empty;

            HorizontalScrollGridCommand = new RelayCommand<object>(x =>
            {
                Message = "Horizontal scroll inside Grid";
            });

            VerticalScrollGridCommand = new RelayCommand<object>(x =>
            {
                Message = "Vertical scroll inside Grid";
            });

            HorizontalScrollCommand = new RelayCommand<object>(x =>
            {
                Message = "Horizontal scroll inside Window";
            });

            VerticalScrollCommand = new RelayCommand<object>(x =>
            {
                Message = "Vertical scroll inside Window";
            });
        }
    }
}
