using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System;

// Create check role in UserService (possible return boolean)
// If role is Admin, claims.Add (new role = Admin)
// Add tags Auth-role tags to controller delete actions 
// Test on postman

namespace MyFace
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserService userService)
            : base(options, logger, encoder)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            // Attempt to parse the Authorization header into a structured AuthenticationHeaderValue object.
            if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
            {
                // If parsing fails, the header is considered invalid and authentication fails.
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            // Verify that the authorization scheme is "Basic".
            if (!"Basic".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // If the scheme is not "Basic", fail authentication with a relevant message.
                return AuthenticateResult.Fail("Invalid Authorization Scheme");
            }

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter)).Split(':', 2);

            if (credentials.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Basic Authentication Credentials");
            }

            // Extract the email (username) and password from the decoded credentials.
            var username = credentials[0];
            var password = credentials[1];

            try
            {
                // Use the IUserService to validate the user credentials.
                var user = await _userService.ValidateUserAsync(username, password);
                if (user == null)
                {
                    // If no user matches the provided credentials, fail authentication.
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                // Indicate that authentication was successful and return the ticket
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch
            {
                // If any exception occurs during authentication, fail with a generic error message.
                return AuthenticateResult.Fail("Error occurred during authentication");
            }
        }
    }
}