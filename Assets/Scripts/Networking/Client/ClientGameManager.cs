using System;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ClientGameManager
{
    const string MenuSceneName = "Menu";
    const string GameSceneName = "Game";
    JoinAllocation allocation;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        AuthenticationState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthenticationState.Authenticated)
        {
            Debug.Log("auth complete! :)");
            return true;
        }

        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        GameBootstrap.CreateClientWorldWithRelayData(relayServerData);
    }
}
