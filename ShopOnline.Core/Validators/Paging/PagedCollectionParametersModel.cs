using ShopOnline.Core.Appsetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Core.Validators.Paging
{
    public class PagedCollectionParametersModel
    {
        public int Skip { get; set; } = AppConfigs.PagedCollectionParameters.Skip;

        public int Take { get; set; } = AppConfigs.PagedCollectionParameters.Take;

        public string Terms { get; set; } = AppConfigs.PagedCollectionParameters.Terms;
    }
}
