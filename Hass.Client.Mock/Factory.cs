using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Hass.Client.HassApi;
using Moq;

namespace Hass.Client.Mock
{
    public static class Factory
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

        public static IHassAPI CreateMockIHassAPI()
        {
            var mWsApi = new Mock<IHassAPI>();
            mWsApi.Setup(api => api.ConnectAsync()).Returns(() => CreateCompletedTask(true));
            mWsApi.Setup(api => api.ListStatesAsync()).Returns(() => CreateCompletedTask(LoadStateResults()));
            return mWsApi.Object;
        }

    }

}