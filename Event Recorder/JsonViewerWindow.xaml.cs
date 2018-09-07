using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml;

namespace Event_Recorder
{
    /// <summary>
    /// Interaction logic for JsonViewerWindow.xaml
    /// </summary>
    public partial class JsonViewerWindow : Window
    {
        public JsonViewerWindow(string json, string title)
        {
            IHighlightingDefinition customHighlighting;
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("Event_Recorder.JsonSyntaxHighlight.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }

            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("JSON", new string[] { ".json" }, customHighlighting);
            
            InitializeComponent();

            Viewer.Text = json;
            this.Title = title;
        }
    }
}
