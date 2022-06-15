using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Core.Validators.Paging
{
    public class PagedCollectionParametersModelValidator: AbstractValidator<PagedCollectionParametersModel>
    {
        public PagedCollectionParametersModelValidator()
        {
            RuleFor(reg => reg.Skip).GreaterThanOrEqualTo(0);
            RuleFor(reg => reg.Take).LessThanOrEqualTo(10000);
        }
    }
}
