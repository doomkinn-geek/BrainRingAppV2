using BrainRingAppV2.Commands;
using BrainRingAppV2.Models;
using BrainRingAppV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
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

        // В ViewModel добавьте
        private ButtonViewModel _firstCandidateViewModel;
        private ButtonViewModel _secondCandidateViewModel;
        private ButtonViewModel _thirdCandidateViewModel;

        public ButtonViewModel FirstCandidateViewModel
        {
            get => _firstCandidateViewModel;
            set => Set(ref _firstCandidateViewModel, value);
        }

        public ButtonViewModel SecondCandidateViewModel
        {
            get => _secondCandidateViewModel;
            set => Set(ref _secondCandidateViewModel, value);
        }

        public ButtonViewModel ThirdCandidateViewModel
        {
            get => _thirdCandidateViewModel;
            set => Set(ref _thirdCandidateViewModel, value);
        }

        public ObservableCollection<string> AvailablePorts { get; private set; }
        public string SelectedPort { get; set; }

        private bool isTestPanelVisible;
        public bool IsTestPanelVisible
        {
            get => isTestPanelVisible;
            set => Set(ref isTestPanelVisible, value);
        }




        public ICommand OpenPortCommand { get; }
        public ICommand ClosePortCommand { get; }
        public ICommand ToggleTestPanelCommand { get; }
        private RelayCommand secondsDecrease;
        public ICommand SecondsDecrease => secondsDecrease ??= new RelayCommand(PerformSecondsDecrease);
        private RelayCommand secondIncrease;
        public ICommand SecondIncrease => secondIncrease ??= new RelayCommand(PerformSecondIncrease);
        private RelayCommand addTestDataCommand;
        public ICommand AddTestDataCommand => addTestDataCommand ??= new RelayCommand(AddTestData);


        public MainWindowViewModel()
        {
            _serialPortService = new SerialPortService();
            ButtonStates = new ObservableCollection<ButtonState>();            
            ButtonViewModels = new ObservableCollection<ButtonViewModel>(); // Добавьте эту строку

            AvailablePorts = new ObservableCollection<string>(SerialPort.GetPortNames());
            SelectedPort = AvailablePorts.FirstOrDefault();

            OpenPortCommand = new RelayCommand(
                o => _serialPortService.OpenPort(SelectedPort),
                o => !_serialPortService.IsOpen && !string.IsNullOrEmpty(SelectedPort));
            ClosePortCommand = new RelayCommand(o => _serialPortService.ClosePort(), o => _serialPortService.IsOpen);

            // Subscribe to the DataReceived event and update ReceivedData when data is received
            _serialPortService.DataReceived += (sender, data) =>
            {
                ReceivedData = data.Trim();
                ParseAndDisplayData(data);
                ErrorMessage = "";
            };

            _serialPortService.ErrorOcured += (sender, error) =>
            {
                ErrorMessage = error.ToString();
            };

            ToggleTestPanelCommand = new RelayCommand(ToggleTestPanel);
            Seconds = 0;
        }
        private void ToggleTestPanel(object parameter)
        {
            IsTestPanelVisible = !IsTestPanelVisible;
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

            if(Enum.TryParse(systemStateData[1].ToString(), out PhaseEnum phase))
    {
                int currentTime = ParseTime(systemStateData.Substring(2, 4)); // Текущее время
                int totalTime = ParseTime(systemStateData.Substring(6, 4)); // Общее время

                UpdateCountdownDisplay(currentTime, totalTime);
                UpdateStatus(phase);
            }            
        }

        private void ParseButtonState(string buttonStateData, int buttonId)
        {
            if (buttonStateData.Length < 6) return;

            if (Enum.TryParse(buttonStateData[0].ToString(), out ButtonStateEnum buttonState))
            {
                int pressOrder = int.Parse(buttonStateData.Substring(1, 1), CultureInfo.InvariantCulture); // Порядок нажатия
                int pressTime = ParseTime(buttonStateData.Substring(2, 4)); // Время нажатия

                UpdateButtonDisplay(buttonId, buttonState, pressOrder, pressTime);
            }
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
                    Seconds = (int)Math.Ceiling(currentTimeInSeconds);
                });
            }
            catch (Exception e) 
            { 
                ErrorMessage = e.ToString();
            }
        }

        private void UpdateButtonDisplay(int buttonId, ButtonStateEnum buttonState, int pressOrder, int pressTime)
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

                button.State = buttonState;
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
            // Обновите этот метод, чтобы он устанавливал модели представления для кандидатов
            var candidates = ButtonStates
                .Where(b => b.State == ButtonStateEnum.PressedOnTime || b.State == ButtonStateEnum.PressedEarly)
                .OrderBy(b => b.PressOrder)
                .Take(3)
                .ToList();

            FirstCandidateViewModel = candidates.Count > 0 ? ConvertToButtonViewModel(candidates[0]) : null;
            SecondCandidateViewModel = candidates.Count > 1 ? ConvertToButtonViewModel(candidates[1]) : null;
            ThirdCandidateViewModel = candidates.Count > 2 ? ConvertToButtonViewModel(candidates[2]) : null;
        }

        private ButtonViewModel ConvertToButtonViewModel(ButtonState buttonState)
        {
            return new ButtonViewModel
            {
                ButtonId = buttonState.ButtonId,
                Text = buttonState.ButtonId.ToString(),
                Background = GetButtonStateColor(buttonState.State),
                PressTime = buttonState.PressTime
            };
        }

        private Brush GetButtonStateColor(ButtonStateEnum state)
        {
            return state switch
            {
                ButtonStateEnum.Unpressed => Brushes.Gray,
                ButtonStateEnum.FalseStart => Brushes.Red,    // Фальстарт
                ButtonStateEnum.PressedEarly => Brushes.Yellow, // Нажата рано
                ButtonStateEnum.PressedOnTime => Brushes.Green,  // Нажата вовремя
                _ => Brushes.GhostWhite// Не нажата или неизвестное состояние
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
        
        private void UpdateStatus(PhaseEnum phase)
        {
            StatusText = phase switch
            {
                PhaseEnum.Idle => "Ожидание (все сброшено)",
                PhaseEnum.RegisterFalseStarts => "Регистрация фальш стартов",
                PhaseEnum.RegisterEarly => "Регистрация ранних нажатий",
                PhaseEnum.RegisterOther => "Регистрация нажатий",
                PhaseEnum.Stopped => "Остановлено",
                // ... и так далее для всех случаев
                _ => "Неизвестная фаза"
            };
        }

        private int seconds;

        public int Seconds { get => seconds; set => Set(ref seconds, value); }

        
        private void PerformSecondsDecrease(object commandParameter)
        {
            Seconds--;
        }
                
        private void PerformSecondIncrease(object commandParameter)
        {
            Seconds++;
        }       

        private void AddTestData(object commandParameter)
        {
            ReceivedData = "!471429C40:3322EC:000000:000000:100349:32169C:000000:000000:2103B9";
            ParseAndDisplayData(ReceivedData); 
        }
    }
}