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
    public interface IDeliveryService
    {
        Task<PagedResult<DeliveryModel>> GetAllDeliverysPaged(int pageSize, int pageIndex, string query = null);
        Task<ResponseResult> Add(AddDeliveryModel command);
        Task<ResponseResult> Delete(Guid Id);
    }

    public class DeliveryService : Service, IDeliveryService
    {
        private readonly HttpClient _httpClient;
        public DeliveryService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.TechsysLogUrl);
        }

        public async Task<ResponseResult> Add(AddDeliveryModel command)
        {
            var content = GetContent(command);
            var response = await _httpClient.PostAsync("/api/Deliverys/create", content);
            if (!HandleErrorsResponse(response)) return await DeserializeResponseObject<ResponseResult>(response);
            return ReturnOk();
        }

        public async Task<ResponseResult> Delete(Guid Id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Deliverys/delete/{Id}");
            if (!HandleErrorsResponse(response)) return await DeserializeResponseObject<ResponseResult>(response);
            return ReturnOk();
        }

        public async Task<PagedResult<DeliveryModel>> GetAllDeliverysPaged(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/api/Deliverys/all?ps={pageSize}&page={pageIndex}&q={query}");
            HandleErrorsResponse(response);
            return await DeserializeResponseObject<PagedResult<DeliveryModel>>(response);
        }

    }
}
