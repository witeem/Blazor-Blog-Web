namespace BlazorServerApp.Data.Blogger;
public struct AdverUserInfoDto
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Picture { get; set; }

    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime BirthDay { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// 用户权限(最多存在5种,使用英文逗号隔开)
    /// </summary>
    public string RoleCodes { get; set; }
}
