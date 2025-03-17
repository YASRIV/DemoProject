using OrderDetailsApi.Interface;
using Car_Wash_Library.Models;
using OrderDetailsApi.OrderInterface;
using OrderDetailsApi.OrderRepository;
using Car_Wash_Library.DTOClass;


namespace OrderDetailsApi.OrderProcess
{
    public class OrdProcess 
    {
        private readonly IOrder _order;
        private readonly IServiceOrder _serviceOrder;
        private readonly IWashRequest _request;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public OrdProcess(IOrder order,IServiceOrder serviceOrder, IWashRequest request, HttpClient httpClient, IConfiguration configuration)
        {
            _order = order;
            _serviceOrder = serviceOrder;
            _request = request;
            _httpClient = httpClient;
            _configuration = configuration;
        }







        //Order


        public async Task<Order> BookOrder(int customerId, List<int> serviceIds)
        {
            try
            {
                // Fetch service details from Service API
                string serviceApiUrl = _configuration["ServiceApi:BaseUrl"];
                List<ServicePlan> services = new List<ServicePlan>();

                foreach (var serviceId in serviceIds)
                {
                    var response = await _httpClient.GetAsync($"{serviceApiUrl}/api/Service/GetServiceById/{serviceId}");
                    response.EnsureSuccessStatusCode();
                    var service = await response.Content.ReadFromJsonAsync<ServicePlan>();

                    if (service != null)
                        services.Add(service);
                }

                if (!services.Any())
                    throw new Exception("No valid services selected for booking.");

                // Calculate total price
                double totalPrice = services.Sum(s => s.Price);

                // Create Order
                var order = new Order
                {
                    CustomerId = customerId,
                    OrderDate = DateTime.UtcNow,
                    WashDate = DateTime.UtcNow,
                    OrderStatus = "Pending",
                    TotalPrice = totalPrice,
                    IsActive = true
                };

                order.OrderId = await _order.AddOrder(order);

                // Create Service Orders
                foreach (var service in services)
                {
                    var serviceOrder = new ServiceOrder
                    {
                        OrderId = order.OrderId,
                        ServiceId = service.ServiceId,
                        Price = service.Price
                    };
                    await _serviceOrder.AddServiceOrder(serviceOrder);
                }

                return order;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error fetching services from Service API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while booking the order.", ex);
            }
        }

