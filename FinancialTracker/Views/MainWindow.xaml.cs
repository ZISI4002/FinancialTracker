using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinancialTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _hideButtonsTimer;
        private StackPanel _currentButtonsPanel;

        public MainWindow()
        {
            InitializeComponent();

            _hideButtonsTimer = new DispatcherTimer();
            _hideButtonsTimer.Interval = TimeSpan.FromSeconds(7);
            _hideButtonsTimer.Tick += HideButtonsTimer_Tick;

            MyListView.PreviewMouseLeftButtonUp += ListView_PreviewMouseLeftButtonUp;
        }

        private void ListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = GetAncestorOfType<ListViewItem>((DependencyObject)e.OriginalSource);
            if (clickedItem == null) return;

            // Найдем ButtonsPanel внутри строки
            var buttonsPanel = FindVisualChildByName<StackPanel>(clickedItem, "ButtonsPanel");
            if (buttonsPanel == null) return;

            if (_currentButtonsPanel != null && _currentButtonsPanel != buttonsPanel)
            {
                // Скрыть предыдущие кнопки
                _currentButtonsPanel.Visibility = Visibility.Collapsed;
            }

            // Показываем текущие кнопки
            buttonsPanel.Visibility = Visibility.Visible;
            _currentButtonsPanel = buttonsPanel;

            // Запускаем/сбрасываем таймер
            _hideButtonsTimer.Stop();
            _hideButtonsTimer.Start();
        }

        private void HideButtonsTimer_Tick(object? sender, EventArgs e)
        {
            if (_currentButtonsPanel != null)
            {
                _currentButtonsPanel.Visibility = Visibility.Collapsed;
                _currentButtonsPanel = null;
            }
            _hideButtonsTimer.Stop();
        }

        // Вспомогательные методы для поиска в визуальном дереве:
        private static T? GetAncestorOfType<T>(DependencyObject? element) where T : DependencyObject
        {
            while (element != null && !(element is T))
            {
                element = VisualTreeHelper.GetParent(element);
            }
            return element as T;
        }

        private static T? FindVisualChildByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild && tChild.Name == name)
                    return tChild;

                var result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }

}