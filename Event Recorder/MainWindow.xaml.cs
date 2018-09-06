using LCU.NET;
using LCU.NET.WAMP;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        
        public bool Attach
        {
            get { return (bool)GetValue(AttachProperty); }
            set { SetValue(AttachProperty, value); }
        }
        public static readonly DependencyProperty AttachProperty = DependencyProperty.Register("Attach", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public ObservableCollection<EventData> Events { get; set; } = new ObservableCollection<EventData>();

        private DateTime StartTime;
        
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            if (!LeagueClient.Default.SmartInit())
            {
                MessageBox.Show("Make sure the LoL client is open!", "Dude", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.Close();
            }

            this.StartTime = DateTime.Now;

            LeagueSocket.SubscribeRaw(Handler);
        }

        private void Handler(JsonApiEvent obj)
        {
            Dispatcher.Invoke(() =>
            {
                if (!Attach)
                    return;

                Events.Add(new EventData(this.StartTime, obj));

                if (ScrollToBottom)
                    EventsList.ScrollToBottom();
            });
        }
        
        private void CopyURI(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)EventsList.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.URI);
        }

        private void CopyData(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)EventsList.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.Data.ToString(Formatting.None));
        }

        private void CopyDataIndented(object sender, RoutedEventArgs e)
        {
            var ev = (EventData)EventsList.SelectedItem;
            Clipboard.SetText(ev.JsonEvent.Data.ToString());
        }

        private void Events_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            bool should = EventsList.ShouldScrollToBottom();

            if ((!should || ScrollToBottom) && (!ScrollToBottom || should))
                ScrollToBottom = should;
        }
        
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Events.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var diag = new VistaSaveFileDialog
            {
                Title = "Choose where to save to log file",
                Filter = "JSON file (*.json)|*.json|All files (*.*)|*.*",
                AddExtension = true
            };

            if (diag.ShowDialog(this) == true)
            {
                File.WriteAllText(diag.FileName, JsonConvert.SerializeObject(Events));
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var diag = new VistaOpenFileDialog
            {
                Title = "Choose where the recorded log file",
                Filter = "JSON file (*.json)|*.json|All files (*.*)|*.*",
                CheckFileExists = true
            };

            if (diag.ShowDialog(this) == true)
            {
                Attach = false;
                ScrollToBottom = false;

                var newEvents = JsonConvert.DeserializeObject<EventData[]>(File.ReadAllText(diag.FileName));

                Events.Clear();

                foreach (var item in newEvents)
                {
                    Events.Add(item);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in EventsList.SelectedItems.Cast<EventData>().ToArray())
            {
                Events.Remove(item);
            }
        }
    }
}
