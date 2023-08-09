using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
    /// <summary>
    /// 待办数据实体
    /// </summary>
    public class ToDoDto : BaseDto
    {
        private string title;
        private string content;
        private int status;
        public string Title
        {
            get { return title; }
            set { title = value; OnPerpertyChanged(); }
        }
        public string Content
        {
            get { return content; }
            set { content = value; OnPerpertyChanged(); }
        }
        public int Status
        {
            get { return status; }
            set { status = value; OnPerpertyChanged(); }
        }
    }
}
