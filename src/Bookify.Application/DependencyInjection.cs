using Bookify.Application.Abstractions.Behaviors;
using Bookify.Domain.Bookings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Bookify.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddTransient<PricingService>();
        }
    }
}
