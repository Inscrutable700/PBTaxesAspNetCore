using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBTaxesAspNetCore.Dto;
using PBTaxesAspNetCore.Helpers;
using PBTaxesAspNetCore.Interfaces;
using TaxesPrivatBank.Business.Services;

namespace PBTaxesAspNetCore.Managers
{
    /// <summary>
    /// The privat bank manager.
    /// </summary>
    public class PrivatBankManager : ManagerBase, IPrivatBankManager
    {
        /// <summary>
        /// The privat24 business service.
        /// </summary>
        private Privat24BusinessService privat24BusinessService;

        /// <summary>
        /// The privat24 service.
        /// </summary>
        private Privat24Service privat24Service; 

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivatBankManager"/> class.
        /// </summary>
        public PrivatBankManager()
        {
            this.privat24BusinessService = new Privat24BusinessService(ConfigurationHelper.PBClientID, ConfigurationHelper.PBClientSecret);
            this.privat24Service = new Privat24Service();
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <returns>
        /// The session.
        /// </returns>
        public Task<PBSessionDto> GetSessionAsync()
        {
            return this.privat24BusinessService.GetSessionAsync();
        }

        /// <summary>
        /// Gets the person session.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The person session.
        /// </returns>
        public Task<PBPersonSessionDto> GetPersonSessionAsync(string sessionID, string login, string password)
        {
            return this.privat24BusinessService.GetPersonSessionAsync(sessionID, login, password);
        }

        /// <summary>
        /// Confirms the SMS code.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="code">The code.</param>
        /// <returns>The person session.</returns>
        public Task<PBPersonSessionDto> ConfirmSmsCodeAsync(string sessionId, string code)
        {
            return this.privat24BusinessService.ConfirmSmsCodeAsync(sessionId, code);
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="interestRate">The interest rate.</param>
        /// <returns></returns>
        public async Task<TaxesDto> GetTaxesAsync(string sessionID, DateTime startDate, DateTime endDate, double interestRate)
        {
            TaxesDto taxes = new TaxesDto()
            {
                StartDate = startDate,
                EndDate = endDate,
            };

            List<PBStatementItemDto> statements = await this.privat24BusinessService.GetStatementsAsync(sessionID, startDate, endDate);

            var inputStatements = statements
                .Where(d => d.Info.ShortType == "C")
                .Where(d => d.Amount.Amount >= 0 && d.Credit.Account.Number.StartsWith("2600") || d.Amount.Amount < 0 && d.Debet.Account.Number.StartsWith("2600"))
                .OrderBy(d => d.Info.PostDate)
                .ToList();

            foreach (var inputStatement in inputStatements)
            {
                inputStatement.IsTaxed = !inputStatement.Purpose.StartsWith("Гривнi вiд вiльного продажу");
                if (inputStatement.Amount.CCY.Equals("UAH"))
                {
                    inputStatement.Course = 1;
                }
                else
                {
                    ExchangeRateDto excangeRate = await this.privat24Service
                        .GetExchangeRate(inputStatement.Amount.CCY, inputStatement.Info.PostDate);
                    if (excangeRate != null)
                    {
                        inputStatement.Course = excangeRate.PurchaseRateNB;
                    }
                }

                inputStatement.AmountInUAH = inputStatement.Course * inputStatement.Amount.Amount;

                if (inputStatement.IsTaxed)
                {
                    taxes.FullAmount += inputStatement.AmountInUAH;
                }
            }

            taxes.TaxesAmount = (interestRate / 100) * taxes.FullAmount;
            taxes.InterestRate = interestRate;
            taxes.Statements = inputStatements;


            return taxes;
        }
    }
}
