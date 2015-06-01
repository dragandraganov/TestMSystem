using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Web.Infrastructure.SearchHelpers
{
    public class SearchCriterion
    {
        public SearchCriterion(NameValueCollection queryString)
        {
            dynamic dynObject = new ExpandoObject();
        }
    }
}
