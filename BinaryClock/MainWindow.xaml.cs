using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BinaryClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer m_renderTimer;
        private AutoResetEvent m_renderSignal = new AutoResetEvent(false);
        private List<Circle> _Circles = new List<Circle>();
        private List<string> _ClockValues = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            GridSetup();

            m_renderTimer = new System.Timers.Timer(500);
            m_renderTimer.Elapsed += (_, __) => m_renderSignal.Set();
            m_renderTimer.Enabled = true;
            Task.Factory.StartNew(() => RenderLoop(), TaskCreationOptions.LongRunning);

        }

        private void GridSetup()
        {
            for (int y = 0; y <= 5; y++) {
                for (int i = 0; i <= 3; i++)
                {
                    Circle C = new Circle();
                    _Circles.Add(C);
                    Canvas1.Children.Add(C);
                    Canvas.SetLeft(C, 50 + (y * 30));
                    Canvas.SetTop(C, 50+ (i * 30));
                } 
            }

            update();

        }

        private void RenderLoop()
        {
            while (true)
            {
                m_renderSignal.WaitOne();
                Dispatcher.BeginInvoke((Action)update, DispatcherPriority.Send, null);
            }
        }

        private void update()
        {         
           
           _ClockValues.Clear();
           string[] Values = DateTime.Now.ToString("hhmmss").Select(c => c.ToString()).ToArray();
           lblBinaryValue.Content = DateTime.Now.ToString("hh:mm:ss");
           foreach (string s in Values) {_ClockValues.Add(Convert.ToString(Convert.ToInt16(s), 2).PadLeft(4, '0'));}
           for (int i = 0; i <= 5; i++) {updateClock(_ClockValues[i], i);}

        }


        private void updateClock(string value, int column) 
        { 
  
         int i = 0; 
           foreach (char s in value)
           {
               if (s.ToString() == "1") { _Circles[(column * 4) + i].toggle(true); }
               else if (s.ToString() == "0"){ _Circles[(column * 4) + i].toggle(false); }
               i++;
           }

        }

    }
}
