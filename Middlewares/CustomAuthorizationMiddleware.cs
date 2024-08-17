using HotChocolate.Resolvers;

namespace GraphQL.Demo.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly FieldDelegate _next;

        public CustomAuthorizationMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            var httpContext = context.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;

            if (httpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = ErrorBuilder.New()
                    .SetMessage("Unauthorized")
                    .SetCode("AUTH_NOT_AUTHORIZED")
                    .Build();
                return;
            }

            await _next(context);
        }
    }
}
