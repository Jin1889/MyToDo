using MyTodo.Extensions;
using MyTodo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace MyTodo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.loginService = loginService;
            this.aggregator = aggregator;
        }

        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": LoginAsync(); break;
                case "LoginOut": LoginOut(); break;
            }
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName) ||
                string.IsNullOrWhiteSpace(PassWord))
            {
                return;
            }
            var loginResult = await loginService.Login(new MyToDo.Shared.Dtos.UserDto()
            {
                Account = UserName,
                PassWord = PassWord
            });

            if (loginResult != null && loginResult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                //登录失败提示...
                RequestClose?.Invoke(new DialogResult(ButtonResult.No));
            }
        }

        private void LoginOut()
        {
            throw new NotImplementedException();
        }
    }
}
