using System;
using System.Linq;
using System.Threading.Tasks;
using HelloNetcore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HelloNetcore.Data
{
    public class ApplicaitonDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SyncTask(ApplicationDbContext context, IServiceProvider service)
        {
            if (!context.Roles.Any())
            {
                _roleManager = service.GetRequiredService<RoleManager<ApplicationUserRole>>();

                var role = new ApplicationUserRole()
                {
                    Name = "Administrators",
                    NormalizedName = "ADMINISTRATORS"
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认角色失败!");
                }
            }

            if (!context.Users.Any())
            {
                _userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

                var defaultUser = new ApplicationUser()
                {
                    UserName = "Administrator",
                    Email = "lixinkuo@outlook.com",
                    NormalizedUserName = "ADMIN",
                    Avator = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1532516856278&di=7b178737ba555d0ad2c2c770258f2370&imgtype=0&src=http%3A%2F%2Fpic.baike.soso.com%2Fp%2F20131209%2F20131209170947-1029672786.jpg"
                };

                var result = await _userManager.CreateAsync(defaultUser, "Qwerty@123");
                await _userManager.AddToRoleAsync(defaultUser, "Administrators");
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败!");
                }
            }



        }
    }
}
