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
        private readonly RandomGraphSettings _settings = new RandomGraphSettings();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clean() => GraphCanvas.Children.Clear();

        private void Draw(List<Node> graph)
        {
            Clean();

            foreach (var node in graph)
            {
                var rectange = new Rectangle
                {
                    Width = node.Width,
                    Height = node.Height,
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2,
                    Tag = node
                };

                Canvas.SetLeft(rectange, node.Position.X);
                Canvas.SetTop(rectange, node.Position.Y);

                GraphCanvas.Children.Add(rectange);
            }

            foreach (var node in graph)
            {
                SetConnectionsForNode(node);
            }
        }

        private void SetConnectionsForNode(Node node)
        {
            foreach (var nextNode in node.NextNodes)
            {
                ConnectNodes(node, nextNode);
            }
        }

        private void ConnectNodes(Node firstNode, Node secondNode)
        {
            var firstNodeRectangle = GetNodeRectange(firstNode);
            var secondNodeRectangle = GetNodeRectange(secondNode);
        }

        private Rectangle GetNodeRectange(Node node)=> GraphCanvas.Children.OfType<Rectangle>().FirstOrDefault(rectangle => rectangle.Tag == node);

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var graph = GraphHelper.GenerateRandomGraph(_settings);

            var layoutBuilder = new RandomLayoutBuilder(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight, 10);
            GraphCanvas.Width = layoutBuilder.SetPositions(graph);

            Draw(graph);
        }
    }
}
