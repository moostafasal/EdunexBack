using AutoMapper;
using EduNexBL.Repository;
using EduNexBL.DTOs.PaymentDTOs;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EduNexBL.IRepository;

namespace EduNexAPI.Controllers
{
    public class PaymobAuthenticationRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly IWallet _walletRepo;
        private readonly ITransaction _TransactionRepo;
        internal string authToken = String.Empty;
        internal string created_at = String.Empty;
        internal string payment_method = String.Empty;
        public PaymobAuthenticationRequestController(IConfiguration configuration, IMapper mapper, IWallet walletRepo, ITransaction transactionRepo)
        {
            _configuration = configuration;
            _mapper = mapper;
            _walletRepo = walletRepo;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _TransactionRepo = transactionRepo;
        }

        private async Task UpdateWalletBalance(string userId, int amount)
        {
            try
            {
                // Get the user's wallet from the repository
                var userWallet = await _walletRepo.GetById(userId);

                if (userWallet != null)
                {
                    // Update the wallet balance
                    userWallet.Balance += amount;

                    // Update the wallet in the repository
                    await _walletRepo.UpdateWallet(userWallet);
                }
                else
                {
                    // Handle scenario where user's wallet is not found
                    Console.WriteLine($"User's wallet with ID {userId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                throw new Exception(ex.Message);
            }
        }

        private async Task CreateTransaction(string userId, int amount) 
        {
            try
            {
                TransactionDTO transactionDTO = new TransactionDTO();
                var wallet = await _walletRepo.GetById(userId); 
                if (wallet != null) 
                {
                    // Create a new transaction WalletId
                    transactionDTO = new TransactionDTO()
                    {
                        WalletId = wallet.WalletId,
                        TransactionType = payment_method,
                        Amount = (decimal)amount,
                        TransactionDate = created_at
                    };

                    // Map the DTO to the entity
                    var transactionEntity = _mapper.Map<Transaction>(transactionDTO);

                    // Add the transaction to the repository
                    await _TransactionRepo.Add(transactionEntity);

                    //Read the transaction
                    Console.WriteLine($"{transactionDTO.WalletId},{transactionDTO.TransactionType},{transactionDTO.TransactionDate},{transactionDTO.Amount}");
                }
                else
                {
                    Console.WriteLine($"Couldn't find the transaction related to user: {userId}");
                }               
            }
            catch (Exception ex)        
            {
                // Handle any exceptions
                throw new Exception(ex.Message);
            }
        }

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
                    created_at = responseObject.created_at;
                    payment_method = responseObject.payment_method;
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
        
        [HttpPost("GetPaymentKey")]
        public async Task<BaseResponseWithDataModel<string>> GetPaymentKey(PaymentKeyRequestDTO paymentKeyRequest, int price, string userId)
        {
            BaseResponseWithDataModel<string> paymentKeyResponse = new BaseResponseWithDataModel<string>();
            var orderRequest = new OrderRequestDTO();
            var amountCents = price * 100;
            var userID = userId;
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

                    await UpdateWalletBalance(userID, price);
                    await CreateTransaction(userID, price);

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

        [HttpGet("GetWalletBalance")]
        public async Task<IActionResult> GetWalletBalance(string ownerId, string ownerType)
        {
            try
            {
                Wallet displayedWallet = await _walletRepo.GetByIdAndOwnerType(ownerId, ownerType);

                if (displayedWallet != null)
                {
                    return Ok(new { Balance = displayedWallet.Balance });
                }
                else
                {
                    return NotFound($"Wallet not found for the provided owner with data: id = {ownerId} & OwnerType: {ownerType}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
