using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediatREndpoint.FunctionalTests.Authentication
{
    public class MockAuthenticationHandlerOptions : AuthenticationSchemeOptions
    {
    }

    public class MockAuthenticationHandler : AuthenticationHandler<MockAuthenticationHandlerOptions>
    {
        public MockAuthenticationHandler(IOptionsMonitor<MockAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
