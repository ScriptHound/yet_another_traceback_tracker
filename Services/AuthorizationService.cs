using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Yet_Another_Traceback_Tracker.Services;

class AuthorizationService : AuthorizationHandler<PasswordAuthorizeAttribute>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserService _userService;
    
    public AuthorizationService(IPasswordHasher passwordHasher, IUserService userService)
    {
        _passwordHasher = passwordHasher;
        _userService = userService;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PasswordAuthorizeAttribute requirement)
    {
        if (requirement.Username is null || requirement.Password is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        var user = _userService.AuthenticateUser(requirement.Username, requirement.Password);
        if (user)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}


class PasswordAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement,
    IAuthorizationRequirementData
{
    public string? Password { get; set; }
    public string? Username { get; set; }

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}

class PasswordAuthorizePolicyProvider : IAuthorizationPolicyProvider
{
    const string POLICY_PREFIX = "Password";
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
    public PasswordAuthorizePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => 
        FallbackPolicyProvider.GetDefaultPolicyAsync();
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        FallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var password = policyName;
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder(
                BearerTokenDefaults.AuthenticationScheme);
            policy.AddRequirements(new PasswordAuthorizeAttribute());
            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return Task.FromResult<AuthorizationPolicy?>(null);
    }
}