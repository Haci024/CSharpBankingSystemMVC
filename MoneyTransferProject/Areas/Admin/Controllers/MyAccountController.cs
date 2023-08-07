using BussinessLayer.Abstract;
using BussinessLayer.Concrete;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.EFrameworkCore;
using DTOLayer.DTOS.AppUserDTOS;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MimeKit.Cryptography;
using MoneyTransferProject.Models;
using MoneyTransferProject.ViewModels;
using NuGet.Protocol;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class MyAccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICustomerAccountService _customerAccountService;
        private readonly AppDbContext _db;
        CustomerAccountManager CAM = new CustomerAccountManager(new EFCustomerAccountRepository());
        public MyAccountController(UserManager<AppUser> userManager, AppDbContext db, SignInManager<AppUser> signInManager, ICustomerAccountService customerAccountService)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _customerAccountService = customerAccountService;
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> AllCards()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var customerAccounts = CAM.TMyCardsList(user.Id);


            return View(customerAccounts);
        }

        #region Hesabdan çıxış/LogOut
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {

                return NotFound();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        #endregion
        #region İstifadəçiMəlumatlarıDəyiş/UpdateUserInfo
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUserUpdateDto appUserUpdateDto = new AppUserUpdateDto();
            appUserUpdateDto.Name = appUser.Name;
            appUserUpdateDto.Surname = appUser.Surname;
            appUserUpdateDto.UserName = appUser.UserName;
            appUserUpdateDto.PhoneNumber = "+9940555515345";
            appUserUpdateDto.ImageUrl = "Test";
            appUserUpdateDto.Email = appUser.Email;
            appUserUpdateDto.District = appUser.District;
            appUserUpdateDto.City = appUser.City;

            return View(appUserUpdateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUserUpdateDto appUserEditDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.PhoneNumber = appUserEditDto.PhoneNumber;
                user.Surname = appUserEditDto.Surname;
                user.UserName = appUserEditDto.UserName;
                user.City = appUserEditDto.City;
                user.District = appUserEditDto.District;
                user.Name = appUserEditDto.Name;
                user.ImageUrl = "test";
                user.Email = appUserEditDto.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(appUserEditDto);
        }
        #endregion
        #region ŞifrəYeniləmə/UpdatePassword
        [HttpGet]
        public IActionResult UpdatePassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(AppUserPasswordUpdateDto appUserPasswordUpdateDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                if (appUserPasswordUpdateDto.Password.Length < 6)
                {
                    ModelState.AddModelError("", "Şifre en az 6 simvol uzunluğunda olmalıdır.");
                    return View();
                }
                if (!appUserPasswordUpdateDto.Password.Any(char.IsNumber))
                {

                    ModelState.AddModelError("", "Şifrənizdə minimum 1 rəqəm olmalıdır!");

                    return View();
                }

                if (!appUserPasswordUpdateDto.Password.Any(char.IsUpper) || !appUserPasswordUpdateDto.Password.Any(char.IsLower))
                {
                    ModelState.AddModelError("", "Şifre ən az bir böyük harf ve bir kiçik hərf olmalıdır.");

                    return View();
                }
            }
            {
                if (appUserPasswordUpdateDto.Password == appUserPasswordUpdateDto.ConfirmPassword)
                {

                    if (!string.IsNullOrEmpty(appUserPasswordUpdateDto.Password))
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, appUserPasswordUpdateDto.Password);
                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {

                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {

                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Yeni şifrəni daxil etmədiniz");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Şifrələr eyni deyil!");
                }
                return View(appUserPasswordUpdateDto);
            }
            #endregion

        }


    }

}

