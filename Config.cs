// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace is4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
               
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "会议数据")
            };


        public static IEnumerable<Client> Clients =>            
            new Client[]
            {             
                //app client
                new Client{

                     ClientId="pwdClient",
                    //OAuth密码模式
                     AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                     ClientSecrets={new Secret("secret".Sha256())},

                     AccessTokenLifetime=60*60*6,
                     AllowOfflineAccess=true,

                    AllowedScopes={
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                },
                //localhost client
                new Client
                {
                    ClientId = "spa",
                    ClientName = "会议管理系统",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,

                    AccessTokenLifetime=60*60,

                    //所有客户端必须加这个，因为不验证客户端id的逻辑，我给删了
                    RequirePkce = true,

                    RequireClientSecret = false,

                    RedirectUris =
                    {
                       "http://localhost:4200/signin-oidc"
                    },

                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes = { "openid", "profile", "api1" }
                },
                //yun client
                new Client
                {
                    ClientId = "spa2",
                    ClientName = "会议管理系统",
                    ClientUri = "http://39.99.217.82:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser=true,
                    RequireConsent=true,

                    AccessTokenLifetime=60*60,

                    RequirePkce = true,

                    RequireClientSecret = false,

                    RedirectUris =
                    {
                       "http://39.99.217.82:4200/signin-oidc"
                    },

                    PostLogoutRedirectUris = { "http://39.99.217.82:4200" },
                    AllowedCorsOrigins = { "http://39.99.217.82:4200" },

                    AllowedScopes = { "openid", "profile", "api1" }
                }
            };
    }
}