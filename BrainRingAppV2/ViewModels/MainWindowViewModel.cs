using BrainRingAppV2.Commands;
using BrainRingAppV2.Models;
using BrainRingAppV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BrainRingAppV2.ViewModels
{
    internal sealed class MainWindowViewModel : BaseViewModel
    {
        private SerialPortService _serialPortService;
        private ObservableCollection<ButtonState> _buttonStates;
        private string _receivedData;

        public ObservableCollection<ButtonState> ButtonStates
        {
            get => _buttonStates;
            set => Set(ref _buttonStates, value);
        }

        private ObservableCollection<ButtonViewModel> _buttonViewModels;
        public ObservableCollection<ButtonViewModel> ButtonViewModels
        {
            get => _buttonViewModels;
            set => Set(ref _buttonViewModels, value);
        }

        public string ReceivedData
        {
            get => _receivedData;
            set => Set(ref _receivedData, value);            
        }

        private string _countdownText;
        public string CountdownText
        {
            get => _countdownText;
            set => Set(ref _countdownText, value);
        }

        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        private string _firstCandidate;
        private string _secondCandidate;
        private string _thirdCandidate;

        public string FirstCandidate
        {
            get => _firstCandidate;
            set => Set(ref _firstCandidate, value);
        }

        public string SecondCandidate
        {
            get => _secondCandidate;
            set => Set(ref _secondCandidate, value);
        }

        public string ThirdCandidate
        {
            get => _thirdCandidate;
            set => Set(ref _thirdCandidate, value);
        }



        public ICommand OpenPortCommand { get; }
        public ICommand ClosePortCommand { get; }        

        public MainWindowViewModel()
        {
            _serialPortService = new SerialPortService();
            ButtonStates = new ObservableCollection<ButtonState>();            
            ButtonViewModels = new ObservableCollection<ButtonViewModel>(); // Добавьте эту строку

            OpenPortCommand = new RelayCommand(o => _serialPortService.OpenPort("COM5"), o => !_serialPortService.IsOpen);
            ClosePortCommand = new RelayCommand(o => _serialPortService.ClosePort(), o => _serialPortService.IsOpen);

            // Subscribe to the DataReceived event and update ReceivedData when data is received
            _serialPortService.DataReceived += (sender, data) =>
            {
                ReceivedData = data.Trim();
                ParseAndDisplayData(data);
            };
        }

        private void ParseAndDisplayData(string receivedData)
        {
            string[] parts = receivedData.Split(':');
            if (parts.Length < 9) return;

            ParseSystemState(parts[0]);
            for (int i = 1; i <= 8; i++)
            {
                ParseButtonState(parts[i], i);
            }
        }

        private void ParseSystemState(string systemStateData)
        {
            if (systemStateData.Length < 10) return;

            char phase = systemStateData[1]; // Фаза работы устройства
            int currentTime = ParseTime(systemStateData.Substring(2, 4)); // Текущее время
            int totalTime = ParseTime(systemStateData.Substring(6, 4)); // Общее время

            UpdateCountdownDisplay(currentTime, totalTime);
            UpdateStatus(phase);
        }

        private void ParseButtonState(string buttonStateData, int buttonId)
        {
            if (buttonStateData.Length < 6) return;

            char buttonState = buttonStateData[0]; // Состояние кнопки
            int pressOrder = int.Parse(buttonStateData.Substring(1, 1), CultureInfo.InvariantCulture); // Порядок нажатия
            int pressTime = ParseTime(buttonStateData.Substring(2, 4)); // Время нажатия

            UpdateButtonDisplay(buttonId, buttonState, pressOrder, pressTime);
        }

        private int ParseTime(string timeData)
        {
            // Конвертация из шестнадцатеричной строки в десятичное число
            return int.Parse(timeData, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        private void UpdateCountdownDisplay(int currentTime, int totalTime)
        {
            try
            {
                if (Application.Current == null) return;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var currentTimeInSeconds = TimeSpan.FromMilliseconds(currentTime).TotalSeconds;
                    var totalTimeInSeconds = TimeSpan.FromMilliseconds(totalTime).TotalSeconds;

                    //CountdownText = $"Текущее время: {currentTimeInSeconds:F2} сек / Общее время: {totalTimeInSeconds:F2} сек";
                    CountdownText = $"{currentTimeInSeconds:F2}";
                });
            }
            catch (Exception e) 
            { 
                ErrorMessage = e.ToString();
            }
        }

        private void UpdateButtonDisplay(int buttonId, char buttonState, int pressOrder, int pressTime)
        {
            if (Application.Current == null) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var button = ButtonStates.FirstOrDefault(b => b.ButtonId == buttonId);
                if (button == null)
                {
                    button = new ButtonState { ButtonId = buttonId };
                    ButtonStates.Add(button);
                }

                button.State = GetButtonStateDescription(buttonState);
                button.PressTime = pressTime;
                button.PressOrder = pressOrder;
                button.StateColor = GetButtonStateColor(buttonState);

                var buttonViewModel = ButtonViewModels.FirstOrDefault(b => b.ButtonId == buttonId);
                if (buttonViewModel == null)
                {
                    buttonViewModel = new ButtonViewModel { ButtonId = buttonId };
                    ButtonViewModels.Add(buttonViewModel);
                }

                buttonViewModel.Visibility = Visibility.Visible;
                buttonViewModel.Background = GetButtonStateColor(buttonState);
                buttonViewModel.Text = $"{buttonId}"; // Или любой другой текст в зависимости от состояния
                buttonViewModel.PressOrder = pressOrder;
                buttonViewModel.PressTime = pressTime;  

                // Обновление списка кандидатов
                UpdateCandidatesDisplay();
            });
        }

        private void UpdateCandidatesDisplay()
        {
            var candidates = ButtonStates
                .Where(b => b.State == "Нажата вовремя")
                .OrderBy(b => b.PressOrder)
                .Take(3)
                //.Select(b => $"{b.ButtonId}\n{Math.Round((double)(b.PressTime / 1000), 3)}")
                .Select(b => $"{b.ButtonId}")
                .ToList();



            FirstCandidate = candidates.ElementAtOrDefault(0) ?? string.Empty;
            SecondCandidate = candidates.ElementAtOrDefault(1) ?? string.Empty;
            ThirdCandidate = candidates.ElementAtOrDefault(2) ?? string.Empty;
        }

        private Brush GetButtonStateColor(char state)
        {
            return state switch
            {
                '1' => Brushes.Red,    // Фальстарт
                '2' => Brushes.Yellow, // Нажата рано
                '3' => Brushes.Green,  // Нажата вовремя
                _ => Brushes.Gray      // Не нажата или неизвестное состояние
            };
        }

        private string GetButtonStateDescription(char state)
        {
            return state switch
            {
                '0' => "Не нажата",
                '1' => "Фальстарт",
                '2' => "Нажата рано",
                '3' => "Нажата вовремя",
                _ => "Неизвестное состояние"
            };
        }

        private void UpdateStatus(char phase)
        {
            StatusText = $"Фаза: {GetPhaseDescription(phase)}";
        }

        private string GetPhaseDescription(char phase)
        {
            return phase switch
            {
                '0' => "IDLE (RESET ALL)",
                '1' => "REGISTER FALSES",
                '2' => "REGISTER EARLY",
                '3' => "REGISTER OTHER",
                '4' => "STOPPED",
                _ => "Неизвестная фаза"
            };
        }

    }
}