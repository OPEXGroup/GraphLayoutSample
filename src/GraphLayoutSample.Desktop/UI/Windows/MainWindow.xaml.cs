using GraphLayoutSample.Engine.Utils;
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
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Layout;
using GraphLayoutSample.Engine.Models;

namespace GraphLayoutSample.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RandomGraphSettings _settings = new RandomGraphSettings();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clean() => GraphGrid.Children.Clear();

        private void Draw(List<Node> graph)
        {
            Clean();

        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var graph = GraphHelper.GenerateRandomGraph(_settings);

            var layoutBuilder = new RandomLayoutBuilder(DrawGrid.Width, DrawGrid.Height, DrawGrid.Margin.Left);
            DrawGrid.Width = layoutBuilder.SetPositions(graph);

            Draw(graph);
        }
    }
}
