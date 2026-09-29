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
    public class DashboardController : Controller
    {
        private readonly HotelManagementDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly EncryptionHelper _encryptionHelper;

        public DashboardController(HotelManagementDBContext context, IWebHostEnvironment webHost, EncryptionHelper encryptionHelper)
        {
            _context = context;
            _webHost = webHost;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            return View("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(int? userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => f.UserId == userId && f.IsActive == true && f.IsDeleted != true);
            if (user == null) return NotFound();

            var model = new EditProfileViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Password = _encryptionHelper.Decrypt(user.Password),
                ExistingImagePath = user.ImageName != null ? $"/Uploads/{user.ImageName}" : null
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditProfileViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(f => f.UserId == model.UserId);
            if (user == null) return Json(new { success = false, message = "User Not Found." });

            var email = _context.Users.FirstOrDefault(u =>u.UserId != model.UserId && u.Email == model.Email);
            if (email != null)
            {
                return Json(new { success = false, message = "This Email has already account." });
            }

            if (email == null)
            {
                user.Email = model.Email;
            }

            user.Username = model.Username;
            user.Password = _encryptionHelper.Encrypt(model.Password);

            if (model.Image != null)
            {
                var uploadsFolder = Path.Combine(_webHost.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (!string.IsNullOrEmpty(user.ImageName))
                {
                    var oldImagePath = Path.Combine(uploadsFolder, user.ImageName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var originalFileName = Path.GetFileNameWithoutExtension(model.Image.FileName);
                var extension = Path.GetExtension(model.Image.FileName);
                var uniqueFileName = $"{originalFileName}_{timeStamp}{extension}";

                var fileSavePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(fileSavePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                user.ImageName = uniqueFileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "User Updated Successfully." });
        }
    }
}


