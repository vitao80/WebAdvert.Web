using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class Account : Controller
    {
        readonly SignInManager<CognitoUser> _signInManager;
        readonly UserManager<CognitoUser> _userManager;
        readonly CognitoUserPool _pool;

        public Account(SignInManager<CognitoUser> signInManager,
            UserManager<CognitoUser> userManager,
            CognitoUserPool pool) 
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
        }

        public async Task<IActionResult> Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel  signupModel)
        {
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(signupModel.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "Usuario já existe");
                    return View(signupModel);
                }

                user.Attributes.Add("name", signupModel.Email);
                var createdUser = await _userManager.CreateAsync(user, signupModel.Password);
                if (createdUser.Succeeded)
                {
                    var confirmModel = new ConfirmModel
                    {
                        Email = signupModel.Email,
                        Code = string.Empty
                    };
                    return RedirectToAction("Confirm", "Account", confirmModel);
                } else
                {
                    if (createdUser.Errors != null)
                    {
                        foreach (var erro in createdUser.Errors)
                        {
                            ModelState.AddModelError(erro.Code, erro.Description);
                        }
                    }
                    return View(signupModel);
                }
            }
            return View();
        }

        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (model == null)
            {
                model = new ConfirmModel();
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> Confirm_Post(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) { 
                     ModelState.AddModelError("NotFound", "E-mail do usuário não foi encontrado");
                    return View(model);
                }
                //var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var erro in result.Errors)
                    {
                        ModelState.AddModelError(erro.Code, erro.Description);
                    }

                    return View(model);
                }
                
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Email and password do not match");
                }
            }

            return View("Login", model);
        }

        public async Task<IActionResult> Signout()
        {
            if (User.Identity.IsAuthenticated) await _signInManager.SignOutAsync().ConfigureAwait(false);
            return RedirectToAction("Login");
        }
    }

}
