using Ishop.Application.Interfaces;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class PaymentManagementService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> MakeChargeRequest(Payment request)
        {
            try
            {
                // Replace with your Flutterwave API key
                var apiKey = "FLWSECK_TEST-7738da366cd9f25b638dd6189badbd9c-X";

                // Replace with your Flutterwave API endpoint
                var apiUrl = "https://api.flutterwave.com/v3/charges?type=mobile_money_uganda";

                // Set the Authorization header with your Flutterwave API key
                _httpClient.DefaultRequestHeaders.Clear(); // Clear headers to avoid duplicates
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Ensure that the required parameters are set in the request body
                var requestBody = new
                {
                    amount = request.Amount,
                    tx_ref = request.Tx_ref,
                    email = request.Email,
                    phone_number = request.Phone_number,
                    currency = request.Currency,
                    // Add other properties as needed
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                Console.WriteLine($"Request Content: {await content.ReadAsStringAsync()}");

                // Make the POST request to the Flutterwave API
                var response = await _httpClient.PostAsync(apiUrl, content);

                // Check if the request was successful (HTTP status code 2xx)
                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response content as a string
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");
                    return responseContent;
                }
                else
                {
                    // Log the error and handle the response content
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Response Content: {responseContent}");

                    return $"Error: {response.StatusCode} - {response.ReasonPhrase} - {responseContent}";
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected exceptions
                Console.WriteLine($"Exception: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }
    }
}
