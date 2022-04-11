using BlazorServerApp.Global.Base.Enums;

namespace BlazorServerApp.HttpServers.Dtos
{
    public class ApiResponce<T>
    {
        public int Code { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }

        public bool Successed
        {
            get
            {
                return Code == (int)ApiResultEnum.Succeed;
            }
        }

        public static ApiResponce<T> Success(T datas, string msg = "操作成功")
        {
            ApiResponce<T> responce = new ApiResponce<T>()
            {
                Code = (int)ApiResultEnum.Succeed,
                Data = datas,
                Msg = msg
            };
            return responce;
        }

        public static ApiResponce<T> Fail(int code, string msg)
        {
            ApiResponce<T> responce = new ApiResponce<T>()
            {
                Code = code,
                Data = default(T),
                Msg = msg
            };
            return responce;
        }
    }
}
