using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BrainRingAppV2.Controls
{
    /// <summary>
    /// Логика взаимодействия для CountdownTimerControl.xaml
    /// </summary>
    public partial class CountdownTimerControl : UserControl
    {
        private DispatcherTimer timer;
        private int remainingSeconds;

        public CountdownTimerControl()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            remainingSeconds = 30;
            //timer.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawCountdown();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                DrawCountdown();
            }
            else
            {
                timer.Stop();
            }
        }

        /*public int RemainingSeconds
        {
            get => remainingSeconds;
            set
            {
                remainingSeconds = value;
                DrawCountdown();
            }
        }*/
        public static readonly DependencyProperty RemainingSecondsProperty = DependencyProperty.Register(
            "RemainingSeconds",
            typeof(int),
            typeof(CountdownTimerControl),
            new PropertyMetadata(0, OnRemainingSecondsChanged));

        public int RemainingSeconds
        {
            get { return (int)GetValue(RemainingSecondsProperty); }
            set 
            {
                remainingSeconds = value;
                SetValue(RemainingSecondsProperty, value); 
            }
        }

        private static void OnRemainingSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CountdownTimerControl)d;
            int v = Convert.ToInt32(e.NewValue.ToString());
            control.RemainingSeconds = v;
            // Обновите здесь логику, если требуется перерисовка или обновление
            control.DrawCountdown();
        }


        private void DrawCountdown()
        {
            canvas.Children.Clear();

            double radius = 100;
            Point center = new Point(canvas.Width / 2, canvas.Height / 2);

            // Рисуем круг секундомера
            Ellipse circle = new Ellipse
            {
                Stroke = Brushes.Black,
                Width = radius * 2,
                Height = radius * 2
            };
            Canvas.SetLeft(circle, center.X - radius);
            Canvas.SetTop(circle, center.Y - radius);
            canvas.Children.Add(circle);

            // Рисуем деления
            for (int i = 0; i < 60; i++)
            {
                Line line = new Line
                {
                    Stroke = Brushes.Black,
                    X1 = center.X + radius * Math.Cos(i * Math.PI / 30),
                    Y1 = center.Y + radius * Math.Sin(i * Math.PI / 30),
                    X2 = center.X + (radius - (i % 5 == 0 ? 10 : 5)) * Math.Cos(i * Math.PI / 30),
                    Y2 = center.Y + (radius - (i % 5 == 0 ? 10 : 5)) * Math.Sin(i * Math.PI / 30)
                };
                canvas.Children.Add(line);
            }

            // Рисуем стрелку секундомера
            if (remainingSeconds > 0)
            {
                Line hand = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 2,
                    X1 = center.X,
                    Y1 = center.Y,
                    X2 = center.X + radius * 0.8 * Math.Cos((remainingSeconds - 15) * Math.PI / 30),
                    Y2 = center.Y + radius * 0.8 * Math.Sin((remainingSeconds - 15) * Math.PI / 30)
                };
                canvas.Children.Add(hand);
            }

            // Обновление текста секундомера
            txtCountdown.Text = $"{remainingSeconds} сек";
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
