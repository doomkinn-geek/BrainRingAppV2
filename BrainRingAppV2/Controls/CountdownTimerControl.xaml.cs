using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        private const double radius = 100;
        private Point center = new Point(100, 100);
        private SoundPlayer tickPlayer = new SoundPlayer("tick.wav"); // Путь к файлу tick.wav
        private SoundPlayer alarmPlayer = new SoundPlayer("alarm.wav"); // Путь к файлу alarm.wav

        public CountdownTimerControl()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            remainingSeconds = 0;
            //timer.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawDial();
            UpdateDisplay();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                if (remainingSeconds <= 5 && remainingSeconds > 0)
                {
                    try { tickPlayer.Play(); }// Воспроизведение звука каждую секунду последние 5 секунд
                    catch { }
                }
                UpdateDisplay();
            }
            else
            {
                timer.Stop();
                try { alarmPlayer.Play(); }// Воспроизведение звука, когда время закончилось
                catch { }
            }
        }        
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
            control.remainingSeconds = (int)e.NewValue;            
            control.UpdateDisplay();
        }


        private void DrawDial()
        {
            for (int i = 0; i < 60; i++)
            {
                double angle = i * Math.PI / 30;
                bool isMajorTick = i % 5 == 0;

                Line tick = new Line
                {
                    X1 = center.X + radius * Math.Cos(angle),
                    Y1 = center.Y + radius * Math.Sin(angle),
                    X2 = center.X + (radius - (isMajorTick ? 10 : 5)) * Math.Cos(angle),
                    Y2 = center.Y + (radius - (isMajorTick ? 10 : 5)) * Math.Sin(angle),
                    Stroke = Brushes.Black,
                    StrokeThickness = 2                    
                };
                canvas.Children.Add(tick);

                if (isMajorTick)
                {
                    TextBlock text = new TextBlock
                    {
                        Text = $"{(i * 5) % 60}",
                        FontSize = 12,
                        FontWeight = FontWeights.Bold
                    };
                    double textAngle = (i * 5 % 60) * Math.PI / 30 - Math.PI / 2;
                    text.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    Size textSize = text.DesiredSize;
                    Canvas.SetLeft(text, center.X + (radius - 25) * Math.Cos(textAngle) - textSize.Width / 2);
                    Canvas.SetTop(text, center.Y + (radius - 25) * Math.Sin(textAngle) - textSize.Height / 2);
                    canvas.Children.Add(text);
                }
            }
        }

        private void UpdateDisplay()
        {
            double angle = (60 - remainingSeconds) * Math.PI / 30;
            UpdateSecondHand(angle);
            UpdateHighlightPath(angle);
            txtCountdown.Text = $"{remainingSeconds:D2}";
        }


        private void UpdateSecondHand(double angle)
        {
            // Угол для стрелки, где 0 секунд - верхняя точка
            angle = -Math.PI / 2 + remainingSeconds * Math.PI / 30;

            var handGeometry = new StreamGeometry();
            using (var ctx = handGeometry.Open())
            {
                ctx.BeginFigure(new Point(center.X, center.Y), true, true);
                ctx.LineTo(new Point(center.X + 5 * Math.Sin(angle), center.Y - 5 * Math.Cos(angle)), true, false);
                ctx.LineTo(new Point(center.X + radius * 0.8 * Math.Cos(angle), center.Y + radius * 0.8 * Math.Sin(angle)), true, false);
                ctx.LineTo(new Point(center.X - 5 * Math.Sin(angle), center.Y + 5 * Math.Cos(angle)), true, false);
                ctx.LineTo(new Point(center.X, center.Y), true, false);
            }
            secondHand.Data = handGeometry;
        }

        private void UpdateHighlightPath(double angle)
        {
            // Создаем фигуру для подсветки
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = center; // Начало в центре

            // Первая точка на окружности - верхняя часть циферблата
            Point startPoint = new Point(center.X, center.Y - radius);
            pathFigure.Segments.Add(new LineSegment(startPoint, true));

            // Рассчитываем угол окончания подсвеченной области
            angle = -Math.PI / 2 + remainingSeconds * Math.PI / 30; // Изменено это место

            // Дуга сегмента
            bool isLargeArc = remainingSeconds > 30; // Изменено условие
            Point endPoint = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
            ArcSegment arcSegment = new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = isLargeArc
            };
            pathFigure.Segments.Add(arcSegment);

            // Замыкаем фигуру линией к центру
            pathFigure.Segments.Add(new LineSegment(center, true));

            // Создаем геометрию и применяем к пути
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            highlightPath.Data = pathGeometry;
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
