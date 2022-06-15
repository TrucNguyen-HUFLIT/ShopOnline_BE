using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Validators.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Staff
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly MyDbContext _context;
        public CustomerBusiness(MyDbContext context)
        {
            _context = context;
        }

        public async Task<PagedCollectionResultModel<CustomerInfor>> GetListCustomerAsync(CustomerParamsModel model)
        {
            var customersQuery = _context.Customers.Where(x => !x.IsDeleted);

            if(!string.IsNullOrWhiteSpace(model.Terms))
            {
                var termsNormalize = model.Terms.Trim().ToUpperInvariant();
                customersQuery = customersQuery.Where(x => x.Id.ToString() == termsNormalize
                                            || x.FullName.Contains(termsNormalize)
                                            || x.Email.Contains(termsNormalize));
            }   

            switch (model.SortBy)
            {
                case CustomerSortByEnum.Name:
                    customersQuery = model.IsDescending
                         ? customersQuery.OrderByDescending(x => x.FullName)
                         : customersQuery.OrderBy(x => x.FullName);
                    break;
                case CustomerSortByEnum.Id:
                    customersQuery = model.IsDescending
                         ? customersQuery.OrderByDescending(x => x.Id)
                         : customersQuery.OrderBy(x => x.Id);
                    break;
                default:
                    customersQuery = model.IsDescending
                         ? customersQuery.OrderByDescending(x => x.Id)
                         : customersQuery.OrderBy(x => x.Id);
                    break;
            }

            var totalRecord = customersQuery.Count();

            var customers = await customersQuery.Select(x => new CustomerInfor
            {
                Id= x.Id,
                Address = x.Address,
                Email = x.Email,
                Avatar = x.Avatar,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
            }).Skip(model.Skip).Take(model.Take).ToListAsync();

            return new PagedCollectionResultModel<CustomerInfor>
            {
                Skip = model.Skip,
                Take = model.Take,
                Total = totalRecord,
                Result = customers,
                Terms = model.Terms
            };
        }
    }
}
