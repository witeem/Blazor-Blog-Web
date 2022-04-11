namespace BlazorServerApp.Global.Config
{
    public class AppSetting
    {
        /// <summary>
        /// 网关地址
        /// </summary>
        public string GatewayUri { get; set; }

        public string ApiUri { get; set; }

        public string ConnKey { get; set; }

        public string ConnIV { get; set; }
    }
}
