using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IAuthorizationHandler = HotChocolate.Authorization.IAuthorizationHandler;

namespace GraphQL.Demo.AuthorizationHandlers
{
    public class DefaultAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationService _authorizationService;

        public DefaultAuthorizationHandler(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        // Method 1: Authorizes a single directive
        public async ValueTask<AuthorizeResult> AuthorizeAsync(
            IMiddlewareContext context,
            AuthorizeDirective directive,
            CancellationToken cancellationToken = default)
        {
            context.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var principal);
            IAuthorizationPolicyProvider? policyProvider =
                context.Services.GetRequiredService<IAuthorizationPolicyProvider>();
            var authorizeResult = await _authorizationService.AuthorizeAsync(
                principal as ClaimsPrincipal,
                null,
                await policyProvider.GetDefaultPolicyAsync());

            return authorizeResult.Succeeded ? AuthorizeResult.Allowed : AuthorizeResult.NotAllowed;
        }

        // Method 2: Authorizes multiple directives
        public async ValueTask<AuthorizeResult> AuthorizeAsync(
            AuthorizationContext context,
            IReadOnlyList<AuthorizeDirective> directives,
            CancellationToken cancellationToken = default)
        {
            context.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var principal);
            foreach (var directive in directives)
            {
                var authorizeResult = await _authorizationService.AuthorizeAsync(
                    principal as ClaimsPrincipal,
                    null,
                    directive.Policy);

                if (!authorizeResult.Succeeded)
                {
                    return AuthorizeResult.NotAllowed;
                }
            }

            return AuthorizeResult.Allowed;
        }
    }
}
