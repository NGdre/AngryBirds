using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Microsoft.Win32;

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
        public string fileName;
        public string fileMode = "default";

        double stretchOfAxis = 3;
        public MainWindow()
        {
            InitializeComponent();

            Canvas.SetLeft(Bird, startPosition.X);
            Canvas.SetTop(Bird, startPosition.Y);
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            if (!TextBoxesAreValid()) return;
            
            ProjectileFlight pf = new ProjectileFlight(initSpeed, angle, weight);

            GetPosition getP = new GetPosition(pf.GetPosition);

            m = new Motion(period);

            if ((fileMode == "write" || fileMode == "read") && fileName == null)
            {
                MessageBox.Show("сначала нужно выбрать файл!");
                return;
            }

            if (fileMode != "read")
                m.Calculate(getP);

            if (fileMode == "write") m.Write(fileName);

            if (fileMode == "read")
                m.Read(fileName);

            tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(0.5);
            tmr.Tick += new EventHandler(TimerOnTick);
            tmr.Start();
            
        }

        private bool TextBoxesAreValid()
        {
            double num;

            if (double.TryParse(Velocity.Text.ToString(), out num) && num > 0)
            {
                initSpeed = num;
            }
            else
            {
                MessageBox.Show("Вы должны ввести положительное число");
                return false;
            }

            if (double.TryParse(Angle.Text.ToString(), out num) && num > 0 && num < 90)
            {
                angle = num;
            }
            else
            {
                MessageBox.Show("Угол должен должен быть меньше 90 градусов и больше 0");
                return false;
            }

            if (double.TryParse(Weight.Text.ToString(), out num) && num > 0)
            {
                weight = num;
            }
            else
            {
                MessageBox.Show("Вы должны ввести положительное число");
                return false;
            }

            return true;
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

        private void PickFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                FileName.Content = "Текущий путь файла: " + fileName;
            }
        }

        private void PickFileMode(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized) return;

            if (e.OriginalSource is RadioButton radioButton)
            {
                MessageBox.Show("выбран режим: " + radioButton.Content, Title);

                if (radioButton.Name == "defaultRB")
                    fileMode = "default";
                else if (radioButton.Name == "writeRB")
                    fileMode = "write";
                else 
                    fileMode = "read";
            }
        }
    }

}
