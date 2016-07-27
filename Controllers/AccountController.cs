using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBTaxesAspNetCore.Dto;
using PBTaxesAspNetCore.Helpers;
using PBTaxesAspNetCore.Interfaces;
using PBTaxesAspNetCore.Managers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PBTaxesAspNetCore.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// The privat bank manager
        /// </summary>
        private IPrivatBankManager privatBankManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
            this.privatBankManager = new PrivatBankManager();
        }

        // GET: Account
        public ActionResult LoginToPB()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginToPB(string login, string password)
        {
            ActionResult result = null;
            PBSessionDto session = await this.privatBankManager.GetSessionAsync();
            PBPersonSessionDto personSession = await this.privatBankManager
                .GetPersonSessionAsync(session.ID, login, password);
            if (personSession.Message.StartsWith("Authentication successful"))
            {
                this.Login(personSession.ID);
                result = this.RedirectToAction("Index", "Home", null);
            }
            else
            {
                CookieHelper.PBSessionID = personSession.ID;
                result = this.RedirectToAction("ConfirmCode", "Account");
            }

            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> LogoutFromPB()
        {
            await this.HttpContext.Authentication.SignOutAsync("Cookies");
            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmCode()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmCode(string code)
        {
            string sessionID = CookieHelper.PBSessionID;
            PBPersonSessionDto personSession = await this.privatBankManager.ConfirmSmsCodeAsync(sessionID, code);
            if (personSession != null)
            {
                this.Login(personSession.ID);
            }

            return this.RedirectToAction("Index", "Home", null);
        }

        private async void Login(string pbTokenID)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, pbTokenID)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }
    }
}
