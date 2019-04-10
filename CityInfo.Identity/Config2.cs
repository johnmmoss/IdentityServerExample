// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace CityInfo.Identiy
{
    public static class Config2
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "office",
                    DisplayName = "Office",
                    UserClaims = { "office_number" },
                    Description = "Office Number"
                }
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return null;
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
               new Client
               {
                   ClientId = "CityInfo.Web",
                   ClientName = "CityInfo Website",
                   AllowedGrantTypes = GrantTypes.Implicit,
                   RedirectUris = {"http://localhost:5000/signin-oidc" },
                   PostLogoutRedirectUris ={"http://localhost:5000/signout-callback-oidc" }, // Used to show link on logged out page
                   AllowedScopes = { "openid", "email", "office", "profile" },
                   FrontChannelLogoutSessionRequired = true,                        // Needed for single signout
                   FrontChannelLogoutUri = "http://localhost:5000/signout-oidc",    // Needed for single signout
                   
               }
            };
        }
    }
}