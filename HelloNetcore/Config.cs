using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace HelloNetcore
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetResouces()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api","My api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="mvc",
                    ClientName = "Mvc Client",
                    ClientUri = "http://localhost:5001",
                    LogoUri = "https://ss2.baidu.com/6ONYsjip0QIZ8tyhnq/it/u=1583943344,1181826411&fm=58&bpow=750&bpoh=716",
                    AllowRememberConsent = true,

                    AllowedGrantTypes= GrantTypes.Implicit,//模式：最简单的模式
                    ClientSecrets={//私钥
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes={//可以访问的Resource
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                    },
                    RedirectUris={"http://localhost:5001/signin-oidc"},//跳转登录到的客户端的地址
                    PostLogoutRedirectUris={"http://localhost:5001/signout-callback-oidc"},//跳转登出到的客户端的地址
                    RequireConsent=true//是否需要用户点击确认进行跳转
                }
            };
        }


        public static List<TestUser> GetTestUser()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "10000",
                    Username = "xink",
                    Password = "123456"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
