using FileLoader.IServices;
using FileLoader.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
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
            // configure client object
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(remoteUrl);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation(SecurityFieldName, $"{encryptedUser} {encryptedPassword}");//inject security header

            // post
            await Task.Delay(500); // delay 500 ms for cancelling token to work
            CancellationToken.None.ThrowIfCancellationRequested();
            HttpResponseMessage response = await client.PostAsync("api/receive", new StringContent(jsonString, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return false;

            // manage answer
            var stringResult = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<DataManagementSystemAnswerModel>(stringResult);
            if (answer == null) return false;
            return answer.Ok;
        }
    }
}
