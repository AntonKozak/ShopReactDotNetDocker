using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("AuctionService", "Auction Service API Full Access"),
            new ApiScope("scope2"),
            new ApiScope("scope1", "Scope 1 for M2M Client"),
        };

    public static IEnumerable<Client> Clients(IConfiguration configuration) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = { "openid", "profile", "AuctionService", "auctionApp" },
                RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                ClientSecrets = [new Secret("NotASecret".Sha256())],
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
            },

            new Client
            {
                ClientId = "nextApp",
                ClientName = "Next App",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                RedirectUris = {configuration["ClientApp"] + "/api/auth/callback/id-server" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "AuctionService", "scope2", "scope1", "offline_access" },
                AccessTokenLifetime = 3600*24*30, // 30 days,
                AlwaysIncludeUserClaimsInIdToken = true,
            },

            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("NotASecret".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
        };
}
