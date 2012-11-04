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
            var solr = SimpleConnector<Product>.Create(@"http://localhost.:8983/solr/", "collection1");
            solr.Save(new Product { id="100", title = new string[] {"abcd"} });
        }

        [TestMethod]
        public void TestSave2()
        {
            var solr = DynamicConnector<Product>.Create(@"http://localhost.:8983/solr/", "collection1");

            List<Product> p = new List<Product>();
            for (int id = 100; id < 10000; id++)
            {
                p.Add(new Product { id = id.ToString(), title = new string[] { "a title " + id.ToString() } });
            }

            solr.Save(p.ToArray());
        }

        //[TestMethod]
        public void TestLoad()
        {
            var solr = SimpleConnector<Product>.Create(@"http://localhost.:8983/solr/", "collection1");
            var results = solr.Query("*:*");
            //Assert.IsNotNull(results);
        }
    }
}
