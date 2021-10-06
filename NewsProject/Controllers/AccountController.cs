using Common.Utilities.Convertors;
using DataLayer.Contracts;
using DataLayer.DTOs.User;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using infrastructure.Services.SendEmails;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NewsProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private IViewRenderService _viewRender;

        public AccountController(IUserRepository userRepository,UserManager<User> userManager,IEmailSender emailSender,IViewRenderService viewRender)
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._viewRender = viewRender;
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel register, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }  
            User user = new User()
            {
                UserName = register.Email,
                Email = register.Email,
                RegisterDate = DateTime.Now,
                Password = register.Password,
                UserAvatar = "Defult.jpg",
                ActiveCode = NameGenerator.GeneratorUniqCode(),
                IsActive = true
            };
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, register.Password);
            var c =await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "AKBAR");
            //await _userRepository.AddAsync(user, cancellationToken);
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userRepository.IsExistEmailAndPassword(login, cancellationToken);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,login.Email),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name,login.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                        IsPersistent = login.RememberMe,
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    ViewBag.IsSuccess = true;
                    return View();

                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمیباشد");
                }
            }
            ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد");
            return View(login);
        }

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassword,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPassword);
            }
            string fixdEmail = FixedText.FixeEmail(forgotPassword.Email);
            User user =await _userRepository.IsExistUserByUserEmail(fixdEmail,cancellationToken);
            if (user==null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
            }
            string bodyEmail =_viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(forgotPassword.Email, "تغییر رمز", bodyEmail);
            return View();
        }

        public IActionResult ResetForgotPassword(string token)
        {
            ViewData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetForgotPassword(string token,ResetForgotPasswordViewModel reset, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            var user =await _userRepository.TableNoTracking.SingleOrDefaultAsync(u=>u.ActiveCode==token);
            string hashedpass = _userManager.PasswordHasher.HashPassword(user,reset.Password);

            user.Password = reset.Password;

            user.PasswordHash = hashedpass;
            await _userRepository.UpdateAsync(user,cancellationToken);
            ViewBag.IsSuccess = true;
            return RedirectToAction(nameof(Login));
        }

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion

    }

}
