using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Common.Models
{
    /// <summary>
    /// 系统菜单导航实体类
    /// </summary>
    public class MenuBar : BindableBase
    {
        //菜单图标
        private string icon;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        //菜单名称
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        //菜单命名空间
        private string nameSpace;

        public string NameSpace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }
    }
}
