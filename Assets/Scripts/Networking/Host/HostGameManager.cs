using System;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using TMPro;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

public class HostGameManager
{
    const int MaxConnections = 20;
    const string GameSceneName = "Game";
    Allocation allocation;
    string joinCode;
    string lobbyId;

    public async Task StartHostAsync()
    {
		try
		{
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
		{
            Debug.LogError(e);
            return;
		}

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

        try
        {
            CreateLobbyOptions lobbyOptions = new();
            lobbyOptions.IsPrivate = false; // Private lobbies won't show up, but players can still connect via join code.
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                         visibility: DataObject.VisibilityOptions.Member,
                         value: joinCode
                    )
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("Multiplayer-Entities Test Server", MaxConnections, lobbyOptions);
            lobbyId = lobby.Id;

            ApplicationController.Instance.StartHeartbeatLobby(15, lobbyId);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            return;
        }

        GameBootstrap.CreateServerWorldWithRelayData(relayServerData);
        new WaitForSecondsRealtime(1);
        // After the server is set up, join as a client (as it's host-server).
        await ApplicationController.Instance.LaunchClient(joinCode);
    }

}
