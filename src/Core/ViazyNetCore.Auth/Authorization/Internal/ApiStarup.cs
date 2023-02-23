using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using ViazyNetCore;
using ViazyNetCore.Authorization.Modules;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiStarup
    {
        public static IServiceCollection AddBMSAuthorization(this IServiceCollection services, Action<MenusOptions> menuOption = null)
        {
            if (menuOption != null)
            {
                services.Configure(menuOption);
            }
            else
            {
                services.AddOptions<MenusOptions>();
            }
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<Aoite.WebApi.IApiFilter, PermissionFilter>());

            return services;
        }

        private static async Task InitUser(IServiceProvider services)
        {
            var freeSql = services.GetService<IFreeSql>();
            var usershipService = services.GetService<IUsershipService>();
            var permissionService = services.GetService<PermissionService>();
            var options = services.GetRequiredService<IOptions<MenusOptions>>().Value;

            if (freeSql == null)
                throw new ArgumentNullException(nameof(freeSql));
            if (usershipService == null)
                throw new ArgumentNullException(nameof(usershipService));
            if (permissionService == null)
                throw new ArgumentNullException(nameof(permissionService));

            var bmsUsers = freeSql.Select<BmsUser>();
            if (!await bmsUsers.AnyAsync(u => u.Username == "admin"))
            {
                var user = new BmsUser()
                {
                    Id = Snowflake.NextIdString(),
                    Status = ComStatus.Enabled,
                    PasswordSalt = Guid.NewGuid(),
                    Username = "admin",
                    Nickname = "管理员",
                    CreateTime = DateTime.Now
                };
                await usershipService.CreateUser(user, "123456".ToMd5());
            }

            if (!await permissionService.ExistsPermissionByPermissionKey(PermissionItemKeys.Instance().User()))
            {
                await permissionService.AddPermission("用户管理", PermissionItemKeys.Instance().User());

                var menuIds = new List<string>();

                var parentMenuId = await permissionService.UpdateMenus(new BmsMenus
                {
                    Id = null,
                    Name = "我的工作台",
                    CreateTime = DateTime.Now,
                    Description = null,
                    Exdata = null,
                    Icon = null,
                    IsHomeShow = true,
                    OpenType = 0,
                    OrderId = 99,
                    ParentId = null,
                    ProjectId = null,
                    Status = ComStatus.Enabled,
                    SysId = null,
                    Type = MenuType.MidNode,
                    Url = null
                });

                menuIds.Add(parentMenuId);

                var orderId = 0;
                foreach (var menu in options.Default)
                {
                    var menuId = await permissionService.UpdateMenus(new BmsMenus
                    {
                        Id = null,
                        Name = menu.Name,
                        CreateTime = DateTime.Now,
                        Description = menu.Description,
                        Exdata = null,
                        Icon = menu.Icon,
                        IsHomeShow = true,
                        OpenType = 0,
                        OrderId = orderId++,
                        ParentId = parentMenuId,
                        ProjectId = null,
                        Status = ComStatus.Enabled,
                        SysId = null,
                        Type = menu.Type,
                        Url = menu.Url
                    });
                    menuIds.Add(menuId);
                }

                await permissionService.UpdateMenusInPermission(PermissionItemKeys.Instance().User(), menuIds.ToArray());
                if (options.OtherMenus != null)
                    await AddMenus(permissionService, options.OtherMenus);

                if (options.PermissionKeys != null)
                {
                    foreach (var key in options.PermissionKeys)
                    {
                        await permissionService.AddPermission(key.Name, key.Key);
                    }
                }
            }
        }
        public static IApplicationBuilder UseBMSAuthorization(this IApplicationBuilder app)
        {
            InitUser(app.ApplicationServices).GetAwaiter().GetResult();
            return app;
        }
        public static async Task AddMenus(PermissionService permissionService, List<Menu> menus, string parentId = "10000")
        {
            var orderId = 0;
            foreach (var menu in menus)
            {
                var menuId = await permissionService.UpdateMenus(new BmsMenus
                {
                    Id = null,
                    Name = menu.Name,
                    CreateTime = DateTime.Now,
                    Description = menu.Description,
                    Exdata = null,
                    Icon = menu.Icon,
                    IsHomeShow = true,
                    OpenType = 0,
                    OrderId = orderId++,
                    ParentId = parentId,
                    ProjectId = null,
                    Status = ComStatus.Enabled,
                    SysId = null,
                    Type = menu.Type,
                    Url = menu.Url
                });
                if (menu.Children != null)
                {
                    await AddMenus(permissionService, menu.Children, menuId);
                }
            }
        }
    }

    public class MenusOptions
    {
        internal List<Menu> Default => new List<Menu>(){
                        new Menu(){
                            Name="用户管理",
                            Type= MenuType.Node,
                            Url=this.UserManageUrl,
                            Icon="el-icon-share"
                        },
                        new Menu(){
                             Name="角色管理",
                             Type= MenuType.Node,
                               Url=this.RoleManageUrl,
                               Icon="el-icon-location"
                        },
                        new Menu(){
                             Name="菜单管理",
                              Type= MenuType.Node,
                               Url=this.MenuManageUrl,
                               Icon="el-icon-menu"
                        },
                         new Menu(){
                             Name="权限管理",
                              Type= MenuType.Node,
                               Url= this.PermissionManageUrl,
                               Icon="el-icon-view"
                        },
                    };

        public string UserManageUrl { get; set; } = "/system/user/index";

        public string RoleManageUrl { get; set; } = "/system/role/index";

        public string MenuManageUrl { get; set; } = "/system/menu/index";

        public string PermissionManageUrl { get; set; } = "/system/permission/index";

        public List<Menu> OtherMenus { get; set; }

        public List<PermissionKeyName> PermissionKeys { get; set; }
    }

    public class Menu
    {
        internal Menu()
        {
        }
        public Menu(string name)
        {
            this.Name = name;
            this.Type = MenuType.MidNode;
        }
        public Menu(string name, string url, string icon)
        {
            this.Name = name;
            this.Type = MenuType.Node;
            this.Url = url;
            this.Icon = icon;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<Menu> Children { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示类型 (0叶子节点，1中间节点， 2按钮)。
        /// </summary>
        public MenuType Type { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示URL地址。
        /// </summary>
        public string Url { get; set; }

        public string Icon { get; set; }
    }

    public class PermissionKeyName
    {
        public PermissionKeyName(string key, string name)
        {
            this.Key = key;
            this.Name = name;
        }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
