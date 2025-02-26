using Microsoft.AspNetCore.Authorization;

namespace GeminiAdvancedAPI.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string CanDeleteProducts = "CanDeleteProductsPolicy";
        // Diğer policy'leri buraya ekleyebilirsiniz

        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(CanDeleteProducts, policy =>
                policy.RequireClaim("CanDeleteProducts", "true"));

            // Diğer policy'leri buraya ekleyebilirsiniz, örneğin:
            // options.AddPolicy("CanCreateProducts", policy =>
            //     policy.RequireClaim("CanCreateProducts", "true"));
            // options.AddPolicy("MustBePremiumUser", policy =>
            //     policy.RequireClaim("PremiumUser", "true"));
        }
    }
}
