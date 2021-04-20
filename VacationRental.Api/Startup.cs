using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Api.Middlewares;
using VacationRental.Api.Models;
using VacationRental.Domain;
using VacationRental.Domain.ValueObject;

namespace VacationRental.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpCacheHeaders((expirationModelOptions) =>
            {
                expirationModelOptions.MaxAge = 60;
                expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            },
            (validationModelOption) =>
            {
                validationModelOption.MustRevalidate = true;
            });

            services.AddResponseCaching();

            services.AddControllers();

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Vacation rental information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
            services.AddSingleton<IDictionary<int, Booking>>(new Dictionary<int, Booking>());

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<Booking, BookingDto>();
                mc.CreateMap<BookingDto, Booking>();

                mc.CreateMap<Rental, RentalDto>();
                mc.CreateMap<RentalDto, Rental>();

                mc.CreateMap<Calendar, CalendarDto>();
                mc.CreateMap<CalendarDto, Calendar>();

                mc.CreateMap<CalendarDate, CalendarDateDto>();
                mc.CreateMap<CalendarDateDto, CalendarDate>();

                mc.CreateMap<CalendarBooking, CalendarBookingDto>();
                mc.CreateMap<CalendarBookingDto, CalendarBooking>();

                mc.CreateMap<PreparationTime, PreparationTimeDto>();
                mc.CreateMap<PreparationTimeDto, PreparationTime>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<ExceptionMiddleware>>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "VacationRental v1"));

            app.UseHttpsRedirection();

            app.UseHttpCacheHeaders();

            app.UseResponseCaching();

            app.UseMiddleware<ExceptionMiddleware>();            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
