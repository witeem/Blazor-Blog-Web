# Blazor-Blog-Web

## 📖 项目简介

这是一个基于 Blazor Server 开发的个人博客前端项目，使用 MASA Blazor UI 组件库构建现代化的博客界面。

## ✨ 功能特性

### 已完成功能
- ✅ 展示博主个人信息
- ✅ 展示所在地天气信息
- ✅ 博客文章列表展示
- ✅ 文章详情页面
- ✅ 用户注册与登录
- ✅ 用户注销功能
- ✅ 多语言支持（中文/英文）

## 🛠️ 技术栈

- **框架**: .NET 6.0 + Blazor Server
- **UI 组件库**: [MASA Blazor](https://blazor.masastack.com/) v0.3.0
- **本地存储**: Blazored.LocalStorage
- **HTTP 客户端**: RestSharp
- **Markdown 渲染**: Markdig
- **工具库**: witeem.CoreHelper

## 📁 项目结构

```
Blazor-Blog-Web/
├── Pages/                  # 页面组件
│   ├── Home/              # 首页相关页面
│   │   ├── Index.razor    # 首页
│   │   ├── DataList.razor # 文章列表
│   │   └── DataDetail.razor # 文章详情
│   └── Authentication/    # 认证相关页面
│       ├── Login.razor    # 登录页面
│       └── Register.razor # 注册页面
├── Shared/                # 共享组件
│   ├── MainLayout.razor   # 主布局
│   └── Search.razor       # 搜索组件
├── Global/                # 全局配置
│   ├── Config/           # 配置文件
│   ├── Middleware/       # 中间件
│   └── Nav/              # 导航相关
├── HttpServers/          # HTTP 服务
├── Data/                 # 数据服务
└── wwwroot/              # 静态资源
    ├── css/              # 样式文件
    ├── js/               # JavaScript 文件
    ├── i18n/             # 多语言文件
    └── nav/              # 导航配置
```

## 🚀 快速开始

### 前置要求

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio 2022 或 VS Code

### 安装步骤

1. **克隆项目**
   ```bash
   git clone https://github.com/witeem/Blazor-Blog-Web.git
   cd Blazor-Blog-Web
   ```

2. **还原依赖**
   ```bash
   dotnet restore
   ```

3. **配置后端 API 地址**
   
   编辑 `appsettings.Production.json` 文件，修改 API 地址：
   ```json
   {
     "AppSetting": {
       "GatewayUri": "http://your-api-address:port",
       "ApiUri": "http://your-api-address:port",
       "ConnKey": "{your-key}",
       "ConnIV": "{your-iv}"
     }
   }
   ```

4. **运行项目**
   ```bash
   dotnet run
   ```

5. **访问应用**
   
   打开浏览器访问：`https://localhost:5001`

## 🐳 Docker 部署

### 构建镜像
```bash
docker build -t blazor-blog-web .
```

### 运行容器
```bash
docker run -d -p 8080:80 --name blog-web blazor-blog-web
```

## 📝 配置说明

### 主题配置

在 `Program.cs` 中配置 MASA Blazor 主题：
```csharp
builder.Services.AddMasaBlazor(builder =>
{
    builder.UseTheme(option =>
    {
        option.Primary = "#4318FF";
        option.Accent = "#4318FF";
    });
});
```

### 多语言配置

语言文件位于 `wwwroot/i18n/` 目录：
- `zh-CN.json` - 中文
- `en-US.json` - 英文

## 🔗 相关项目

- **后端 API**: [BlogCore.API](https://github.com/witeem/BlogCore.API)

## 📚 参考资料

- [Blazor 官方文档](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)
- [MASA Blazor 文档](https://blazor.masastack.com/getting-started/installation)

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情

---

⭐ 如果这个项目对你有帮助，欢迎给个 Star！
