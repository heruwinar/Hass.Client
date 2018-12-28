using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hass.Client.HassApi;
using Hass.Client.Models;

namespace Hass.Client.UWP.Tests.Models
{
    [TestClass]
    public class HassSystemTest
    {

        [TestMethod]
        public void InitializeTest()
        {
            WsAPI.Instance = Mock.Factory.CreateMockIHassAPI();

            var hass = new HassSystem();

            hass.Initialize().Wait();

            Assert.IsNotNull(hass.AllEntities);
        }

    }
}
