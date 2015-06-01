using ManagementSystem.Data.Models;
using System;
using System.Linq;
using ManagementSystem.Web.Infrastructure.Mapping;

namespace ManagementSystem.Web.ViewModels
{
    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}