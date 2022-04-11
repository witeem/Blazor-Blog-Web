namespace BlazorServerApp.Global.Base.Enums
{
    public enum ApiResultEnum
    {
        Succeed = 200,
        UnAuthorization = 1401,
        Undefind = 1404,
        Error = 1502,
        ParamsError = 1504,
        SignError = 1999
    }
}
