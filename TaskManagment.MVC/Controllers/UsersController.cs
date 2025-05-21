using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using TaskManagment.Core.Abstractions;
using TaskManagment.Core.ViewModel.User;
using TaskManagment.Mvc.Filters;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace TaskManagment.Mvc.Controllers
{
    [Authorize(Roles = DefaultRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _userManager.Users.ToListAsync();
            var userVM = model.Adapt<IEnumerable<UserViewModel>>();
            return View(userVM);
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var UserVM = new UserFormViewModel
            {
                Roles = await _roleManager.Roles
                                          .Select(r => new SelectListItem
                                          {
                                              Text = r.Name,
                                              Value = r.Name
                                          }).ToListAsync()
            };
            return PartialView("_Form", UserVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel userFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            ApplicationUser user = new()
            {
                FullName = userFormViewModel.FullName,
                UserName = userFormViewModel.UserName,
                Email = userFormViewModel.Email,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                CreatedOn = DateTime.Now,
            };
            var result = await _userManager.CreateAsync(user, userFormViewModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userFormViewModel.SelectedRoles);

                //Code To Converm 

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code, },
                    protocol: Request.Scheme);

                var EmailPath = $"{Directory.GetCurrentDirectory()}/Templates/email.html";
                StreamReader stream = new(EmailPath);
                var body = await stream.ReadToEndAsync();
                stream.Close();
                body = body.Replace("[imageUrl]", "https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg")
                           .Replace("[header]", $"Hey : {user.FullName}")
                           .Replace("[body]", "Please Confirm Your Email")
                           .Replace("[url]", HtmlEncoder.Default.Encode(callbackUrl!))
                           .Replace("[logo]", $"{_httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString()}/assets/images/Logo.png")
                           .Replace("[linkTitle]", "Active Account!");

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);

                var userVM = user.Adapt<UserViewModel>();
                return PartialView("_UsersRow", userVM);
            }
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _userManager.UpdateAsync(user);
            if (user.IsDeleted)
                await _userManager.UpdateSecurityStampAsync(user);

            return Ok(user.LastUpdatedOn.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var isLocked = await _userManager.IsLockedOutAsync(user);

            if (isLocked)
                await _userManager.SetLockoutEndDateAsync(user, null);

            return Ok();
        }
        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> ResetPassword(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }
            var ResetPasswordVM = new ResetPasswordFormViewModel { Id = Id };
            return PartialView("_resetPassword", ResetPasswordVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormViewModel resetPasswordVM)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordVM.Id);
            if (user is null)
            {
                return NotFound();
            }
            var currentPasswordHash = user.PasswordHash;

            await _userManager.RemovePasswordAsync(user);

            var result = await _userManager.AddPasswordAsync(user, resetPasswordVM.Password);

            if (result.Succeeded)
            {
                user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                user.LastUpdatedOn = DateTime.Now;
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
                var viewModel = user.Adapt<UserViewModel>();
                return PartialView("_UsersRow", viewModel);
            }

            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);

            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }
        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
            {
                return NotFound();
            }

            var userVM = user.Adapt<UserFormViewModel>();
            userVM.SelectedRoles = await _userManager.GetRolesAsync(user);
            userVM.Roles = await _roleManager.Roles
                                          .Select(r => new SelectListItem
                                          {
                                              Text = r.Name,
                                              Value = r.Name
                                          }).ToListAsync();
            return PartialView("_Form", userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel userVM)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userVM.Id);

            if (user is null)
                return NotFound();

            user = userVM.Adapt(user);
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            user.LastUpdatedOn = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                var rolesUpdated = !currentRoles.SequenceEqual(userVM.SelectedRoles);

                if (rolesUpdated)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRolesAsync(user, userVM.SelectedRoles);
                }

                await _userManager.UpdateSecurityStampAsync(user);
                var viewModel = user.Adapt<UserViewModel>();
                return PartialView("_UsersRow", viewModel);
            }

            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }
        public async Task<IActionResult> AllowUserName(UserFormViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isAllowed = user is null || user.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        public async Task<IActionResult> AllowEmail(UserFormViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isAllowed = user is null || user.Id.Equals(model.Id);

            return Json(isAllowed);
        }
    }
}
