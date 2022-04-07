using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Logic.Staff
{
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly MyDbContext _context;
        public CustomerBusiness(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<CustomerInfor>> GetListCustomerAsync(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var listCustomer = new List<CustomerInfor>();
            var customers = await _context.Customers.Where(x => !x.IsDeleted).ToListAsync();
            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    var customerInforForList = new CustomerInfor
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        FullName = customer.FullName,
                        Avatar = customer.Avatar,
                        Address = customer.Address,
                        PhoneNumber = customer.PhoneNumber
                    };
                    listCustomer.Add(customerInforForList);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listCustomer = listCustomer.Where(s => s.FullName.ToLower().Contains(searchString.ToLower())
                                            || s.Email.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listCustomer = sortOrder switch
                {
                    "name_desc" => listCustomer.OrderByDescending(x => x.FullName).ToList(),
                    "name" => listCustomer.OrderBy(x => x.FullName).ToList(),
                    "id_desc" => listCustomer.OrderByDescending(x => x.Id).ToList(),
                    _ => listCustomer.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listCustomer.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }
    }
}
