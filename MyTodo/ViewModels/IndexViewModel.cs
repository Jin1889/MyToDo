using MyTodo.Common;
using MyTodo.Service;
using MyTodo.Views;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialog;

        public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            CreateTaskBars();
            //TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute); 
            this.toDoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.dialog = dialog;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(); break;
                case "新增备忘录": AddMemo(); break;
            }
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #region 属性
        //任务栏
        private ObservableCollection<Common.Models.TaskBar> taskBars;

        public ObservableCollection<Common.Models.TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }
        //todo
        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        //备忘录
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        #endregion

        async Task AddToDo()
        {
            var dialogResult = await dialog.ShowDialog("AddToDoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                    if (todo.Id > 0)
                    {
                        
                    }
                    else
                    {
                        var addResult = await toDoService.AddAsync(todo);
                        if (addResult.Status)
                        {
                            ToDoDtos.Add(addResult.Result);
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        async void AddMemo()
        {
            var dialogResult = await dialog.ShowDialog("AddMemoView", null);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                    if (memo.Id > 0)
                    {

                    }
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            MemoDtos.Add(addResult.Result);
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<Common.Models.TaskBar>();
            TaskBars.Add(new Common.Models.TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "9", Color = "#FF0CA0FF", Target = "" });
            TaskBars.Add(new Common.Models.TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "9", Color = "#FF1ECA3A", Target = "" });
            TaskBars.Add(new Common.Models.TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "100%", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new Common.Models.TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "19", Color = "#FFFFA000", Target = "" });
        }
    }
}