        public async Task<bool> GenerateWashRequest(int orderId)
        {
            try
            {
                var order = await _order.GetOrderById(orderId);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");

                var washRequest = new WashRequest
                {
                    OrderId = order.OrderId,
                    RequestStatus = "Pending",
                    IsActive = true
                };

                return await _request.AddWashRequest(washRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating wash request for Order ID {orderId}.", ex);
            }
        }


        public async Task<bool> MakePayment(int orderId)
        {
            try
            {
                var order = await _order.GetOrderById(orderId);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");

                if (order.TotalPrice == null || order.TotalPrice <= 0)
                    throw new Exception("Invalid order amount for payment.");

                // Call Payment API
                string paymentApiUrl = _configuration["PaymentApi:BaseUrl"];
                var paymentRequest = new PaymentRequest { OrderId = order.OrderId, Amount = order.TotalPrice, PaymentMethod = "credit card" };
                var response = await _httpClient.PutAsJsonAsync($"{paymentApiUrl}/api/Payment/ProcessPayment", paymentRequest);
                response.EnsureSuccessStatusCode();
                var paymentResponse = await response.Content.ReadFromJsonAsync<ResponsePayment>();
                order.TransactionId = paymentResponse.TransactionId;
                order.IsInvoiceGenerated = true;
                order.OrderStatus = "Paid";
                await _order.UpdateOrder(order);

                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error processing payment with Payment API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error occurred while making payment for Order ID {orderId}.", ex);
            }
        }

        public async Task<List<Order>> ViewCustomerOrders(int customerId)
        {
            try
            {
                return await _order.GetOrdersByCustomerId(customerId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching orders for Customer ID {customerId}.", ex);
            }
        }

        public async Task<List<Order>> ViewWasherOrders(int washerId)
        {
            try
            {
                return await _order.GetOrdersByWasherId(washerId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching orders for Washer ID {washerId}.", ex);
            }
        }







        public async Task<int> AddOrder(Order order)
        {
            return await _order.AddOrder(order);
        }
        public async Task<bool> UpdateOrder( Order order)
        {
            return await _order.UpdateOrder( order);
        }

        public async Task<bool> DeleteOrder(int OrderId)
        {
            return await _order.DeleteOrder(OrderId);
        }
        public async Task<Order> GetOrderById(int OrderId)
        {
            return await _order.GetOrderById(OrderId);
        }
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _order.GetOrders();
        }



        //ServiceOrder

        public async Task<bool> AddServiceOrder(ServiceOrder serviceorder)
        {
            return await _serviceOrder.AddServiceOrder(serviceorder);
        }
        public async Task<bool> UpdateServiceOrder(int id, ServiceOrder serviceorder)
        {
              return await _serviceOrder.UpdateServiceOrder(id ,serviceorder);
        }

        public async Task<bool> DeleteServiceOrder(int serviceorderId)
        {
              return await _serviceOrder.DeleteServiceOrder(serviceorderId);
        }
        public async Task<ServiceOrder> GetServiceOrderById(int serviceorderId)
        {
              return await _serviceOrder.GetServiceOrderById(serviceorderId);
        }
        public async Task<IEnumerable<ServiceOrder>> GetServiceOrders()
        {
            return await _serviceOrder.GetServiceOrders();
        }


        //washrequest

        public async Task<bool> AddWashRequest(WashRequest washRequest)
        {
            return await _request.AddWashRequest(washRequest);
        }
        public async Task<bool> UpdateWashRequest(int id)
        {
            return await _request.UpdateWashRequest(id);
        }

        public async Task<bool> DeleteWashRequest(int requestId)
        {
            return await _request.DeleteWashRequest(requestId);
        }
        public async Task<WashRequest> GetWashRequestById(int requestId)
        {
            return await _request.GetWashRequestById(requestId);
        }
        public async Task<IEnumerable<WashRequest>> GetWashRequests()
        {
            return await _request.GetWashRequests();
        }


        public async Task<bool> AcceptWashRequest(int requestId, int washerId)
        {
            var item = await _request.AcceptWashRequest(requestId, washerId);
            if (item)
            {
                var request = await _request.GetWashRequestById(requestId);
                var order = await _order.GetOrderById(request.OrderId);
                order.RequestId = requestId;
                order.WasherId = washerId;
                await _order.UpdateOrder(order);
                return true;
            }
            return item;
        }

        public async Task<bool> HandleWashRequest(int requestId)
        {
            return await _request.HandleWashRequest(requestId);
        }




        #region invoice

        public async Task<InvoiceDto?> GenerateInvoice(int requestId)
        {
            var washRequest = await _request.GetWashRequestById(requestId);
            if (washRequest == null || washRequest.RequestStatus != "Completed")
                return null; // ✅ Invoice only for completed orders

            var order = await _order.GetOrderById(washRequest.OrderId);
            if (order == null) return null;

            var serviceIds = await _serviceOrder.GetServiceOrderByOrderId(order.OrderId);
            if (serviceIds == null) return null;

            // ✅ Call Customer API
            var customerResponse = await _httpClient.GetAsync($"http://localhost:8801/api/Customers/GetCustomer/{order.CustomerId}");
            var customer = customerResponse.IsSuccessStatusCode ?
                await customerResponse.Content.ReadFromJsonAsync<Customer>() : null;

            // ✅ Call Washer API
            Washer? washer = null;
            if (order.WasherId != null)
            {
                var washerResponse = await _httpClient.GetAsync($"http://localhost:8806/api/Washers/GetWasherById/{order.WasherId}");
                washer = washerResponse.IsSuccessStatusCode ?
                    await washerResponse.Content.ReadFromJsonAsync<Washer>() : null;
            }

            // Call ServicePlan API
            // var servicePlan = await _httpClient.GetAsync($"http://localhost:8805/api/ServicePlan/GetServicePlanById/{order.CustomerId}");
            string serviceApiUrl = _configuration["ServiceApi:BaseUrl"];
            List<string> servicePlan = new List<string>();

            foreach (var serviceId in serviceIds)
            {
                var response = await _httpClient.GetAsync($"{serviceApiUrl}/api/ServicePlan/GetServicePlanById/{serviceId}");
                response.EnsureSuccessStatusCode();
                var service = await response.Content.ReadFromJsonAsync<ServicePlan>();

                if (service != null)
                    servicePlan.Add(service.ServiceName);
            }

            if (!servicePlan.Any())
                throw new Exception("No valid services selected for booking."); // we have to catch this exception

            // Calculate total price
            //double totalPrice = servicePlan.Sum(s => s.Price);

            // ✅ Call Car Details from Customer API
            var carResponse = await _httpClient.GetAsync($"http://localhost:8801/api/Customers/CarsBycustomerId/{order.CustomerId}");
            var car = carResponse.IsSuccessStatusCode ?
                await carResponse.Content.ReadFromJsonAsync<List<Car>>() : null;

            // ✅ Return Invoice DTO
            return new InvoiceDto
            {
                OrderId = order.OrderId,
                TransactionId = order.TransactionId ?? 0,
                WasherId = washRequest.WasherId,
                WasherName = washer?.WasherName ?? "Washer Not Found",
                CustomerId = customer?.CustomerId ?? 0,
                CustomerName = customer?.Name ?? "Customer Not Found",
                CarDetails = car,
                ServiceName = servicePlan,
                Price = order.TotalPrice,
                Date = order.WashDate,
                Status = "Paid"
            };
        }




        #endregion invoice

    }
}
