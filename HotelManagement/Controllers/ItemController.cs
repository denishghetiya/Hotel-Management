using HotelManagement.DBContext;
using HotelManagement.Helper;
using HotelManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly HotelManagementDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly EncryptionHelper _encryptionHelper;

        public ItemController(HotelManagementDBContext context, IWebHostEnvironment webHost, EncryptionHelper encryptionHelper)
        {
            _context = context;
            _webHost = webHost;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> ItemList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ItemList([FromBody] RequestPaginationViewModel model)
        {
            var query = _context.ItemLists.Include(u => u.CreatedByNavigation).Include(u => u.UpdatedByNavigation)
                .Where(u => u.IsDeleted != true)
                .AsQueryable();

            var totalCount = query.Count();

            var Order = query
                .Skip(model.Start)
                .Take(model.Length)
                .Select(a => new CreateItemViewModel
                {
                    ItemId = a.ItemId,
                    ItemName = a.ItemName,
                    Price = a.Price,
                    IsActive = a.IsActive,
                    CreatedBy = a.CreatedByNavigation.UserId,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedByNavigation.UserId,
                    UpdatedDate = a.UpdatedDate,
                }).OrderByDescending(a => a.ItemId).ToList();

            return Json(new
            {
                draw = model.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = Order
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateItem(int? itemId)
        {
            if (itemId > 0)
            {
                var item = await _context.ItemLists.FirstOrDefaultAsync(f => f.ItemId == itemId && f.IsDeleted != true);

                var model = new CreateItemViewModel
                {
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Price = item.Price,
                    IsActive = item.IsActive,
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemViewModel model)
        {
            if (model.ItemId == 0)
            {
                int userIdd = int.Parse(User.FindFirst("UserId").Value);
                var item = new ItemList
                {
                    ItemName = model.ItemName,
                    Price = model.Price,
                    IsActive = model.IsActive,
                    CreatedBy = userIdd,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = null,
                };
                _context.ItemLists.Add(item);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Item created successfully." });
            }
            if (model.ItemId > 0)
            {
                int userIdd = int.Parse(User.FindFirst("UserId").Value);
                var item = await _context.ItemLists.FirstOrDefaultAsync(f => f.ItemId == model.ItemId && f.IsDeleted != true);
                if (item == null) return Json(new { success = false, message = "Item Not Found." });

                item.ItemName = model.ItemName;
                item.Price = model.Price;
                item.IsActive = model.IsActive;
                item.UpdatedBy = userIdd;
                item.UpdatedDate = DateTime.Now;

                _context.ItemLists.Update(item);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Item updated successfully." });
            }
            return Json(new { success = false, message = "Something went wrong." });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeItemStatus(int itemId, bool isActive)
        {
            var item = _context.ItemLists.FirstOrDefault(a => a.ItemId == itemId && a.IsDeleted != true);
            item.IsActive = isActive;
            _context.ItemLists.Update(item);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Item Status Changed Successfully." });
        }
        public async Task<IActionResult> DeleteItem(int? itemId)
        {
            var item = await _context.ItemLists.FirstOrDefaultAsync(f => f.ItemId == itemId && f.IsDeleted != true);
            if (item == null) return Json(new { success = false, message = "Item Not Found." });
            var isactive = await _context.OrderItemLists.AnyAsync(f => f.ItemId == itemId && f.IsDeleted != true);
            if (isactive == true) return Json(new { success = false, message = "Item is in use." });
            item.IsActive = false;
            item.IsDeleted = true;
            _context.ItemLists.Update(item);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Item deleted successfully." });
        }
    }
}


