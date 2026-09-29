using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelManagement.DBContext;
using HotelManagement.Helper;
using HotelManagement.Services;
using HotelManagement.ViewModels;

namespace HotelManagement.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly HotelManagementDBContext _context;
        private readonly EmailService _emailService;
        private readonly EncryptionHelper _encryptionHelper;
        private readonly IJwtHelper _jwtHelper;
        private readonly IWebHostEnvironment _webHost;

        public LoginController(HotelManagementDBContext context, EmailService emailService, EncryptionHelper encryptionHelper, IJwtHelper jwtHelper, IWebHostEnvironment webHost)
        {
            _context = context;
            _emailService = emailService;
            _encryptionHelper = encryptionHelper;
            _jwtHelper = jwtHelper;
            _webHost = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.IsActive == true && u.IsDeleted != true );
            
            if (user == null)
            {
                return Json(new { success = false, message = "Invalid Email or Password." });
            }
            var decpass = _encryptionHelper.Decrypt(user.Password);
            if (decpass != model.Password)
            {
                return Json(new { success = false, message = "Invalid Password." });
            }
            var loginData = new UserInfoViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
            var token = _jwtHelper.GenerateJSONWebToken(loginData);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("Username", user.Username.ToString()),
                new Claim("Email", user.Email.ToString()),
                new Claim("Token", token)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            HttpContext.Session.SetString("UserId", Convert.ToString(user.UserId));
            HttpContext.Session.SetString("Username", Convert.ToString(user.Username));
            HttpContext.Session.SetString("Email", Convert.ToString(user.Email));

            await HttpContext.SignInAsync("Cookies", principal);

            return Json(new { success = true, message = "Logged in Successfully." });
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("Cookies");
            return Json(new { success = true, message = "Logout Successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return Json(new { success = false, message = "Password and ConfirmPassword are should be same." });
            }
            var email = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.IsDeleted != true);
            if (email == null)
            {
                if (model.Image != null)
                {
                    var uploadsFolder = Path.Combine(_webHost.WebRootPath, "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
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
                
                    var user = new User
                    {
                        Username = model.Username,
                        Email = model.Email,
                        Password = _encryptionHelper.Encrypt(model.Password),
                        CreatedDate = DateTime.Now,
                        ImageName = uniqueFileName,
                        IsActive = true
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var resetLink = Url.Action("ActiveAccount", "Login", new { email = model.Email }, Request.Scheme);
                    string subject = "Confirm the Request";
                    string body = $@"<!DOCTYPE html>
                        <html>
                        <body>
                            <p>Click the button below to Confirm the request:</p>
                            <a href=""{resetLink}"">Confirm</a>
                        </body>
                        </html>";

                    //await _emailService.SendEmailAsync(model.Email, subject, body);
                    //return Json(new { success = true, message = "Register Successfully. Go in your Email Account and confirm the request." });
                    return Json(new { success = true, message = "Register Successfully." });
                }
                return Json(new { success = false, message = "Image not found." });
            }
            return Json(new { success = false, message = "This Email has already account." });
        }
        public async Task<IActionResult> ActiveAccount(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.IsDeleted != true);
            if (user != null)
            {
                user.IsActive = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Login");
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                var loginData = new UserInfoViewModel
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                };

                //var token = Guid.NewGuid().ToString();
                //var encryptedToken = _encryptionHelper.Encrypt(token);
                var token = _jwtHelper.GenerateJSONWebToken(loginData);

                user.ResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                _context.Update(user);
                await _context.SaveChangesAsync();
                
                var resetLink = Url.Action("ResetPassword", "Login", new { token = token }, Request.Scheme);

                string subject = "Reset Your Password";
                string body = $@"<!DOCTYPE html>
                    <html>
                    <body>
                        <p>Click the link below to reset your password:</p>
                        <a href=""{resetLink}"">Reset Password</a>
                    </body>
                    </html>";

                await _emailService.SendEmailAsync(model.Email, subject, body);
                return Json(new { success = true, message = "ResetPassword Link send on Email Successfully." });
            }
            return Json(new { success = false, message = "Account with this email does not exist." });
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest();

            return View(new ResetPasswordViewModel { Token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (model.NewPassword == model.ConfirmPassword)
            {
                //var decryptedToken = _encryptionHelper.Decrypt(model.Token);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(model.Token);
                var token = jsonToken as JwtSecurityToken;

                var userId = token.Claims.FirstOrDefault(t=>t.Type == "UserId");
                var username = token.Claims.FirstOrDefault(t=>t.Type == "Username");
                var email = token.Claims.FirstOrDefault(t=>t.Type == "Email");

                var user = _context.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(userId.Value) && u.Username == username.Value  && u.Email == email.Value && u.ResetToken == model.Token && u.ResetTokenExpiry > DateTime.UtcNow);
                if (user == null)
                {
                    return Json(new { success = false, message = "Invalid or expired token." });
                }

                user.Password = _encryptionHelper.Encrypt(model.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                _context.Update(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Reset Password Successfully." });
            }
            return Json(new { success = false, message = "New Password and Confirm Password are not same." });
        }

        public IActionResult AccessDenied() => View();
    }
}


