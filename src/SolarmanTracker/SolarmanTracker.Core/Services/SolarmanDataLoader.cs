using Amazon.Lambda.Core;
using SolarmanTracker.Core.DataModel.DataLoaderModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SolarmanTracker.Core.Services
{
    public sealed class SolarmanDataLoader(ILambdaLogger logger)
    {
        private readonly HttpClient client = new HttpClient();

        private const string RealTimeStationUrl = "https://globalapi.solarmanpv.com/station/v1.0/realTime?language=en";

        public async Task<string> LoadRealTimeStationData(int stationId, string accessToken)
        {
            var body = new RealTimeStationRequest
            {
                stationId = stationId
            };
            return await LoadData(RealTimeStationUrl, accessToken, body);
        }

        private async Task<string> LoadData(string url, string accessToken, object body)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                request.Content = JsonContent.Create(body);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error loading data from {url}: {ex.Message}");
                throw;
            }
        }
    }
}
