using AutoMapper;
using ManagementSystem.Data.Models;
using ManagementSystem.Web.Infrastructure.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ManagementSystem.Web.ViewModels
{
    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {

        public CommentViewModel()
        {
        }

        public CommentViewModel(int taskId)
        {
            this.TaskId = taskId;
        }

        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "The comment text must be at least 3 characters")]
        [Required(ErrorMessage = "The comment text must be at least 3 characters")]
        [AllowHtml]
        public string Content { get; set; }

        public int TaskId { get; set; }

        public int AuthorId { get; set; }

        public string AuthorName { get; set; }

        public CommentType Type { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? ReminderDate { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
               .ForMember(cm => cm.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
               .ReverseMap();
        }

    }
}