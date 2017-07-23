using NXMobileHandsOn.ServiceReference;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace NXMobileHandsOn
{
    public class LoginService
    {

        public static readonly EndpointAddress EndPoint
            = new EndpointAddress("https://nxcloud-0-nxv2.superstream.co.jp/SuperStreamNX/SC/NLC00100SI.svc");

        private INLC00100SI client;

        public LoginService()
        {
            BasicHttpBinding binding = CreateBasicHttp();
            client = new NLC00100SIClient(binding, EndPoint);
        }

        public async Task<NLC00100SIParamV2> LoginAsync(string kaiCode, string userId, string password, DateTime loginDate)
        {
            var param = new NLC00100SIParamV2();
            param.KaiCode = kaiCode;
            param.UsrID = userId;
            param.Pswd = password;
            param.LoginDate = loginDate;

            var task = new TaskFactory().FromAsync(client.BeginLogInV2, client.EndLogInV2, param, null);
            return await task;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }
    }
}
