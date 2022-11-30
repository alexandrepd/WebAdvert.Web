using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Account;
using WebAdvert.Web.Models.Account.ForgetPassword;

namespace WebAdvert.Web.Controllers
{

    public class AccountController : Controller
    {

        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoUserPool;
        public AccountController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
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
                    return RedirectToAction("Index", "Home");
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

        public IActionResult Login()
        {
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var _result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
                if (_result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Email and Password do not match");
                }
            }

            return View(loginModel);
        }


        public IActionResult ForgetPassword()
        {
            ForgetPasswordModel forgetPasswordModel = new ForgetPasswordModel();
            return View("ForgetPassword/ForgetPassword");
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var _user = await _userManager.FindByEmailAsync(forgetPasswordModel.Email);

                if (_user != null)
                {
                    await _user.ForgotPasswordAsync();
                    return View("ForgetPassword/ConfirmForgetPassword");
                }
                else
                {
                    ModelState.AddModelError("EmailNotFound", "Email not found");
                }
            }
            return View("ForgetPassword/ForgetPassword");
        }

        public IActionResult ConfirmForgetPassword()
        {
            ConfirmForgetPasswordModel forgetPasswordModel = new ConfirmForgetPasswordModel();
            return View("ForgetPassword/ConfirmForgetPassword", forgetPasswordModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmForgetPassword(ConfirmForgetPasswordModel confirmForgetPasswordModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var _user = await _userManager.FindByEmailAsync(confirmForgetPasswordModel.Email);

                    if (_user != null)
                    {
                        await _user.ConfirmForgotPasswordAsync(confirmForgetPasswordModel.Code, confirmForgetPasswordModel.Password);

                        if (_user.Status != null)
                            return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("ForgetError", "Erro while forget password");
                    }
                }
                return View("ForgetPassword/ConfirmForgetPassword");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                throw;
            }
        }

    }
}
