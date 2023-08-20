using System.Windows;
using System.Windows.Input;
using MyTodo.Common;
using MyTodo.Extensions;
using Prism.Events;

namespace MyTodo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;
        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();

            //注册等待消息窗口
            aggregator.Register(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;
                if (arg.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();
                }
            });

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (s, e) =>
            {
                var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close();
            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
            //改变菜单关闭左侧菜单栏
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
        }
    }
}
