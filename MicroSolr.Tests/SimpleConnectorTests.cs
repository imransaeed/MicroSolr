using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroSolr.Connectors;
using MicroSolr.Core.Clients;
using MicroSolr.Core.Cores;

namespace MicroSolr.Tests
{
    [TestClass]
    public class SimpleConnectorTests
    {
        class Product
        {
            public string id;
            public string[] title;
        }

        [TestMethod]
        public void TestSave()
        {
            //var solr = SimpleConnector<Product>.Create(@"http://localhost.:8983/solr/", "collection1");
            //solr.Save(new Product { id="100", title = "abcd" });
        }

        [TestMethod]
        public void TestLoad()
        {
            var solr = SimpleConnector<Product>.Create(@"http://localhost.:8983/solr/", "collection1");
            var results = solr.Query("*:*");
            //Assert.IsNotNull(results);
        }
    }
}
