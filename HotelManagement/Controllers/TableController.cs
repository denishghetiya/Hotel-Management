using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using HotelManagement.DBContext;
using HotelManagement.Helper;
using HotelManagement.ViewModels;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        private readonly HotelManagementDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly EncryptionHelper _encryptionHelper;

        public TableController(HotelManagementDBContext context, IWebHostEnvironment webHost, EncryptionHelper encryptionHelper)
        {
            _context = context;
            _webHost = webHost;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> TableList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TableList([FromBody] RequestPaginationViewModel model)
        {
            var query = _context.TableLists.Include(u => u.CreatedByNavigation).Include(u => u.UpdatedByNavigation)
                .Where(u => u.IsDeleted != true)
                .AsQueryable();

            var totalCount = query.Count();

            var Order = query
                .Skip(model.Start)
                .Take(model.Length)
                .Select(a => new CreateTableViewModel
                {
                    TableId = a.TableId,
                    TableNumber = a.TableNumber,
                    TableName = a.TableName,
                    DisplayTableName = a.DisplayTableName,
                    IsActive = a.IsActive,
                    CreatedBy = a.CreatedByNavigation.UserId,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedByNavigation.UserId,
                    UpdatedDate = a.UpdatedDate,
                }).OrderByDescending(a => a.TableId).ToList();

            return Json(new
            {
                draw = model.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = Order
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateTable(int? tableId)
        {
            if (tableId > 0)
            {
                var table = await _context.TableLists.FirstOrDefaultAsync(f => f.TableId == tableId && f.IsDeleted != true);

                var model = new CreateTableViewModel
                {
                    TableId = table.TableId,
                    TableNumber = table.TableNumber,
                    TableName = table.TableName,
                    DisplayTableName = table.DisplayTableName,
                    IsActive = table.IsActive,
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(CreateTableViewModel model)
        {
            if (model.TableId == 0)
            {
                int userIdd = int.Parse(User.FindFirst("UserId").Value);
                var table = new TableList
                {
                    TableNumber = model.TableNumber,
                    TableName = model.TableName,
                    DisplayTableName = model.TableNumber +"_"+ model.TableName,
                    IsActive = model.IsActive,
                    CreatedBy = userIdd,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedDate = null,
                };
                _context.TableLists.Add(table);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Table created successfully." });
            }
            if (model.TableId > 0)
            {
                int userIdd = int.Parse(User.FindFirst("UserId").Value);
                var table = await _context.TableLists.FirstOrDefaultAsync(f => f.TableId == model.TableId && f.IsDeleted != true);
                if (table == null) return Json(new { success = false, message = "Table Not Found." });

                table.TableNumber = model.TableNumber;
                table.TableName = model.TableName;
                table.DisplayTableName = model.TableNumber +"_"+ model.TableName;
                table.UpdatedBy = userIdd;
                table.UpdatedDate = DateTime.Now;

                _context.TableLists.Update(table);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Table updated successfully." });
            }
            return Json(new { success = false, message = "Something went wrong." });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeTableStatus(int tableId, bool isActive)
        {
            var table = _context.TableLists.FirstOrDefault(a => a.TableId == tableId && a.IsDeleted != true);
            table.IsActive = isActive;
            _context.TableLists.Update(table);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Table Status Changed Successfully." });
        }
        public async Task<IActionResult> DeleteTable(int? tableId)
        {
            var table = await _context.TableLists.FirstOrDefaultAsync(f => f.TableId == tableId && f.IsDeleted != true);
            if (table == null) return Json(new { success = false, message = "Table Not Found." });
            var isactive = await _context.OrderLists.AnyAsync(f => f.TableId == tableId && f.IsDeleted != true);
            if (isactive == true) return Json(new { success = false, message = "Table is in use." });
            table.IsActive = false;
            table.IsDeleted = true;
            _context.TableLists.Update(table);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Table deleted successfully." });
        }
    }
}


