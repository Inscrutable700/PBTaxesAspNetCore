using System;
using System.Collections.Generic;
using PBTaxesAspNetCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PBTaxesAspNetCore.Helpers;
using PBTaxesAspNetCore.Interfaces;
using PBTaxesAspNetCore.Managers;
using PBTaxesAspNetCore.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace PBTaxesAspNetCore.Controllers
{
    public class TaxesController : Controller
    {
        private IPrivatBankManager privatBankManager;

        private PrivatBankConfig privatBankConfig;

        public TaxesController(IOptions<PrivatBankConfig> pbConfig){
            this.privatBankConfig = pbConfig.Value;
            this.privatBankManager = new PrivatBankManager();
        }

        /// <summary>
        /// The generate taxes page.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> Index()
        {
            TaxesViewModel model = new TaxesViewModel();
            var identity = (await HttpContext.Authentication.GetAuthenticateInfoAsync("Cookies")).Principal;
            var sessionID = identity.Identity.Name;
            var taxes = await this.privatBankManager
                .GetTaxesAsync(sessionID, DateTime.Parse("04.01.2016"), DateTime.Parse("06.30.2016"), 5);

            model.FullAmount = taxes.FullAmount;
            model.StartDate = taxes.StartDate;
            model.EndDate = taxes.EndDate;
            model.TaxesAmount = taxes.TaxesAmount;
            model.InterestRate = taxes.InterestRate;

            List<TaxesViewModel.Statement> credits = new List<TaxesViewModel.Statement>();
            foreach (var statement in taxes.Statements)
            {
                credits.Add(new TaxesViewModel.Statement()
                {
                    Amount = statement.Amount.Amount,
                    Description = statement.Purpose,
                    IsTaxed = statement.IsTaxed,
                    CCY = statement.Amount.CCY,
                    Date = statement.Info.PostDate,
                    CurrencyExchange = statement.Course,
                    AmountInUAH = statement.AmountInUAH,
                });
            }

            model.Statements = credits.ToArray();
            
            return View(model);
        }
    }
}
