using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Timers;
using System.Web;
using System.Xml;

namespace ExchangeApp
{
    public class ExchangeRateDeamon : IDisposable
    {
        string[] supportedCurrency = new string[] {"USD","GBP","AUD","EUR","CAD","SGD" };
        //Using System.Timers so that if there are available threads 
        //on thread pool elapsed event can run parallelly.
        private Timer timer;

        public ExchangeRateDeamon()
        {
            timer = new Timer();
            timer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            timer.Elapsed += Timer_Elapsed;            
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                FetchRates();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (timer != null)
                    timer.Enabled = true;
            }
        }

        public void Dispose()
        {
            timer.Dispose();
            timer = null;
        }

        public void Start()
        {
            timer.Start();
        }

        public void FetchRates()
        {
            foreach (var item in supportedCurrency)
            {
                string resource = string.Format("/finance/converter?a=1&from={0}&to={1}", item.ToUpper(), "INR");
                RestSharp.RestRequest request = new RestSharp.RestRequest(resource);
                RestSharp.RestClient client = new RestSharp.RestClient("http://www.google.com");
                RestRequestAsyncHandle handle = client.ExecuteAsyncGet(request, AsyncReadUpdateDB, "GET");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AsyncReadUpdateDB(IRestResponse responseArguement, 
            RestRequestAsyncHandle requestHandle)
        {
            if (responseArguement.StatusCode != System.Net.HttpStatusCode.OK)
                return;

            string content = responseArguement.Content;
            Regex regex = new Regex(@"<div[^>].*?>1 ([^=]*) ([^>]*)>([^<]*) INR<\/span>", RegexOptions.IgnoreCase);
            Match match = regex.Match(content);
            
            if (!match.Success || match.Groups.Count != 4)
                return;

            string code = match.Groups[1].Value;
            decimal rate = System.Convert.ToDecimal(match.Groups[3].Value.Trim());
            using (var db = new ApplicationTestEntities())
            {
                CurrencyRate crate = db.CurrencyRates.FirstOrDefault(x => x.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
                if (crate != null)
                {
                    crate.Rate = (double)rate;
                    crate.LastUpdateTimeStamp = responseArguement.Cookies[0].TimeStamp;
                    db.SaveChanges();
                }
                else
                {                    
                    db.CurrencyRates.Add(new CurrencyRate()
                    { Code = code, Rate = (double)rate, LastUpdateTimeStamp = responseArguement.Cookies[0].TimeStamp });
                    db.SaveChanges();
                }
            }
        }
    }
}