using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiveRpi.ViewModels;

namespace LiveRpi
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Closed += (s, e) => ((MainViewModel)DataContext).Dispose();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
