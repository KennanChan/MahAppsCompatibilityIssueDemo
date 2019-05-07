using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using SDK;

namespace MahAppsCompatibilityIssue
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RunPlugins();
        }

        private void RunPlugins()
        {
            if (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) is string dir)
            {
                var pluginInterface = typeof(IPlugin);
                new DirectoryInfo(Path.Combine(dir, "plugins"))
                    .EnumerateFiles("*.plugin", SearchOption.AllDirectories)
                    .OrderBy(pluginFile => pluginFile.Name)
                    .SelectMany(pluginFile =>
                    {
                        if (!(pluginFile.DirectoryName is string pluginDir)) return new Type[] { };
                        return Assembly
                            .LoadFrom(Path.Combine(pluginDir, File.ReadAllText(pluginFile.FullName)))
                            .GetTypes()
                            .Where(type => !type.IsAbstract)
                            .Where(type => !type.IsInterface)
                            .Where(type => pluginInterface.IsAssignableFrom(type));
                    })
                    .Select(Activator.CreateInstance)
                    .Cast<IPlugin>()
                    .ToList()
                    .ForEach(plugin => plugin.Run());
            }
        }
    }
}