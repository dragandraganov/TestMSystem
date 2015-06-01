using AutoMapper;
using ManagementSystem.Data.Models;
using System;
using System.Linq;
using ManagementSystem.Web.Infrastructure.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Web.ViewModels
{
    public class TaskViewModel : IMapFrom<Task>, IHaveCustomMappings, IValidatableObject
    {

        public int Id { get; set; }

        public DateTime CreatedOnDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequiredByDate { get; set; }

        public DateTime? NextActionDate { get; set; }

        [Required]
        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        public TaskType Type { get; set; }

        public IList<string> AssignedToUsers { get; set; }

        public IList<CommentViewModel> Comments { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Task, TaskViewModel>()
                .ForMember(tm => tm.AssignedToUsers, opt => opt.MapFrom(t => t.AssignedToUsers.Select(u => u.UserName)))
                .ReverseMap();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.RequiredByDate < DateTime.Now)
            {

                yield return new ValidationResult("Required By Date can not be in the past.", new[] { "RequiredByDate" });

            }

            if (this.NextActionDate != null)
            {
                if (this.NextActionDate < DateTime.Now || this.NextActionDate > RequiredByDate)
                {
                    yield return new ValidationResult("Next Action Date must be between current and Required By Date.", new[] { "NextActionDate" });
                }
            }
        }
    }
}