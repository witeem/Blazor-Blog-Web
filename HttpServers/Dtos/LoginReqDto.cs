namespace BlazorServerApp.HttpServers.Dtos
{
    public class LoginReqDto : ApiSignDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginResult
    { 
        /// <summary>
        /// 请求成功与否
        /// </summary>
        public bool success { get; set; }

        public string access_token { get; set; }

        public string token_type { get; set; }
    }
}
