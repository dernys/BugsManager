using BugsManager.Application.Interfaces;
using BugsManager.Domain.Common;
using BugsManager.Domain.Common.Interfaces;
using BugsManager.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services
                 .AddTransient<IMediator, Mediator>()
                 .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                 .AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}
