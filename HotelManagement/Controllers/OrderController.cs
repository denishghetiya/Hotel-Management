using HotelManagement.DBContext;
using HotelManagement.Helper;
using HotelManagement.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly HotelManagementDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly EncryptionHelper _encryptionHelper;

        public OrderController(HotelManagementDBContext context, IWebHostEnvironment webHost, EncryptionHelper encryptionHelper)
        {
            _context = context;
            _webHost = webHost;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrderList([FromBody] RequestPaginationViewModel model)
        {
            var query = _context.OrderLists.Include(u => u.CreatedByNavigation).Include(u => u.UpdatedByNavigation).Include(u => u.Table)
                .Where(u => u.IsDeleted != true)
                .AsQueryable();

            if (model.Filters.field1 != null && model.Filters.field2 == null)
            {
                query = query.Where(b => b.CreatedDate >= model.Filters.field1);
            }
            if (model.Filters.field1 == null && model.Filters.field2 != null)
            {
                query = query.Where(b => b.CreatedDate <= model.Filters.field2);
            }
            if (model.Filters.field1 != null && model.Filters.field2 != null)
            {
                query = query.Where(b => b.CreatedDate >= model.Filters.field1 && b.CreatedDate <= model.Filters.field2);
            }

            var sortColumn = model.Columns?.FirstOrDefault(c => c.Sort != null && c.IsSortable);
            if (sortColumn != null && !string.IsNullOrEmpty(sortColumn.Field))
            {
                var direction = sortColumn.Sort?.Direction ?? KSortDirection.Ascending;

                switch (sortColumn.Field)
                {
                    case "customerName":
                        query = direction == KSortDirection.Ascending
                            ? query.OrderBy(u => u.CustomerName)
                            : query.OrderByDescending(u => u.CustomerName);
                        break;

                    case "displayTableName":
                        query = direction == KSortDirection.Ascending
                            ? query.OrderBy(u => u.Table.TableName)
                            : query.OrderByDescending(u => u.Table.TableName);
                        break;

                    case "createdDate":
                        query = direction == KSortDirection.Ascending
                            ? query.OrderBy(u => u.CreatedDate)
                            : query.OrderByDescending(u => u.CreatedDate);
                        break;

                    case "billAmount":
                        query = direction == KSortDirection.Ascending
                            ? query.OrderBy(u => u.BillAmount)
                            : query.OrderByDescending(u => u.BillAmount);
                        break;

                    case "billPayed":
                        query = direction == KSortDirection.Ascending
                            ? query.OrderBy(u => u.BillPayed)
                            : query.OrderByDescending(u => u.BillPayed);
                        break;

                    default:
                        query = query.OrderByDescending(u => u.OrderId);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(u => u.OrderId);
            }

            var totalCount = query.Count();

            var Order = query
                .Skip(model.Start)
                .Take(model.Length)
                .Select(a => new OrderViewModel
                {
                    OrderId = a.OrderId,
                    TableId = a.TableId,
                    DisplayTableName = a.Table.DisplayTableName,
                    CustomerName = a.CustomerName,
                    BillAmount = a.BillAmount,
                    BillPayed = a.BillPayed,
                    CreatedBy = a.CreatedByNavigation.UserId,
                    CreatedDate = a.CreatedDate,
                    UpdatedBy = a.UpdatedByNavigation.UserId,
                    UpdatedDate = a.UpdatedDate,
                }).ToList();

            return Json(new
            {
                draw = model.Draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = Order
            });
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(int? orderId)
        {
            var tables = await _context.TableLists
                .Where(r => r.IsActive == true && r.IsDeleted != true)
                .Select(r => new CreateTableViewModel
                {
                    TableId = r.TableId,
                    DisplayTableName = r.DisplayTableName
                })
                .ToListAsync();

            var dropdownItems = await _context.ItemLists
                .Where(x => !x.IsDeleted && x.IsActive == true)
                .Select(x => new CreateItemViewModel
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    Price = x.Price
                })
                .ToListAsync();

            if (orderId > 0)
            {
                var order = await _context.OrderLists.Include(f=>f.Table).Include(f=>f.OrderItemLists)
                    .FirstOrDefaultAsync(f => f.OrderId == orderId && f.IsDeleted != true);

                if (order == null)
                    return RedirectToAction("OrderList");

                var model = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    TableId = order.TableId,
                    CustomerName = order.CustomerName,
                    BillAmount = order.BillAmount,
                    DisplayTableName = order.Table.DisplayTableName,
                    BillPayed = order.BillPayed,
                    Tables = tables,
                    ItemList = dropdownItems,
                    OrderItems = order.OrderItemLists
                        .Where(x => !x.IsDeleted)
                        .Select(x => new OrderItemViewModel
                        {
                            OrderItemId = x.OrderItemId,
                            ItemId = x.ItemId,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            FinalPrice = x.FinalPrice
                        }).ToList()
                };
                return View(model);
            }

            return View(new OrderViewModel
            {
                Tables = tables,
                ItemList = dropdownItems
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel model)
        {
            int userIdd = int.Parse(User.FindFirst("UserId").Value);
            foreach (var ip in model.OrderItems.Where(p => p != null))
            {
                var itemm = await _context.ItemLists.FirstOrDefaultAsync(ii => ii.ItemId == ip.ItemId);
                if (itemm != null)
                {
                    if (itemm.Price != ip.Price)
                    {
                        itemm.Price = ip.Price;
                        _context.ItemLists.Update(itemm);
                    }
                }
            }
            if (model.OrderId == 0)
            {
                var order = new OrderList
                {
                    CustomerName = model.CustomerName,
                    TableId = model.TableId,
                    BillAmount = model.BillAmount,
                    BillPayed = model.BillPayed,
                    CreatedBy = userIdd,
                    CreatedDate = DateTime.Now,
                    OrderItemLists = model.OrderItems.Where(oi => oi != null).Select(oi => new OrderItemList
                    {
                        ItemId = oi.ItemId,
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        FinalPrice = oi.FinalPrice,
                        CreatedBy = userIdd,
                        CreatedDate = DateTime.Now,
                    }).ToList() 
                };
                _context.OrderLists.Add(order);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Order created successfully." });
            }
            if (model.OrderId > 0)
            {
                var order = await _context.OrderLists.Include(o => o.OrderItemLists).FirstOrDefaultAsync(f => f.TableId == model.TableId && f.IsDeleted != true);
                if (order == null) return Json(new { success = false, message = "Order Not Found." });

                order.CustomerName = model.CustomerName;
                order.TableId = model.TableId;
                order.BillAmount = model.BillAmount;
                order.BillPayed = model.BillPayed;
                order.UpdatedBy = userIdd;
                order.UpdatedDate = DateTime.Now;

                foreach (var i in model.OrderItems.Where(it => it != null))
                {
                    var item = order.OrderItemLists.FirstOrDefault(oi => oi.OrderItemId == i.OrderItemId && oi.IsDeleted != true);
                    if (item == null || item.OrderItemId == 0)
                    {
                        order.OrderItemLists.Add(new OrderItemList
                        {
                            OrderId = model.OrderId,
                            ItemId = i.ItemId,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            FinalPrice = i.FinalPrice,
                            CreatedBy = userIdd,
                            CreatedDate = DateTime.Now
                        });
                    }
                    else
                    {
                        item.ItemId = i.ItemId;
                        item.Quantity = i.Quantity;
                        item.Price = i.Price;
                        item.FinalPrice = i.FinalPrice;
                        item.UpdatedBy = userIdd;
                        item.UpdatedDate = DateTime.Now;
                    }
                }

                var uiitem = model.OrderItems.Select(x => x.OrderItemId).ToList();
                var delitem = order.OrderItemLists
                    .Where(oi => !uiitem.Contains(oi.OrderItemId) && !oi.IsDeleted)
                    .ToList();

                foreach (var del in delitem)
                {
                    del.IsDeleted = true;
                    del.UpdatedBy = userIdd;
                    del.UpdatedDate = DateTime.Now;
                }

                _context.OrderLists.Update(order);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Order updated successfully." });
            }
            return Json(new { success = false, message = "Something went wrong." });
        }
        [HttpPost]
        public async Task<IActionResult> IsBillPayed(int orderId, bool billpayed)
        {
            var order = _context.OrderLists.FirstOrDefault(a => a.OrderId == orderId && a.IsDeleted != true);
            order.BillPayed = billpayed;
            _context.OrderLists.Update(order);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Bill Payed." });
        }
        public async Task<IActionResult> DeleteOrder(int? orderId)
        {
            var order = await _context.OrderLists.FirstOrDefaultAsync(f => f.OrderId == orderId && f.IsDeleted != true);
            if (order == null) return Json(new { success = false, message = "Order Not Found." });
            order.IsDeleted = true;
            order.OrderItemLists.Where(oi => oi.IsDeleted != true).ToList().ForEach(ii => ii.IsDeleted = true);
            _context.OrderLists.Update(order);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Order deleted successfully." });
        }
        [HttpGet]
        public IActionResult GetItemList()
        {
            var items = _context.ItemLists
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new
                {
                    itemId = x.ItemId,
                    itemName = x.ItemName,
                    price = x.Price
                })
                .ToList();

            return Json(items);
        }

        public async Task<IActionResult> PrintOrderPDF(int orderId)
        {
            var order = _context.OrderLists
                .Include(o => o.OrderItemLists)
                .ThenInclude(i => i.Item)
                .Include(o => o.Table)
                .FirstOrDefault(o => o.OrderId == orderId && !o.IsDeleted);

            if (order == null)
                return NotFound("Order not found");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                doc.Add(new Paragraph("Order Bill", titleFont));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph($"Order Id: {order.OrderId}"));
                doc.Add(new Paragraph($"Customer Name: {order.CustomerName}"));
                doc.Add(new Paragraph($"Table: {order.Table.DisplayTableName}"));
                doc.Add(new Paragraph($"Bill Payed: {order.BillPayed}"));
                doc.Add(new Paragraph($"Date: {order.CreatedDate}\n\n"));

                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;

                table.AddCell("Item Name");
                table.AddCell("Price");
                table.AddCell("Qty");
                table.AddCell("Final Price");

                foreach (var item in order.OrderItemLists.Where(x => !x.IsDeleted))
                {
                    table.AddCell(item.Item.ItemName);
                    table.AddCell(item.Price.ToString());
                    table.AddCell(item.Quantity.ToString());
                    table.AddCell(item.FinalPrice.ToString());
                }

                doc.Add(table);

                doc.Add(new Paragraph($"Bill Amount: {order.BillAmount}"));

                doc.Close();

                byte[] bytes = ms.ToArray();
                return File(bytes, "application/pdf", $"Order_{orderId}.pdf");
            }
        }
    }
}


