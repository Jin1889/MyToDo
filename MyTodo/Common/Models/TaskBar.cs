using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Common.Models
{
    public class TaskBar : BindableBase
    {
        //图标
        private string icon;
        //标题
        private string title;
        //内容
        private string content;
        //颜色
        private string color;
        //目标
        private string target;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

    }
}
