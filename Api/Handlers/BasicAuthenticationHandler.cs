using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UnitOfWork _unitOfWork;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger,
        UrlEncoder encoder, 
        ISystemClock clock, UnitOfWork unitOfWork) : base(options, logger, encoder, clock)
    {
        _unitOfWork = unitOfWork;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var isHaveHeader = Request.Headers.ContainsKey("Authorization");

        if (!isHaveHeader)
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
        }
        
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader["Basic ".Length..].Trim();
            Console.WriteLine(token);
            var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialstring.Split(':');

            var account = _unitOfWork.AccountsRepository.Authenticate(credentials[0], credentials[1]);

            if (account != null)
            {
                var claims = new[] { new Claim("Username", credentials[0]), new Claim(ClaimTypes.Role, account.Role.ToString()) };
                var identity = new ClaimsIdentity(claims, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(identity);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
        }

        Response.StatusCode = 401;
        return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
    }
}