using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Service
{
    internal class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
        }

        public Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
