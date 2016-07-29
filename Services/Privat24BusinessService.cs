using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBTaxesAspNetCore.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaxesPrivatBank.Business.Services
{
    /// <summary>
    /// The privat24 client service.
    /// </summary>
    public class Privat24BusinessService : ServiceBase
    {
        /// <summary>
        /// The privat bank API URL.
        /// </summary>
        private const string ApiUrl = "https://link.privatbank.ua/";

        /// <summary>
        /// The client identifier.
        /// </summary>
        private string clientID;

        /// <summary>
        /// The client secret.
        /// </summary>
        private string clientSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="Privat24BusinessService" /> class.
        /// </summary>
        /// <param name="clientID">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        public Privat24BusinessService(string clientID, string clientSecret)
            : base(ApiUrl)
        {
            this.clientID = clientID;
            this.clientSecret = clientSecret;
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <param name="clientID">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <returns>The session.</returns>
        public Task<PBSessionDto> GetSessionAsync()
        {
            string apiEndpoint = "api/auth/createSession";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "clientId", this.clientID },
                { "clientSecret", this.clientSecret},
            };

            return this.GetPOSTResponseAsync<PBSessionDto>(apiEndpoint, parameters);
        }

        /// <summary>
        /// Gets the person session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>The person session.</returns>
        public Task<PBPersonSessionDto> GetPersonSessionAsync(string sessionId, string login, string password)
        {
            string apiEndpoint = "api/p24BusinessAuth/createSession";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "sessionId", sessionId },
                { "login", login},
                { "password", password }
            };

            return this.GetPOSTResponseAsync<PBPersonSessionDto>(apiEndpoint, parameters);
        }

        public async Task<PBPersonSessionDto> GetPersonSessionAsyncTest(string sessionId, string login, string password)
        {
            string apiEndpoint = "api/p24BusinessAuth/createSession";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "sessionId", sessionId },
                { "login", login},
                { "password", password }
            };

            string test = await this.GetPOSTResponseStringAsync(apiEndpoint, parameters);
            //string test = "{\"id\":\"session-id\",\"clientId\":\"client-id\",\"expiresIn\":1418703284,\"message\":[{\"id\":\"1111111\",\"number\":\"050...111\"},{\"id\":\"1111112\",\"number\":\"068...112\"},{\"id\":\"1111112\",\"number\":\"095...113\"}],\"roles\":[\"ROLE_CLIENT\"]}";
            dynamic test1 = JsonConvert.DeserializeObject<dynamic>(test);
            PBPersonSessionDto personSessionDto = new PBPersonSessionDto();
            personSessionDto.ID = test1.id;
            personSessionDto.ClientId = test1.clientId;
            personSessionDto.ExpiresIn = test1.expiresIn;

            if (test1.message.Type == JTokenType.String)
            {
                personSessionDto.Message = test1.message;
            }
            else if(test1.message.Type == JTokenType.Array)
            {
                foreach(dynamic test2 in test1.message)
                {
                    personSessionDto.PhoneNumbers.Add(new PBPersonSessionDto.PhoneNumber()
                    {
                        Id = test2.id,
                        Number = test2.number,
                    });
                }
            }
            else
            {
                throw new Exception("Something went wrong with PB API.");
            }

            return personSessionDto;
        }

        public Task<PBPersonSessionDto> SelectNumber(string sessionId, string numberId)
        {
            string apiEndpoint = "/api/p24BusinessAuth/sendOtp";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "sessionId", sessionId },
                { "otpDev", numberId},
            };

            return this.GetPOSTResponseAsync<PBPersonSessionDto>(apiEndpoint, parameters);
        }

        /// <summary>
        /// Confirms the SMS code.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="code">The code.</param>
        /// <returns>The person session.</returns>
        public Task<PBPersonSessionDto> ConfirmSmsCodeAsync(string sessionId, string code)
        {
            string apiEndpoint = "api/p24BusinessAuth/checkOtp";
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "sessionId", sessionId },
                { "otp", code},
            };

            return this.GetPOSTResponseAsync<PBPersonSessionDto>(apiEndpoint, parameters);
        }

        /// <summary>
        /// Gets the statements.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <returns></returns>
        public Task<List<PBStatementItemDto>> GetStatementsAsync(string sessionID, DateTime startDate, DateTime endDate)
        {
            string apiEndPoint = "api/p24b/statements";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "stdate", startDate.ToString("dd.MM.yyyy") },
                { "endate", endDate.ToString("dd.MM.yyyy")},
                { "showInf", null }
            };

            return this.GetGETResponseAsync<List<PBStatementItemDto>>(apiEndPoint, parameters, $"Token {sessionID}");
        }
    }
}
