using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hass.Client.HassApi;
using Hass.Client.Models;
using Moq;
using Newtonsoft.Json.Linq;


namespace Hass.Client.UWP.Tests.Models
{
    [TestClass]
    public class HassSystemTest
    {

        public static Task<T> CreateCompletedTask<T>(T result)
        {
            var t = new TaskCompletionSource<T>();
            t.SetResult(result);
            return t.Task;
        }

        private static StateResult[] LoadStateResults()
        {
            return StateResult.ParseStates(Data.MockDataLoader.LoadJson<WsAPI>("ListStatesAsync.json"));
        }

        [TestMethod]
        public void InitializeTest()
        {
            var mWsApi = new Mock<IHassAPI>();
            mWsApi.Setup(api => api.ConnectAsync()).Returns(() => CreateCompletedTask(true));
            mWsApi.Setup(api => api.ListStatesAsync()).Returns(() => CreateCompletedTask(LoadStateResults()));

            var hass = new HassSystem(mWsApi.Object);

            hass.Initialize().Wait();

            Assert.IsNotNull(hass.AllEntities);
        }

    }
}
