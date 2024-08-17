using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;
using IAuthorizationHandler = HotChocolate.Authorization.IAuthorizationHandler;

namespace GraphQL.Demo.AuthorizationHandlers
{
    public class DefaultAuthorizationHandler1 : IAuthorizationHandler
    {
        public async ValueTask<AuthorizeResult> AuthorizeAsync(IMiddlewareContext context, AuthorizeDirective directive, CancellationToken cancellationToken = default)
        {
            if (!TryGetAuthenticatedPrincipal(context, out ClaimsPrincipal? principal))
            {
                return AuthorizeResult.NotAuthenticated;
            }

            if (IsInAnyRole(principal, directive.Roles))
            {
                if (NeedsPolicyValidation(directive))
                {
                    return await AuthorizeWithPolicyAsync(
                            context, directive, principal!)
                        .ConfigureAwait(false);
                }

                return AuthorizeResult.Allowed;
            }

            return AuthorizeResult.NotAllowed;
        }

        public async ValueTask<AuthorizeResult> AuthorizeAsync(AuthorizationContext context, IReadOnlyList<AuthorizeDirective> directives, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private static bool TryGetAuthenticatedPrincipal(
            IMiddlewareContext context,
            [NotNullWhen(true)] out ClaimsPrincipal principal)
        {
            if (context.ContextData.TryGetValue(nameof(ClaimsPrincipal), out var o)
                && o is ClaimsPrincipal p
                && p.Identities.Any(t => t.IsAuthenticated))
            {
                principal = p;
                return true;
            }

            principal = null;
            return false;
        }

        private static bool IsInAnyRole(
            IPrincipal principal,
            IReadOnlyList<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                return true;
            }

            return roles.Any(principal.IsInRole);
        }

        private static bool NeedsPolicyValidation(AuthorizeDirective directive)
        {
            return directive.Roles == null
                   || directive.Roles.Count == 0
                   || !string.IsNullOrEmpty(directive.Policy);
        }

        private static async Task<AuthorizeResult> AuthorizeWithPolicyAsync(
            IMiddlewareContext context,
            AuthorizeDirective directive,
            ClaimsPrincipal principal)
        {
            IServiceProvider services = context.Service<IServiceProvider>();
            IAuthorizationService? authorizeService =
                services.GetService<IAuthorizationService>();
            IAuthorizationPolicyProvider? policyProvider =
                services.GetService<IAuthorizationPolicyProvider>();

            if (authorizeService == null || policyProvider == null)
            {
                // authorization service is not configured so the user is
                // authorized with the previous checks.
                return string.IsNullOrWhiteSpace(directive.Policy)
                    ? AuthorizeResult.Allowed
                    : AuthorizeResult.NotAllowed;
            }

            AuthorizationPolicy? policy = null;

            if ((directive.Roles is null || directive.Roles.Count == 0)
                && string.IsNullOrWhiteSpace(directive.Policy))
            {
                policy = await policyProvider.GetDefaultPolicyAsync()
                    .ConfigureAwait(false);

                if (policy == null)
                {
                    return AuthorizeResult.NoDefaultPolicy;
                }
            }
            else if (!string.IsNullOrWhiteSpace(directive.Policy))
            {
                policy = await policyProvider.GetPolicyAsync(directive.Policy)
                    .ConfigureAwait(false);

                if (policy == null)
                {
                    return AuthorizeResult.PolicyNotFound;
                }
            }

            if (policy is not null)
            {
                AuthorizationResult result =
                    await authorizeService.AuthorizeAsync(principal, context, policy)
                        .ConfigureAwait(false);
                return result.Succeeded ? AuthorizeResult.Allowed : AuthorizeResult.NotAllowed;
            }

            return AuthorizeResult.NotAllowed;
        }
    }
}