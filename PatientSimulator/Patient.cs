using Microsoft.Extensions.Hosting;
using System.Text;

namespace PatientSimulator
{
    public class Patient : BackgroundService
    {
        private HttpClient _httpClient;

        private string baseUrl = "https://localhost:7124";
        private string patientEndpoint = "/patient";
        private string vitalSignsEndpoint = "/vitalSigns";

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            var newPatient = patientEndpoint + "?name=John Doe";
            var response = await _httpClient.PostAsync(newPatient, null, stoppingToken);
            var patientId = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Added patient with ID {patientId}");

            while (!stoppingToken.IsCancellationRequested)
            {
                var newValue = new Random().Next(0, 100);
                var newVitalSignType = new Random().Next(0, 4);

                var vitalSignObject = $"{{\"Type\": {newVitalSignType}," +
                    $"\"Value\": {newValue}," +
                    $"\"MeasurementTimestamp\": \"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}\"," +
                    $"\"PatientId\": {patientId}}}";

                var stringContent = new StringContent(vitalSignObject, UnicodeEncoding.UTF8, "application/json");

                var newVitalSign = await _httpClient.PostAsync(vitalSignsEndpoint, stringContent, stoppingToken);
                Console.WriteLine($"Recorded new vital sign {newVitalSign}");

                await Task.Delay(2000, stoppingToken);
            }

            _httpClient.Dispose();
            await Task.CompletedTask;
        }
    }
}
