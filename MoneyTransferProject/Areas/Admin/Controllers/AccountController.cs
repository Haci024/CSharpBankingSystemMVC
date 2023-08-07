using BussinessLayer.ValidationRules.AppUserValidation;
using DataAccsessLayer.Concrete;
using DTOLayer.DTOS.AppUserDTOS;
using EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MoneyTransferProject.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MoneyTransferProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register/Qeydiyyat 
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AppUserRegisterDto appUserRegisterDto)
        {
            var validator = new AppUserRegisterValidator();
            var validationResult = await validator.ValidateAsync(appUserRegisterDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(appUserRegisterDto);
            }


            Random random = new Random();
            int confirmCode = random.Next(100000, 1000000);
            AppUser appUser = new AppUser()
            {
                Name = appUserRegisterDto.Name,
                Email = appUserRegisterDto.Email,
                UserName = appUserRegisterDto.Username,
                Surname = appUserRegisterDto.Surname,
                City = "Baku",
                District = "ResidBehbudov",
                ImageUrl = "image.jpg",
                ConfirmCode = confirmCode,
                RemainingTimeForConfirmCode = 120,
                RuleAndContract = appUserRegisterDto.RuleAndContract,

            };
            if (appUser.RuleAndContract == false)
            {
                ModelState.AddModelError("", "Qaydaları və Məlumatları qəbul etdiyinizi təsdiq edin!");
                return View();
            }
            var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);
            if (result.Succeeded)
            {
                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
                MailboxAddress ConfirmAdressTo = new MailboxAddress("User", appUser.Email);

                mimeMessage.From.Add(ConfirmAddressFrom);
                mimeMessage.To.Add(ConfirmAdressTo);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Email ünvanı təsdiqləmək üçün kodunuz:" + confirmCode;

                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = "Email təsdiq kodunuz";

                SmtpClient client = new SmtpClient();

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");

                client.Send(mimeMessage);
                client.Disconnect(true);


                TempData["Mail"] = appUserRegisterDto.Email;
                return RedirectToAction("ConfirmMail", "Account", new { appUser.UserName });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(appUserRegisterDto);
            }
        }
        #endregion
        #region ConfirmMail/EmailTəsdiqləmə
        //[HttpGet]
        //public async Task<IActionResult> ConfirmMail(string UserName)
        //{
        //	var value = TempData["Mail"];
        //	ViewBag.Mail = value;

        //	AppUser appUser = await _userManager.FindByNameAsync(UserName);
        //	ViewBag.RemainingTimeForConfirmCode = appUser.RemainingTimeForConfirmCode;

        //	if (appUser == null)
        //	{

        //		return RedirectToAction("ResendConfirmationEmail", "Account");
        //	}

        //	return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ConfirmMail(ConfirmCodeViewModel confirmCodeViewModel)
        //{
        //	if (confirmCodeViewModel.Email == null)
        //	{
        //		ModelState.AddModelError("", "Email daxil edin");
        //		return View(confirmCodeViewModel);

        //	}
        //	var user = await _userManager.FindByEmailAsync(confirmCodeViewModel.Email);
        //	ViewBag.RemainingTimeForConfirmCode = user.RemainingTimeForConfirmCode;
        //	if (user.RemainingTimeForConfirmCode == 0 || DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalSeconds >= 120)
        //	{
        //		ModelState.AddModelError("", "Şifrə təsdiqi üçün vaxtınız bitdi. Yeni təsdiq kodu üçün istək göndərin!");
        //		return View();
        //	}
        //	if (user == null)
        //	{
        //		ModelState.AddModelError("", "Daxil etdiyiniz elektron ünvanla heç bir istifadəçi qeydiyyatdan keçməyib!");
        //		return View(confirmCodeViewModel);
        //	}
        //	if (user.RemainingTimeForConfirmCode==0)
        //	{
        //		ModelState.AddModelError("", "Təsdiq mesajındakı şifrəni verilmiş vaxt ərzində dəyişmədiyiniz üçün yeni şifrə almqın");
        //		return View(confirmCodeViewModel);
        //	}
        //	if (user.CodeGenerationAttempts >= 3 && DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalMinutes < 5)
        //	{
        //		var remainingTime = (int)Math.Ceiling(5 - DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalMinutes);
        //		ViewBag.BlockMessage = $"You have reached the maximum code generation attempts. Please try again after {remainingTime} minutes.";
        //	}
        //	if (user.EmailConfirmed)
        //	{
        //		ModelState.AddModelError("ConfirmCode", "Bu hesab artıq təsdiq olunub!");
        //		return View(confirmCodeViewModel);
        //	}
        //	if (user.ConfirmCode == confirmCodeViewModel.ConfirmCode)
        //	{

        //		user.EmailConfirmed = true;
        //		await _userManager.UpdateAsync(user);


        //		return RedirectToAction("Login", "Account");
        //	}
        //	else
        //	{
        //		ModelState.AddModelError("ConfirmCode", "Daxil edilən təsdiq kodu yalnışdır.Diqqətli daxil edin!");
        //	}

        //	return View();
        //}
        [HttpGet]
        public async Task<IActionResult> ConfirmMail(string UserName)
        {
            var value = TempData["Mail"];
            ViewBag.Mail = value;

            AppUser appUser = await _userManager.FindByNameAsync(UserName);

            if (appUser == null)
            {
                return RedirectToAction("ResendConfirmationEmail", "Account");
            }


            var remainingTime = CalculateRemainingTime(appUser.LastCodeGenerationTime);
            ViewBag.RemainingTimeForConfirmCode = remainingTime;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmMail(ConfirmCodeViewModel confirmCodeViewModel)
        {
            if (confirmCodeViewModel.Email == null)
            {
                ModelState.AddModelError("", "Email daxil edin");

                return View(confirmCodeViewModel);
            }

            var user = await _userManager.FindByEmailAsync(confirmCodeViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz elektron ünvanla heç bir istifadəçi qeydiyyatdan keçməyib!");
                return View(confirmCodeViewModel);
            }

            if (user.RemainingTimeForConfirmCode == 0 || DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalSeconds >= 120)
            {
                ModelState.AddModelError("", "Şifrə təsdiqi üçün vaxtınız bitdi. Yeni təsdiq kodu üçün istək göndərin!");
                return View(confirmCodeViewModel);
            }
            var remainingTime = CalculateRemainingTime(user.LastCodeGenerationTime);
            ViewBag.RemainingTimeForConfirmCode = remainingTime;

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("ConfirmCode", "Bu hesab artıq təsdiq olunub!");
                return View(confirmCodeViewModel);
            }

            if (user.ConfirmCode == confirmCodeViewModel.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("ConfirmCode", "Daxil edilən təsdiq kodu yalnışdır.Diqqətli daxil edin!");
            }

            return View();
        }

        private int CalculateRemainingTime(DateTime lastCodeGenerationTime)
        {
            var elapsedTime = DateTime.Now.Subtract(lastCodeGenerationTime);
            var remainingTime = 120 - (int)elapsedTime.TotalSeconds;

            if (remainingTime < 0)
            {
                remainingTime = 0;
            }

            return remainingTime;
        }



        [HttpGet]
        public IActionResult ResendConfirmationEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmail(ConfirmCodeViewModel confirmCodeViewModel)
        {
            if (confirmCodeViewModel.Email == null)
            {
                ModelState.AddModelError("", "Elektron ünvanı daxil edin!");
                return View(confirmCodeViewModel);
            }

            var user = await _userManager.FindByEmailAsync(confirmCodeViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz elektron ünvanla heç bir istifadəçi qeydiyyatdan keçməyib!");
                return View(confirmCodeViewModel);
            }


            if (user.CodeGenerationAttempts >= 3 && DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalMinutes < 5)
            {
                var remainingTime = (int)Math.Ceiling(5 - DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalMinutes);

                ViewBag.BlockMessage = $"Təsdiq kodu maksimum 3 dəfə göndərilə bilər. Yeni kodu göndərilməsi üçün {remainingTime} dəqiqə gözləməyiniz tələb olunur!";


                return View();
            }

            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Bu hesab təsdiq olunub!");
                return View(confirmCodeViewModel);
            }

            Random random = new Random();
            int confirmCode = random.Next(100000, 1000000);
            user.ConfirmCode = confirmCode;

            if (user.CodeGenerationAttempts >= 3 && DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalMinutes >= 5)
            {

                user.CodeGenerationAttempts = 0;
                await _userManager.UpdateAsync(user);
                return View(confirmCodeViewModel);
            }
            else
            {

                user.CodeGenerationAttempts++;
            }
            user.LastCodeGenerationTime = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
                MailboxAddress ConfirmAdressTo = new MailboxAddress("User", user.Email);

                mimeMessage.From.Add(ConfirmAddressFrom);
                mimeMessage.To.Add(ConfirmAdressTo);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Email ünvanı təsdiqləmək üçün yeni kodunuz: " + confirmCode;

                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = "Email təsdiq kodunuz";

                SmtpClient client = new SmtpClient();

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");

                client.Send(mimeMessage);
                client.Disconnect(true);

                TempData["Mail"] = user.Email;

                return RedirectToAction("ConfirmMail", "Account", new { userName = user.UserName });
            }

            return View();
        }

        #endregion
        #region Login/Daxil ol
        public IActionResult Login()
        {


            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (loginVM.UserName == null)
            {
                ModelState.AddModelError("", "İstifadəçi adını qeyd edin!");
                return View(loginVM);
            }
            if (loginVM.Password == null)
            {
                ModelState.AddModelError("", "Parolunuzu daxil  edin!");
                return View(loginVM);
            }
            var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Şifrəni 5 dəfə səhv yazdığınız üçün müvəqqəti olaraq bloklandı");
                return View();
            }
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginVM.UserName);


                if (user.EmailConfirmed == true)
                {

                    return RedirectToAction("AllCards", "MyAccount");
                }
                else
                {
                    ModelState.AddModelError("", "Elektron ünvanız təsdiq edilməyib!Zəhmət olmasa hesabınızı aktivləşdirin!");

                }

            }
            else
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yalnışdır!");
            }
            return View();
        }
        public async Task<IActionResult> ForgetPassword()
        {


            return View();
        }
        #endregion
        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPasswordConfirmMail()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordConfirmMail(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            Random random = new Random();
            int AccessCode = random.Next(100000, 1000000);
            AppUser appUser = new AppUser()
            {
                Email = forgetPasswordViewModel.Email,
                ResetPasswordCode = AccessCode

            };
            if (forgetPasswordViewModel.Email == null)
            {
                ModelState.AddModelError("", "Email daxil edin");
                return View(forgetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Daxil etdiyiniz elektron ünvanla heç bir istifadəçi qeydiyyatdan keçməyib!");
                return View(forgetPasswordViewModel);
            }
            if (user.Email == forgetPasswordViewModel.Email)
            {
                MimeMessage mimeMessage = new MimeMessage();
                MailboxAddress ConfirmAddressFrom = new MailboxAddress("MoneyTransport", "odisseybanks024@gmail.com");
                MailboxAddress ConfirmAdressTo = new MailboxAddress("User", appUser.Email);

                mimeMessage.From.Add(ConfirmAddressFrom);
                mimeMessage.To.Add(ConfirmAdressTo);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Şifrə təkrarını təsdiqləmək üçün kodunuz:" + AccessCode;

                mimeMessage.Body = bodyBuilder.ToMessageBody();
                mimeMessage.Subject = "Yeni Şifrə";

                SmtpClient client = new SmtpClient();

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("odisseybanks024@gmail.com", "voxryimidhytyjot");
                client.Send(mimeMessage);
                client.Disconnect(true);


                TempData["Mail"] = forgetPasswordViewModel.Email;
                user.ResetPasswordCode = AccessCode;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("ForgetPasswordAccessCode", "Account", new { email = forgetPasswordViewModel.Email });

            }
            //else
            //{
            //    ModelState.AddModelError("", "Daxil edilən təsdiq kodu yalnışdır.Diqqətli daxil edin!");
            //}



            return View();
        }
        [HttpGet]
        public IActionResult ForgetPasswordAccessCode(string email)
        {
            var value = TempData["Mail"];
            ViewBag.Email = value;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasswordAccessCode(ForgetPasswordViewModel forgetPasswordViewModel, string email)
        {

            if (forgetPasswordViewModel.Email == null)
            {
                ModelState.AddModelError("", "Email ünvanınızı daxil edin");

                return View(forgetPasswordViewModel);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Elektron ünvanızı düzgün daxil edin!");

                return View(forgetPasswordViewModel);
            }
            if (user.ResetPasswordCode == forgetPasswordViewModel.ResetCode)
            {


                await _userManager.UpdateAsync(user);


                return RedirectToAction("ResetPassword", "Account", new { email });
            }
            else
            {
                ModelState.AddModelError("", "Daxil edilən təsdiq kodu yalnışdır.Diqqətli daxil edin!");
            }


            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email)
        {
            if (email == null)
            {

                return NotFound();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(NewPasswordViewModel NewPasswordViewModel, string Email)
        {

            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                if (NewPasswordViewModel.Password.Length < 6)
                {
                    ModelState.AddModelError("", "Şifrə ən az 6 simvol uzunluğunda olmalıdır.");
                    return View();
                }
                if (!NewPasswordViewModel.Password.Any(char.IsNumber))
                {
                    ModelState.AddModelError("", "Şifrənizdə minimum 1 rəqəm olmalıdır!");
                    return View();
                }

                if (!NewPasswordViewModel.Password.Any(char.IsUpper) || !NewPasswordViewModel.Password.Any(char.IsLower))
                {
                    ModelState.AddModelError("", "Şifrədə ən az bir böyük hərf ve bir kiçik hərf olmalıdır.");

                    return View();
                }
            }

            if (NewPasswordViewModel.Password == NewPasswordViewModel.ConfirmPassword)
            {
                if (!string.IsNullOrEmpty(NewPasswordViewModel.Password))
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, NewPasswordViewModel.Password);
                    user.ResetPasswordCount += 1;
                    if (user.ResetPasswordCount > 4 && DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalDays <= 3)
                    {
                        var remainingTime = (int)Math.Ceiling(3 - DateTime.Now.Subtract(user.LastCodeGenerationTime).TotalDays);
                        ViewBag.BlockMessage = $"Şifrənizi 48  saat ərzində ancaq 3 dəfə  yeniləyə  bilərsiniz.Yenidən dəyişmək üçün qalan vaxtınız {remainingTime} gün gözləməyiniz tələb olunur!";
                        return View();
                    }
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {

                        return RedirectToAction("Login", "Account");
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

            return View(NewPasswordViewModel);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }

}







