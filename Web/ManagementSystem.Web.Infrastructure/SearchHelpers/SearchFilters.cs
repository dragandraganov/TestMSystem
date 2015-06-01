using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Web.Infrastructure.SearchHelpers
{
    public class SearchFilters<T> where T : class
    {
        public static IEnumerable<Expression<Func<T, bool>>> GetFilters(SearchCriterion criterion) 
        {
            var filters = new List<Expression<Func<T, bool>>>();

            foreach (var property in criterion.GetType().GetProperties())
            {
                if (property.PropertyType==typeof(String))
                {
                    if (!String.IsNullOrWhiteSpace(property.Name))
                    {
                        //filters.Add(t => t.GetType().GetProperty(property.Name));
                    }
                }
            }

            //if (!string.IsNullOrWhiteSpace(criterion.Title))
            //    filters.Add(b => b.Title.Contains(criterion.Title));

            //if (!string.IsNullOrWhiteSpace(criterion.Author))
            //    filters.Add(b => b.Author.ToLower().Contains(criterion.Author.ToLower().Trim()));

            //if (criterion.StartYear != null)
            //    filters.Add(b => b.Published >= criterion.StartYear);

            //if (criterion.EndYear != null)
            //    filters.Add(b => b.Published <= criterion.EndYear);

            //if (!string.IsNullOrWhiteSpace(criterion.ISBN))
            //    filters.Add(b => b.ISBN.Equals(criterion.ISBN));

            return filters;
        }
    }
}
