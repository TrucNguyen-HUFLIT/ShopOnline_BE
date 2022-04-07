using ShopOnline.Core.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopOnline.Core.Helpers
{
    public sealed class BrandSingleton
    {
        public static BrandSingleton Instance { get; } = new BrandSingleton();
        public List<BrandInforModel> BrandInfors { get; } = new List<BrandInforModel>();
        private BrandSingleton() { }
        private List<int> Ids { get; } = new List<int>();

        public void Init(List<BrandInforModel> brandInfors)
        {
            if (BrandInfors.Count == 0 || BrandInfors.Count != brandInfors.Count)
            {
                foreach (var brandInfor in brandInfors)
                {
                    var isExisted = Ids.Contains(brandInfor.Id);
                    if (!isExisted)
                    {
                        Ids.Add(brandInfor.Id);
                        BrandInfors.Add(brandInfor);
                    }
                }

                var newIds = brandInfors.Select(x => x.Id).ToArray();
                var currentAmount = Ids.Count;

                for (int i = 0; i < currentAmount; i++)
                {
                    var isNotDeleted = newIds.Contains(Ids[i]);
                    if (!isNotDeleted)
                    {
                        BrandInfors.Remove(BrandInfors.Where(x => x.Id == Ids[i]).FirstOrDefault());
                        Ids.Remove(Ids[i]);
                    }
                }
            }
        }
    }
}
