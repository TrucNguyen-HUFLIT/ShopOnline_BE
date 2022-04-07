using Microsoft.EntityFrameworkCore;
using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Models.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Staff
{
    public class ReportBusiness : IReportBusiness
    {
        private readonly MyDbContext _context;

        public ReportBusiness(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<ReportModel>> GetListReportAsync(int? page)
        {
            var queryReport = _context.OrderDetails.Where(x => !x.Order.IsDeleted && x.Order.StatusOrder == StatusOrder.Completed);

            //queryReport = sortOrder switch
            //{
            //    "order_day" => queryReport.OrderBy(x => x.Order.OrderDay),
            //    _ => queryReport.OrderByDescending(x => x.Order.OrderDay),
            //};

            var listReportOrder = await queryReport.Select(detail => new
            {
                detail.Order.Payment,
                detail.Order.ExtraFee,
                detail.Product.Name,
                detail.Product.Size,
                detail.QuantityPurchased,
                detail.TotalBasePrice,
                detail.TotalPrice
            })
            .AsEnumerable()
            .GroupBy(x => x.Name)
            .Select(group => new ReportModel
            {
                ProductName = group.Key,
                QuantityPurchasedSizes = group
                                        .GroupBy(x => x.Size)
                                        .Select(group => new QuantityPurchasedSizeModel
                                        {
                                            Size = group.Key,
                                            QuantityPurchased = group.Sum(x => x.QuantityPurchased)
                                        }).ToArray(),
                PaymentMethods = group.Select(x => x.Payment).Distinct().ToArray(),
                TotalExtraFree = group.Sum(x => x.ExtraFee),
                TotalBasePrice = group.Sum(x => x.TotalBasePrice),
                TotalPrice = group.Sum(x => x.TotalPrice),
                TotalProfit = group.Sum(x => x.TotalPrice) - group.Sum(x => x.TotalBasePrice) - group.Sum(x => x.ExtraFee)
            }).AsEnumerable()
            .ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return listReportOrder.ToPagedList(pageNumber, pageSize);
        }
    }
}
