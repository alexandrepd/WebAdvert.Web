using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class AccountsController : Controller
    {

        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;
        public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }


        public IActionResult Signup()
        {
            SignupModel model = new SignupModel();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                CognitoUser user = _cognitoUserPool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exists");
                    return View(model);
                }

                user.Attributes.Add("name", model.Email);
                var createdUser = await _userManager.CreateAsync(user, model.Password);


                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Confirm");
                }
                else
                {
                    foreach (var item in createdUser.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);

                    }
                    return View(model);
                }
            }
            return View();
        }

        public IActionResult Confirm()
        {
            ConfirmModel model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.FindByEmailAsync(model.Email);
                if (_user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with the given email address was not found");
                    return View(model);
                }

                var _result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(_user, model.code, true);

                if (_result.Succeeded)
                {
                  return  RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in _result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);

                    }
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
