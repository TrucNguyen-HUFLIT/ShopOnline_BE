using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Core.Validators.Paging
{
    public class PagedCollectionResultModel<T> : PagedCollectionParametersModel
    {
        public PagedCollectionResultModel()
        {

        }

        public PagedCollectionResultModel(PagedCollectionParametersModel baseCollectionParametersModel)
        {
            Skip = baseCollectionParametersModel.Skip;
            Take = baseCollectionParametersModel.Take;
            Terms = baseCollectionParametersModel.Terms;
        }
        public long Total { get; set; }

        public IEnumerable<T> Result { get; set; } = new List<T>();
    }
}
