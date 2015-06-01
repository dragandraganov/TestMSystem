using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ManagementSystem.Data.Models
{
    public class Task : IModel
    {
        public Task()
        {
            this.CreatedOnDate = DateTime.Now;
            this.AssignedToUsers = new HashSet<User>();
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public DateTime RequiredByDate { get; set; }

        public DateTime? NextActionDate { get; set; }

        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        public TaskType Type { get; set; }

        public virtual ICollection<User> AssignedToUsers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
