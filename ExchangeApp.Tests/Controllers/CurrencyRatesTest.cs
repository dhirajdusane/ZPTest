using ExchangeApp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Tests.Controllers
{    
    [TestClass]
    public class CurrencyRatesTest
    {
        [TestCategory("CurrencyRatesController")]
        [TestMethod]
        public void PostWithParam()
        {
            CurrencyRatesController c = new CurrencyRatesController();
            //Console.WriteLine(c.PostRate("BSD"));
        }

        [TestCategory("CurrencyRatesController")]
        [TestMethod]
        public void Post()
        {
            CurrencyRatesController c = new CurrencyRatesController();            
            //Console.WriteLine(c.PostRate());
        }
    }
}
