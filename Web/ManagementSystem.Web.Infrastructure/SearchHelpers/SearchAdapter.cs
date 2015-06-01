using ManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Web.Infrastructure.SearchHelpers
{
    public class SearchAdapter<T> where T :class
    {
        private IManagementSystemData data;
        private NameValueCollection queryString;

        public SearchAdapter(IManagementSystemData data, NameValueCollection queryString)
        {
            this.data = data;
            this.queryString = queryString;

        }

        public Task<ICollection<T>> PerformSearch(SearchCriterion criterion)
        {
            var list = GetSearchViewFromDatabase(criterion);
            return Task.Factory.StartNew(() => AssembleSearchResult(list));
        }

        private ICollection<T> AssembleSearchResult(IEnumerable<T> entityCollection)
            
        {
            var searchResult = new List<T>();
            if (entityCollection == null) return searchResult;
            foreach (var entity in entityCollection)
            {
                searchResult.Add(entity);
                
            }

            return searchResult;
        }

        private IEnumerable<T> GetSearchViewFromDatabase(SearchCriterion criterion)
        {
            var filters = new List<Expression<Func<T, bool>>>();

            filters.AddRange(SearchFilters<T>.GetFilters(criterion));

            var fullList = this.data.GetRepository<T>().All();

            if (filters.Count < 1) return fullList.ToList();

            foreach (var f in filters)
            {
                if (fullList != null)
                    fullList = fullList.Where(f);
            }

            return fullList != null ? fullList.ToList() : null;
        }
    }
}
