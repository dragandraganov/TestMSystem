using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementSystem.Web.ViewModels
{
    public class ComplexViewModel
    {
        public TaskViewModel TaskViewModel { get; set; }

        public ICollection<UserViewModel> AllUsers { get; set; }

        public string[] SelectedUsersId { get; set; }
    }
}