using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Entities;
using ShopOnline.Core.Exceptions;
using ShopOnline.Core.Helpers;
using ShopOnline.Core.Models.Enum;
using ShopOnline.Core.Models.Staff;
using ShopOnline.Core.Validators.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Staff
{
    public class StaffBusiness : IStaffBusiness
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;
        public StaffBusiness(MyDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task<PagedCollectionResultModel<StaffInfor>> GetListStaffAsync(StaffParamsModel model, TypeAcc typeAcc)
        {
            var staffQuery = _context.Staffs.Where(x => !x.IsDeleted && x.TypeAcc == typeAcc);

            if (!string.IsNullOrWhiteSpace(model.Terms))
            {
                var termsNormalize = model.Terms.Trim().ToUpperInvariant();
                staffQuery = staffQuery.Where(x => x.Id.ToString() == termsNormalize
                                            || x.FullName.Contains(termsNormalize)
                                            || x.Email.Contains(termsNormalize));
            }

            switch (model.SortBy)
            {
                case CustomerSortByEnum.Name:
                    staffQuery = model.IsDescending
                         ? staffQuery.OrderByDescending(x => x.FullName)
                         : staffQuery.OrderBy(x => x.FullName);
                    break;
                case CustomerSortByEnum.Id:
                    staffQuery = model.IsDescending
                         ? staffQuery.OrderByDescending(x => x.Id)
                         : staffQuery.OrderBy(x => x.Id);
                    break;
                default:
                    staffQuery = model.IsDescending
                         ? staffQuery.OrderByDescending(x => x.Id)
                         : staffQuery.OrderBy(x => x.Id);
                    break;
            }

            var totalRecord = staffQuery.Count();

            var staffs = await staffQuery.Select(x => new StaffInfor
            {
                Id = x.Id,
                Address = x.Address,
                Email = x.Email,
                Avatar = x.Avatar,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
            }).Skip(model.Skip).Take(model.Take).ToListAsync();

            return new PagedCollectionResultModel<StaffInfor>
            {
                Skip = model.Skip,
                Take = model.Take,
                Total = totalRecord,
                Result = staffs,
                Terms = model.Terms
            };
        }

        public async Task <int> CreateAsync(StaffCreate staffCreate, TypeAcc typeAcc)
        {
            var isExistedAcc = await _context.Staffs.AnyAsync(x => x.Email == staffCreate.Email && !x.IsDeleted && x.TypeAcc == typeAcc);

            if (!isExistedAcc)
            {
                HashPasswordHelper.HashPasswordStrategy = new HashSHA256Strategy();
                var staff = new StaffEntity
                {
                    FullName = staffCreate.FullName,
                    Address = staffCreate.Address,
                    Salary = staffCreate.Salary,
                    Email = staffCreate.Email,
                    Password = HashPasswordHelper.DoHash(staffCreate.Password),
                    PhoneNumber = staffCreate.PhoneNumber,
                    TypeAcc = typeAcc,
                };
                if (staffCreate.UploadAvt == null)
                {
                    staff.Avatar = "/img/Avatar/avatar-icon-images-4.jpg";
                }
                else
                {
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(staffCreate.UploadAvt.FileName);
                    string extension = Path.GetExtension(staffCreate.UploadAvt.FileName);
                    staff.Avatar = "/img/Avatar/" + fileName + extension;
                    string path1 = Path.Combine(wwwRootPath + "/img/Avatar/", fileName + extension);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await staffCreate.UploadAvt.CopyToAsync(fileStream);
                    }
                }
                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();
                return staff.Id;
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.EmailExisted);
            }
        }

        public async Task<StaffEdit> GetStaffById(int id, TypeAcc typeAcc)
        {
            var staff = await _context.Staffs.Where(x => x.Id == id && !x.IsDeleted && x.TypeAcc == typeAcc).Select(x => new StaffEdit
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Avatar = x.Avatar,
                TypeAcc = x.TypeAcc
            }).FirstOrDefaultAsync();
            return staff;

        }

        public async Task<bool> EditAsync(StaffEdit staffEdit, TypeAcc typeAcc)
        {
            var staffEntity = await _context.Staffs.Where(x => x.Id == staffEdit.Id && !x.IsDeleted && x.TypeAcc == typeAcc).FirstOrDefaultAsync();

            staffEntity.FullName = staffEdit.FullName;
            staffEntity.Address = staffEdit.Address;
            staffEntity.PhoneNumber = staffEdit.PhoneNumber;

            if (staffEdit.UploadAvt != null)
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName1;
                string extension1;

                fileName1 = Path.GetFileNameWithoutExtension(staffEdit.UploadAvt.FileName);
                extension1 = Path.GetExtension(staffEdit.UploadAvt.FileName);
                staffEntity.Avatar = fileName1 += extension1;
                string path1 = Path.Combine(wwwRootPath + "/img/Avatar/", fileName1);
                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await staffEdit.UploadAvt.CopyToAsync(fileStream);
                }
            }
            _context.Staffs.Update(staffEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public StaffEdit GetStaffByEmail(string email, TypeAcc typeAcc)
        {
            var staff = _context.Staffs.Where(x => x.Email == email && !x.IsDeleted && x.TypeAcc == typeAcc).Select(x => new StaffEdit
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Avatar = x.Avatar,
                TypeAcc = x.TypeAcc
            }).FirstOrDefault();
            return staff;
        }

        public async Task<bool> DeleteStaffAsync(int id, TypeAcc typeAcc)
        {
            var staff = await _context.Staffs.Where(x => x.Id == id && !x.IsDeleted && x.TypeAcc == typeAcc).FirstOrDefaultAsync();

            if (staff != null)
            {
                staff.IsDeleted = true;
                _context.Staffs.Update(staff);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IPagedList<StaffInfor>> GetListShipperAccAsync(string sortOrder, string searchString, int? page)
        {
            var listStaff = new List<StaffInfor>();
            var shippers = await _context.Shippers.Where(x => !x.IsDeleted).ToListAsync();
            if (shippers != null)
            {
                foreach (var shipper in shippers)
                {
                    var staffInforForList = new StaffInfor
                    {
                        Id = shipper.Id,
                        Email = shipper.Email,
                        FullName = shipper.FullName,
                        Avatar = shipper.Avatar,
                        TypeAcc = shipper.TypeAcc,
                        Address = shipper.Address,
                        PhoneNumber = shipper.PhoneNumber
                    };
                    listStaff.Add(staffInforForList);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listStaff = listStaff.Where(s => s.FullName.ToLower().Contains(searchString.ToLower())
                                            || s.Email.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listStaff = sortOrder switch
                {
                    "name_desc" => listStaff.OrderByDescending(x => x.FullName).ToList(),
                    "name" => listStaff.OrderBy(x => x.FullName).ToList(),
                    "id_desc" => listStaff.OrderByDescending(x => x.Id).ToList(),
                    _ => listStaff.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listStaff.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }

        public async Task CreateShipperAsync(StaffCreate staffCreate)
        {
            var isExistedAcc = await _context.Shippers.AnyAsync(x => x.Email == staffCreate.Email && !x.IsDeleted);

            if (!isExistedAcc)
            {
                HashPasswordHelper.HashPasswordStrategy = new HashSHA1Strategy();
                var shipper = new ShipperEntity
                {
                    FullName = staffCreate.FullName,
                    Address = staffCreate.Address,
                    Salary = staffCreate.Salary,
                    Email = staffCreate.Email,
                    Password = HashPasswordHelper.DoHash(staffCreate.Password),
                    PhoneNumber = staffCreate.PhoneNumber,
                    TypeAcc = TypeAcc.Shipper,
                };
                if (staffCreate.UploadAvt == null)
                {
                    shipper.Avatar = "/img/Avatar/avatar-icon-images-4.jpg";
                }
                else
                {
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(staffCreate.UploadAvt.FileName);
                    string extension = Path.GetExtension(staffCreate.UploadAvt.FileName);
                    shipper.Avatar = "/img/Avatar/" + fileName + extension;
                    string path1 = Path.Combine(wwwRootPath + "/img/Avatar/", fileName + extension);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await staffCreate.UploadAvt.CopyToAsync(fileStream);
                    }
                }
                _context.Shippers.Add(shipper);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.EmailExisted);
            }

        }

        public StaffEdit GetShipperById(int id)
        {
            var shipper = _context.Shippers.Where(x => x.Id == id && !x.IsDeleted).Select(x => new StaffEdit
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Avatar = x.Avatar,
                TypeAcc = x.TypeAcc
            }).FirstOrDefault();
            return shipper;

        }

        public async Task<bool> EditShipperAsync(StaffEdit staffEdit)
        {
            var shipper = await _context.Shippers.Where(x => x.Id == staffEdit.Id && !x.IsDeleted).FirstOrDefaultAsync();

            shipper.FullName = staffEdit.FullName;
            shipper.Address = staffEdit.Address;
            shipper.PhoneNumber = staffEdit.PhoneNumber;

            if (staffEdit.UploadAvt != null)
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName1;
                string extension1;

                fileName1 = Path.GetFileNameWithoutExtension(staffEdit.UploadAvt.FileName);
                extension1 = Path.GetExtension(staffEdit.UploadAvt.FileName);
                shipper.Avatar = fileName1 += extension1;
                string path1 = Path.Combine(wwwRootPath + "/img/Avatar/", fileName1);
                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await staffEdit.UploadAvt.CopyToAsync(fileStream);
                }
            }
            _context.Shippers.Update(shipper);
            await _context.SaveChangesAsync();
            return true;
        }

        public StaffEdit GetShipperByEmail(string email)
        {
            var staff = _context.Shippers.Where(x => x.Email == email && !x.IsDeleted).Select(x => new StaffEdit
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Avatar = x.Avatar,
                TypeAcc = x.TypeAcc
            }).FirstOrDefault();
            return staff;
        }

        public async Task<bool> DeleteShipperAsync(int id)
        {
            var shipper = await _context.Shippers.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();

            if (shipper != null)
            {
                shipper.IsDeleted = true;
                _context.Shippers.Update(shipper);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
