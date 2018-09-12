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
    public class DataManagementSystemCallerServices : IDataManagementSystemCallerServices
    {
        private HttpClient client = new HttpClient();

        public DataManagementSystemCallerServices()
        {
            client.BaseAddress = new System.Uri("http://localhost:5000");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> PostAsync(string jsonString)
        {
            await Task.Delay(500); // delay 500 ms for cancelation token to work
            CancellationToken.None.ThrowIfCancellationRequested();

            HttpResponseMessage response = await client.PostAsync("api/receive", new StringContent(jsonString, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return false;

            var stringResult = await response.Content.ReadAsStringAsync();
            var answer = JsonConvert.DeserializeObject<DataManagementSystemAnswerModel>(stringResult);
            if (answer == null) return false;
            return answer.Ok;
        }
    }
}
