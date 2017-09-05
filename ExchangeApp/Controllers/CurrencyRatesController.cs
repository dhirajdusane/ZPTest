using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ExchangeApp;
using System.Web.Mvc;
using System.Web.Http.Results;

namespace ExchangeApp.Controllers
{
    public class Result
    {
        public string SourceCurrency { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public int returncode { get; set; }
        public string err { get; set; }
        public string timestamp { get; set; }
    }

    public class CurrencyRatesController : ApiController
    {
        [OutputCache( Duration = 120, VaryByParam = "currencyCode" )]
        public JsonResult<Result> PostRate(decimal amount, string currencyCode)
        {
            Result result = new Result();
            result.SourceCurrency = currencyCode;
            result.Amount = amount;

            CurrencyRate rate = null;
            using (ApplicationTestEntities entities = new ApplicationTestEntities())
            {
                rate = entities.CurrencyRates.FirstOrDefault(x => x.Code.Equals
                (currencyCode, StringComparison.InvariantCultureIgnoreCase));
            }

            if (rate == null || !rate.Rate.HasValue)
            {
                result.returncode = 0;
                result.err = "Unable to get conversion rate. Only Support {USD,GBP,AUD,EUR,CAD,SGD}";
                result.timestamp = "0";
                return this.Json(result);
            }

            result.err = "success";
            result.returncode = 1;
            result.ConversionRate = Convert.ToDecimal(Math.Round(rate.Rate.Value, 2));
            result.Total = Math.Round(result.ConversionRate * amount, 2);
            result.timestamp = rate.LastUpdateTimeStamp.Value.ToFileTimeUtc().ToString();
            return this.Json(result);
        }
    }
}