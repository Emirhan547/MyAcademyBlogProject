using AutoMapper;
using Blogy.Business.DTOs.UserDtos;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        // PROFIL INDEX
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var dto = _mapper.Map<EditProfileDto>(user);
            return View(dto);
        }

        // PROFIL GÜNCELLEME
        [HttpPost]
        public async Task<IActionResult> Index(EditProfileDto model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);
            TempData["PasswordSuccess"] = "Profil bilgileriniz güncellendi.";

            return RedirectToAction("Index");
        }

        // ŞIFRE DEĞİŞTIRME
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                TempData["PasswordError"] = "Yeni şifreler birbiriyle uyuşmuyor.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);

            if (!result.Succeeded)
            {
                TempData["PasswordError"] = "Mevcut şifre yanlış veya şifre gereksinimleri karşılanmıyor.";
                return RedirectToAction("Index");
            }

            TempData["PasswordSuccess"] = "Şifreniz başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
