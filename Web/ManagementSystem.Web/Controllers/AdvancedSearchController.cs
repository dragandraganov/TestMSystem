using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystem.Data;
using System.Threading.Tasks;
using ManagementSystem.Web.Infrastructure.SearchHelpers;
using System.Linq.Expressions;

namespace ManagementSystem.Web.Controllers
{
    public class AdvancedSearchController<Tentity, Tmodel> 
    {
        //public AdvancedSearchController(ManagementSystemData data)
        //    : base(data)
        //{
        //}

        //public Task<ICollection<Tmodel>> PerformSearch(SearchCriterion criterion)
        //{
        //    var list = GetSearchViewFromDatabase(criterion);
        //    return Task.Factory.StartNew(() => AssembleSearchResult<Tentity, Tmodel>(list));
        //}

        //private ICollection<Tmodel> AssembleSearchResult<Tentity, Tmodel>(IEnumerable<Tentity> entityCollection)
        //    where Tmodel : new()
        //{
        //    var searchResult = new IList<Tmodel>();
        //    if (entityCollection == null) return searchResult;
        //    foreach (var entity in entityCollection)
        //    {
        //        //add reflection
        //        searchResult.Add(new Tmodel
        //        {
        //            //Author = entity.Author,
        //            //ISBN = entity.ISBN,
        //            //Published = entity.Published,
        //            //Title = entity.Title,
        //            //Section = entity.Section
        //        });
        //    }

        //    return searchResult;
        //}

        //private IEnumerable<Tentity> GetSearchViewFromDatabase(SearchCriterion criterion)
        //{
        //    var filters = new List<Expression<Func<Tentity, bool>>>();

        //    filters.AddRange(SearchFilters<Tentity>.GetFilters(criterion));

        //    var fullList = this.Data.GetRepository<Tentity>().All();

        //    if (filters.Count < 1) return fullList.ToList();

        //    foreach (var f in filters)
        //    {
        //        if (fullList != null)
        //            fullList = fullList.Where(f);
        //    }

        //    return fullList != null ? fullList.ToList() : null;
        //}
    }
}