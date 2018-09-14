using FileLoader.IServices;
using FileLoader.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileLoader.Services
{
    /*
     * this sevice handles the actual call to the Data Management system API.
     */
    public class DataManagementSystemCallerServices : IDataManagementSystemCallerServices
    {
        private const string SecurityFieldName = "Authorization";

        public DataManagementSystemCallerServices()
        {
            
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
                // yeah i know i should inquire about the excetion, but that maybe when u guys hire me ;)
                return false;
            }

            // manage answer
            if (response != null && !response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
