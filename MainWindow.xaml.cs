using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using PointX = System.Windows.Point;

namespace FrontAngryBirds
{
  public partial class MainWindow : Window
    {
        public double weight;
        static double FPS = 60;
        double period = 1 / FPS;
        double initSpeed;
        public double angle;
        public Motion m;
        public PointX startPosition = new PointX(150, 570);
        DispatcherTimer tmr;

        double stretchOfAxis = 3;
        public MainWindow()
        {
            InitializeComponent();

            Canvas.SetLeft(Bird, startPosition.X);
            Canvas.SetTop(Bird, startPosition.Y);
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            initSpeed = Convert.ToDouble(Velocity.Text.ToString());
            angle = Convert.ToDouble(Angle.Text.ToString());
            weight = Convert.ToDouble(Weight.Text.ToString());

            ProjectileFlight pf = new ProjectileFlight(initSpeed, angle, weight);

            GetPosition getP = new GetPosition(pf.GetPosition);

            m = new Motion(period);

            m.Calculate(getP);

            tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(0.5);
            tmr.Tick += new EventHandler(TimerOnTick);
            tmr.Start();
        }

        int i = 0;
        private void Retry(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Bird, startPosition.X);
            Canvas.SetTop(Bird, startPosition.Y);
            tmr.Stop();

            i = 0;
        }  
        private void TimerOnTick(object sender, EventArgs e)
        {
            if (i < m.Points.Count)
            {
                Canvas.SetLeft(Bird, stretchOfAxis * m.Points[i].X + startPosition.X);
                Canvas.SetTop(Bird, -stretchOfAxis * m.Points[i].Y + startPosition.Y);
            }
            else tmr.Stop();

            i++;
        }
    }

}
