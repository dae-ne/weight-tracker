﻿using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace WeightTracker.Cli.Authentication;

/// <inheritdoc/>
/// <param name="authOptions">The authentication options.</param>
internal sealed class AuthService(IOptions<AuthOptions> authOptions) : IAuthService
{
    private const string EnvVariableName = "AUTH_TOKEN";

    /// <inheritdoc/>
    /// <remarks>
    /// This method uses the interactive authentication flow to acquire the access token.
    /// </remarks>
    public async Task AcquireTokenAsync(CancellationToken cancellationToken = default)
    {
        var (clientId, tenantId, redirectUri) = authOptions.Value;

        var scopes = new[] { $"api://{clientId}/access_as_user" };

        var options = new PublicClientApplicationOptions
        {
            ClientId = clientId,
            TenantId = tenantId,
            RedirectUri = redirectUri
        };

        var client = PublicClientApplicationBuilder
            .CreateWithApplicationOptions(options)
            .Build();

        var authResult = await client
            .AcquireTokenInteractive(scopes)
            .ExecuteAsync(cancellationToken);

        Environment.SetEnvironmentVariable(EnvVariableName, authResult.AccessToken, EnvironmentVariableTarget.User);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method retrieves the access token from the environment variable.
    /// It uses the user environment variable target, so the access token is stored between sessions.
    /// </remarks>
    public string? GetToken()
    {
        return Environment.GetEnvironmentVariable(EnvVariableName, EnvironmentVariableTarget.User);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method removes the access token from the environment variable.
    /// </remarks>
    public Task ForgetTokenAsync()
    {
        Environment.SetEnvironmentVariable(EnvVariableName, null, EnvironmentVariableTarget.User);
        return Task.CompletedTask;
    }
}
