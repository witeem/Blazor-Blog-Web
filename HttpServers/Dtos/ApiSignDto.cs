namespace BlazorServerApp.HttpServers.Dtos
{
    public class ApiSignDto
    {
        /// <summary>
        /// 时间擢
        /// </summary>
        public string Timespan { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }
}
