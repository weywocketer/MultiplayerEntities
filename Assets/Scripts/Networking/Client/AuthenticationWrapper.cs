using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

/// <summary>
/// Wrapper class used when authenticating to the Unity Gaming Services.
/// </summary>
public static class AuthenticationWrapper
{
    public static AuthenticationState AuthState { get; private set; } = AuthenticationState.NotAuthenticated;

    public static async Task<AuthenticationState> DoAuth(int maxRetries = 5)
    {
        if (AuthState == AuthenticationState.Authenticated)
        {
            return AuthState;
        }

        if (AuthState == AuthenticationState.Authenticating)
        {
            Debug.LogWarning("Already authenticating!");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxRetries);

        return AuthState;
    }

    static async Task<AuthenticationState> Authenticating()
    {
        while(AuthState == AuthenticationState.Authenticating || AuthState == AuthenticationState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    static async Task SignInAnonymouslyAsync(int maxRetries)
    {
        AuthState = AuthenticationState.Authenticating;

        int tries = 0;
        while (AuthState == AuthenticationState.Authenticating && tries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthenticationState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authException)
            {
                Debug.LogError(authException);
                AuthState = AuthenticationState.Error;
            }
            catch (RequestFailedException requestException)
            {
                Debug.LogError(requestException);
                AuthState = AuthenticationState.Error;
            }

            tries++;
            await Task.Delay(1000);
        }

        if (AuthState != AuthenticationState.Authenticated)
        {
            Debug.LogWarning("Player authentication time out.");
            AuthState = AuthenticationState.TimeOut;
        }
    }

}

public enum AuthenticationState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}