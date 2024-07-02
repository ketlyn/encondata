using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechsysLog.Core.Communications;
using TechsysLog.Core.Extensions;
using TechsysLog.Web.Extensions;
using TechsysLog.Web.Models;

namespace TechsysLog.Web.Services
{
    public interface IOrderService
    {
        Task<PagedResult<OrderModel>> GetAllOrdersPaged(int pageSize, int pageIndex, string query = null);
        Task<ResponseResult> Add(AddOrderModel command);
        Task<ResponseResult> Delete(Guid Id);
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;
        public OrderService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.TechsysLogUrl);
        }

        public async Task<ResponseResult> Add(AddOrderModel command)
        {
            var content = GetContent(command);
            var response = await _httpClient.PostAsync("/api/orders/create", content);
            if (!HandleErrorsResponse(response)) return await DeserializeResponseObject<ResponseResult>(response);
            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid Id)
        {
            var response = await _httpClient.DeleteAsync($"/api/orders/delete/{Id}");
            if (!HandleErrorsResponse(response)) return await DeserializeResponseObject<ResponseResult>(response);
            return ReturnOk();
        }

        public async Task<PagedResult<OrderModel>> GetAllOrdersPaged(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/api/orders/all?ps={pageSize}&page={pageIndex}&q={query}");
            HandleErrorsResponse(response);
            return await DeserializeResponseObject<PagedResult<OrderModel>>(response);
        }

    }
}
