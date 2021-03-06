﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphLayoutSample.Engine.Helpers;
using GraphLayoutSample.Engine.Layout;
using GraphLayoutSample.Engine.Models;
using GraphLayoutSample.Engine.Utils;

namespace GraphLayoutSample.Desktop.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RandomGraphSettings _settings = new RandomGraphSettings
        {
            MinNodeDegree = 1,
            MaxNodeDegree = 1,
            NodeCount = 10,
            LayerCount = 5
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clean() => GraphCanvas.Children.Clear();

        private void Draw(IReadOnlyCollection<Node> graph)
        {
            Clean();

            foreach (var node in graph)
            {
                var rectangle = new Rectangle
                {
                    Width = node.Width,
                    Height = node.Height,
                    Stroke = Brushes.LightBlue,
                    StrokeThickness = 2,
                    Tag = node
                };

                Canvas.SetLeft(rectangle, node.Position.X);
                Canvas.SetTop(rectangle, node.Position.Y);

                GraphCanvas.Children.Add(rectangle);
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
            var line = new Line
            {
                Stroke = Brushes.LightSteelBlue,
                StrokeThickness = 2,
                X1 = firstNode.Position.X + firstNode.Width,
                X2 = secondNode.Position.X,
                Y1 = firstNode.Position.Y + firstNode.Height / 2,
                Y2 = secondNode.Position.Y + secondNode.Height / 2
            };


            GraphCanvas.Children.Add(line);
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var graph = GraphHelper.GenerateRandomGraph(_settings);

            var layoutBuilder = new OptimalLayoutBuilder();
            var newSize = layoutBuilder.SetPositions(graph,
                new RectangleSize(GraphCanvas.ActualWidth, GraphCanvas.ActualHeight));
            GraphCanvas.Width = newSize.Width;
            GraphCanvas.Height = newSize.Heigth;

            Draw(graph);
        }

        private void MainWindow_OnClosed(object sender, EventArgs e) => Application.Current.Shutdown();
    }
}
