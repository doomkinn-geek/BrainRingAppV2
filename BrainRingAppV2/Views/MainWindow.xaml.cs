using BrainRingAppV2.ViewModels;
using System.Windows;

namespace BrainRingAppV2.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;

            // Добавляем обработчик события закрытия окна
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Вызываем команду ClosePortCommand перед закрытием окна
            if (_viewModel != null && _viewModel.ClosePortCommand.CanExecute(null))
            {
                _viewModel.ClosePortCommand.Execute(null);
            }
        }
    }
}
