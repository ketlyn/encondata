using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TechsysLog.Api.Commands;
using TechsysLog.Api.Data;
using TechsysLog.Api.Repositories;
using TechsysLog.Core.Identity;

namespace TechsysLog.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IRequestHandler<AddDeliveryCommand, ValidationResult>, AddDeliveryCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteOrderCommand, ValidationResult>, DeleteOrderCommandHandler>();

            services.AddScoped<TechsysLogDBContext>();

        }
    }
}
