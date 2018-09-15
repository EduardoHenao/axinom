using ControlPanel.IServices;
using ControlPanel.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlPanel.Services
{
    /*
     * this sevice handles the actual call to the Data Management system API.
     */
    public class DataManagementSystemCallerServices : IDataManagementSystemCallerServices
    {
        private const string SecurityFieldName = "Authorization";
        private readonly ILogger _logger;

        public DataManagementSystemCallerServices(ILogger<DataManagementSystemCallerServices> logger)
        {
            _logger = logger;
        }

        public async Task<bool> PostAsync(string remoteUrl, string jsonString, string encryptedUser, string encryptedPassword)
        {
            // cancelation token
            await Task.Delay(500); // delay 500 ms for cancelling token to work
            CancellationToken.None.ThrowIfCancellationRequested();

            // configure client object
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(remoteUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation(SecurityFieldName, $"{encryptedUser} {encryptedPassword}"); // inject security header

            // post
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync("api/receive", new StringContent(jsonString, Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException e)
            {
                _logger.LogCritical(e.Message);
                return false;
            }

            // manage answer
            if (response != null && !response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
