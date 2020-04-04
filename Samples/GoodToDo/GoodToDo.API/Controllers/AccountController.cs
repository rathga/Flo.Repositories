using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flo;
using GoodToDo.API.Users;
using System.ComponentModel.DataAnnotations;

namespace GoodToDo.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public partial class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> Login(string email, string password)
        {
            if (email == null || password == null) return Result.Fail("Invalid login attempt.");

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Result.Ok();
            }
            if (result.IsLockedOut)
            {
                return Result.Fail("User is locked out");
            }
            else
            {
                return Result.Fail("Invalid login attempt.");
            }
        }

        // Data annotations validation function
        private Result Validate(object model)
        {
            Result result = Result.Ok();

            // perform basic model validation based on data annotations
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results);

            if (!isValid)
            {
                result.Combine(Result.Fail(results.Select(vr => new Error { Key = vr.MemberNames.FirstOrDefault(), Message = vr.ErrorMessage }).ToArray()));
            }

            return result;
        }
  
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result> Register(RegisterViewModel registerViewModel)
        {
            var valResult = Validate(registerViewModel);
            if (valResult.Failure) return valResult;

            var user = new User { Email = registerViewModel.Email };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Result.Ok();
            }
            else
            {
                return Result.Fail(result.Errors.Select(e => new Error { Key = mapIdentityErrorCode(e.Code), Message = e.Description }).ToArray());
            }

        }

        // helper function to map Identity codes to viewmodel keys
        private string mapIdentityErrorCode(string c)
        {
            if (c.StartsWith("Password")) return "Password";
            if (c.EndsWith("UserName")) return "Email";
            return c;
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public async Task LogOff()
        {
            await _signInManager.SignOutAsync();
        }

        //
        // GET: /Account/GetCurrentUserId
        [HttpGet]
        public async Task<Guid> GetCurrentUserId()
        {
            return (await GetCurrentUserAsync()).Id;
        }
        
        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }


    }
}
