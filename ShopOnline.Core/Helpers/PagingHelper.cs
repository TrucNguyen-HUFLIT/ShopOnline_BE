using ShopOnline.Core.Models.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Core.Helpers
{
    public static class PagingHelper
    {
        public static async Task<IPagedList<ProductInforModel>> ClientProductsPagingAsync(bool sortIncrease, int? page, List<ProductInforModel> productInforModels)
        {
            if (sortIncrease)
                productInforModels = productInforModels.OrderBy(x => x.PriceVND).ToList();
            else
                productInforModels = productInforModels.OrderByDescending(x => x.PriceVND).ToList();

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            return await productInforModels.ToPagedListAsync(pageNumber, pageSize);
        }
    }
}
