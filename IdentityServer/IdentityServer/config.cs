using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
               new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
         new ApiScope[]
         {
         new ApiScope("Admin","Api 1 scope"),
         new ApiScope("Detailing","Api 2 scope")
         };

        public static IEnumerable<Client> Clients =>
          new Client[]
          {
              new Client
              {
                  ClientId = "client1",
                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AllowedScopes = { "Admin" },


              },
               new Client
              {
                  ClientId = "client2",
                  AllowedGrantTypes = GrantTypes.ClientCredentials,
                  ClientSecrets =
                  {
                      new Secret("secret".Sha256())
                  },
                  AllowedScopes = { "Detailing" },


              }

          };
           

    }
}
