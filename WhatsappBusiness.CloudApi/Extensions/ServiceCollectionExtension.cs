﻿using Microsoft.Extensions.DependencyInjection;
#if !NET472
using Polly;
using Polly.Extensions.Http;
#endif
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Interfaces;

namespace WhatsappBusiness.CloudApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Creating a whatsapp business cloud api service collection to be used in projects that support dependency injection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="whatsAppBusinessPhoneNumberId"></param>
        public static void AddWhatsAppBusinessCloudApiService(this IServiceCollection services, WhatsAppBusinessCloudApiConfig whatsAppConfig, string? graphAPIVersion = null)
        {
#if !NET472
            Random jitterer = new Random();

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
#endif

            services.AddTransient<IWhatsAppBusinessClientFactory, WhatsAppBusinessClientFactory>();

			services.AddSingleton(new WhatsAppBusinessCloudApiConfig
            {
                WhatsAppBusinessPhoneNumberId = whatsAppConfig.WhatsAppBusinessPhoneNumberId,
                WhatsAppBusinessAccountId = whatsAppConfig.WhatsAppBusinessAccountId,
                WhatsAppBusinessId = whatsAppConfig.WhatsAppBusinessId,
                GroupId = whatsAppConfig.GroupId,
				AccessToken = whatsAppConfig.AccessToken,
                AppName = whatsAppConfig.AppName,
                Version = whatsAppConfig.Version,
                WebhookVerifyToken = whatsAppConfig.WebhookVerifyToken
            });

            services.AddHttpClient<IWhatsAppBusinessClient, WhatsAppBusinessClient>(options =>
            {
                options.BaseAddress = (string.IsNullOrWhiteSpace(graphAPIVersion)) ? WhatsAppBusinessRequestEndpoint.BaseAddress : new Uri(WhatsAppBusinessRequestEndpoint.GraphApiVersionBaseAddress.ToString().Replace("{{api-version}}", graphAPIVersion));
                options.Timeout = TimeSpan.FromMinutes(10);
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
#if !NET472
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);
#else
            });
#endif
        }

        public static void AddWhatsAppBusinessCloudApiService<THandler>(this IServiceCollection services, WhatsAppBusinessCloudApiConfig whatsAppConfig, string? graphAPIVersion = null) where THandler : HttpMessageHandler
        {
#if !NET472
            Random jitterer = new Random();

            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();
#endif

			services.AddTransient<IWhatsAppBusinessClientFactory, WhatsAppBusinessClientFactory>();

			services.AddSingleton(new WhatsAppBusinessCloudApiConfig
            {
                WhatsAppBusinessPhoneNumberId = whatsAppConfig.WhatsAppBusinessPhoneNumberId,
                WhatsAppBusinessAccountId = whatsAppConfig.WhatsAppBusinessAccountId,
                WhatsAppBusinessId = whatsAppConfig.WhatsAppBusinessId,
                GroupId = whatsAppConfig.GroupId,
				AccessToken = whatsAppConfig.AccessToken,
                AppName = whatsAppConfig.AppName,
                Version = whatsAppConfig.Version,
                WebhookVerifyToken = whatsAppConfig.WebhookVerifyToken
            });

            services.AddHttpClient<IWhatsAppBusinessClient, WhatsAppBusinessClient>(options =>
            {
                options.BaseAddress = (string.IsNullOrWhiteSpace(graphAPIVersion)) ? WhatsAppBusinessRequestEndpoint.BaseAddress : new Uri(WhatsAppBusinessRequestEndpoint.GraphApiVersionBaseAddress.ToString().Replace("{{api-version}}", graphAPIVersion));
                options.Timeout = TimeSpan.FromMinutes(10);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan)
              .ConfigurePrimaryHttpMessageHandler<THandler>()
#if !NET472
              .AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);
#else
              ;
#endif
        }
    }
}
