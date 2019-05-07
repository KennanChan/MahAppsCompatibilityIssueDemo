using System;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using SDK;

namespace PluginA
{
    public class PluginEntry : IPlugin
    {
        public PluginEntry()
        {
            Window = new MetroWindow {Title = "PluginA", Background = Brushes.White};
        }

        private MetroWindow Window { get; }

        public void Run()
        {
            try
            {
                Window.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "PluginA");
            }
        }
    }
}