using AutoMapper;
using EduNexBL.DTOs.PaymentDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EduNexAPI.Controllers
{
    public class PaymobAuthenticationRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        internal string authToken = String.Empty;
        public PaymobAuthenticationRequestController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        //[Microsoft.AspNetCore.Mvc.HttpPost("PostPaymob")]
        public async Task<BaseResponseWithDataModel<string>> GetPaymobToken()
        {
            var AuthResponse = new BaseResponseWithDataModel<string>();
            AuthResponse.ErrorMsg = "";

            try
            {
                // Paymob API key
                string apiKey = _configuration["PaymobKey:ApiKey"];

                // Create JSON object with API key
                var requestData = new
                {
                    api_key = apiKey
                };

                // Convert object to JSON string
                string jsonRequest = JsonConvert.SerializeObject(requestData);

                // Create StringContent with JSON data
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                // Set the base address for this request
                _client.BaseAddress = new Uri("https://accept.paymob.com/api/");

                // Send POST request to Paymob authentication endpoint
                HttpResponseMessage response = await _client.PostAsync("auth/tokens", content);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response to extract token
                    dynamic responseObject = JsonConvert.DeserializeObject<AuthResponseModel>(responseContent);
                    string token = responseObject.token;
                    authToken = responseObject.token;
                    // Return the token
                    AuthResponse.Data = token;
                    return AuthResponse;
                }
                else
                {
                    // Handle error response from Paymob API
                    string errorMessage = $"Paymob API request failed with status code: {response.StatusCode}";
                    AuthResponse.ErrorMsg = errorMessage;
                    return AuthResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                AuthResponse.ErrorMsg = ex.Message;
                return AuthResponse;
            }
        }
        //[Microsoft.AspNetCore.Mvc.HttpPost("CreateOrder")]
        public async Task<BaseResponseWithDataModel<string>> CreateOrder(OrderRequestDTO orderRequest, int price)
        {
            BaseResponseWithDataModel<string> idResponse = new BaseResponseWithDataModel<string>();
            try
            {
                // Get the Paymob token
                BaseResponseWithDataModel<string> tokenResponse = await GetPaymobToken();

                if (!string.IsNullOrEmpty(tokenResponse.ErrorMsg))
                {
                    string errorMessage = $"Paymob API request failed with status code: {tokenResponse.ErrorMsg}";
                    tokenResponse.ErrorMsg = errorMessage;
                    return tokenResponse;
                }

                string token = tokenResponse.Data;

                // Prepare the order request data
                var requestID = new
                {
                    auth_token = token,
                    delivery_needed = orderRequest.DeliveryNeeded,
                    amount_cents = price,
                    currency = orderRequest.Currency,
                    items = orderRequest.Items
                };

                // Convert object to JSON string
                string jsonRequest = JsonConvert.SerializeObject(requestID);

                // Create StringContent with JSON data
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                // Create a new HttpClient instance
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://accept.paymob.com/api/ecommerce/");

                // Send POST request to Paymob order creation endpoint
                HttpResponseMessage response = await client.PostAsync("orders", content);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response to extract token
                    dynamic responseObject = JsonConvert.DeserializeObject<OrderResponseModel>(responseContent);
                    int id = responseObject.id;
                    idResponse.Data = id.ToString();
                    // Return success status
                    return idResponse;
                }
                else
                {
                    // Handle error response from Paymob API
                    string errorMessage = $"Paymob order creation failed with status code: {response.StatusCode}";
                    idResponse.ErrorMsg = errorMessage;
                    return idResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                idResponse.ErrorMsg = ex.Message;
                return idResponse;
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("GetPaymentKey/{price}")]
        public async Task<BaseResponseWithDataModel<string>> GetPaymentKey(PaymentKeyRequestDTO paymentKeyRequest, int price)
        {
            BaseResponseWithDataModel<string> paymentKeyResponse = new BaseResponseWithDataModel<string>();
            var orderRequest = new OrderRequestDTO();
            var amountCents = price * 100;
            try
            {
                // Get the Paymob token
                BaseResponseWithDataModel<string> idResponse = await CreateOrder(orderRequest, amountCents);

                if (!string.IsNullOrEmpty(idResponse.ErrorMsg))
                {
                    string errorMessage = $"Paymob API request failed with status code: {idResponse.ErrorMsg}";
                    idResponse.ErrorMsg = errorMessage;
                    return idResponse;
                }

                string id = idResponse.Data;

                // Prepare the order request data
                var requestKey = new
                {
                    auth_token = authToken,
                    amount_cents = amountCents,
                    expiration = paymentKeyRequest.expiration,
                    order_id = int.Parse(id),
                    billing_data = paymentKeyRequest.billing_data,
                    currency = paymentKeyRequest.currency,
                    integration_id = paymentKeyRequest.integration_id
                };

                // Convert object to JSON string
                string jsonRequest = JsonConvert.SerializeObject(requestKey);

                // Create StringContent with JSON data
                var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                // Create a new HttpClient instance
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://accept.paymob.com/api/acceptance/");

                // Send POST request to Paymob order creation endpoint
                HttpResponseMessage response = await client.PostAsync("payment_keys", content);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response to extract token
                    dynamic responseObject = JsonConvert.DeserializeObject<KeyResponseModel>(responseContent);
                    string paymentKey = responseObject.token;
                    paymentKeyResponse.Data = "https://accept.paymob.com/api/acceptance/iframes/838672?payment_token=" + paymentKey;
                    // Return success status
                    return paymentKeyResponse;
                }
                else
                {
                    // Handle error response from Paymob API
                    string errorMessage = $"Paymob order creation failed with status code: {response.StatusCode}";
                    paymentKeyResponse.ErrorMsg = errorMessage;
                    return paymentKeyResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                paymentKeyResponse.ErrorMsg = ex.Message;
                return paymentKeyResponse;
            }
        }
    }
}
