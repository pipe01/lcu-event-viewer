using LCU.NET;
using LCU.NET.WAMP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Event_Recorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ScrollToBottom
        {
            get { return (bool)GetValue(ScrollToBottomProperty); }
            set { SetValue(ScrollToBottomProperty, value); }
        }
        public static readonly DependencyProperty ScrollToBottomProperty = DependencyProperty.Register("ScrollToBottom", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        private DateTime StartTime;
        
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.StartTime = DateTime.Now;
            LeagueClient.Default.BeginTryInit();

            LeagueSocket.SubscribeRaw(Handler);
        }

        private void Handler(JsonApiEvent obj)
        {
            Dispatcher.Invoke(() =>
            {
                bool scroll = Events.ShouldScrollToBottom();
                
                Events.Items.Add(new EventData(this.StartTime, obj));

                if (ScrollToBottom)
                    Events.ScrollToBottom();
            });
        }
        
        private void CopyURI(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)Events.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.URI);
        }

        private void CopyData(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)Events.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.Data.ToString(Formatting.None));
        }

        private void CopyDataIndented(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)Events.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.Data.ToString());
        }

        private void Events_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollToBottom = Events.ShouldScrollToBottom();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Events.Items.Clear();
        }
    }
}
