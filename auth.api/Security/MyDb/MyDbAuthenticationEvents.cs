using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace auth.api.Security.MyDb
{
    public class MyDbAuthenticationEvents
    {
        public Func<MyDbAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        public Func<ValidateCredentialsContext, IServiceProvider, Task> OnValidateCredentials { get; set; } = (context, serviceProvider) => Task.CompletedTask;

        public virtual Task AuthenticationFailed(MyDbAuthenticationFailedContext context) => OnAuthenticationFailed(context);

        public virtual Task ValidateCredentials(ValidateCredentialsContext context, IServiceProvider serviceProvider) => OnValidateCredentials(context, serviceProvider);
    }
}