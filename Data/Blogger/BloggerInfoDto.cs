namespace BlazorServerApp.Data.Blogger;
public struct BloggerInfoDto
{
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 博主
    /// </summary>
    public string Blogger { get; set; }

    /// <summary>
    /// 博主简介
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; }
}
