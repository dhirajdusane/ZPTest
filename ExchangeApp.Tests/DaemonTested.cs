using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Tests
{
    [TestClass]
    public class DaemonTested
    {
        [TestMethod]
        public void TestFetch()
        {
            ExchangeRateDeamon daemon = new ExchangeRateDeamon();
            daemon.FetchRates();
        }
    }
}
