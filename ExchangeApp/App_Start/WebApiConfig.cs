using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ExchangeApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/v0/rate",
            defaults: new { amount= 1, currencyCode = "USD", controller = "CurrencyRates", action = "PostRate" });

            config.Routes.MapHttpRoute(
            name: "onlyAmount",
            routeTemplate: "api/v0/rate/{amount}",
            defaults: new { amount = 1, currencyCode = "USD", controller = "CurrencyRates", action = "PostRate" });

            config.Routes.MapHttpRoute(
            name: "onlyCode",
            routeTemplate: "api/v0/rate/{currencyCode}",
            defaults: new { amount = 1, currencyCode = "USD", controller = "CurrencyRates", action = "PostRate" });

            config.Routes.MapHttpRoute(
            name: "both",
            routeTemplate: "api/v0/rate/{amount}/{currencyCode}",
            defaults: new { amount = 1, currencyCode = "USD", controller = "CurrencyRates", action = "PostRate" });

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add
                (new MediaTypeHeaderValue("application/json"));
        }
    }
}
