using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ManagementSystem.Data.Models
{
    public class Comment : IModel
    {
        public Comment()
        {
            this.DateAdded = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public string Content { get; set; }

        public CommentType Type { get; set; }

        public DateTime? ReminderDate { get; set; }

        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}
