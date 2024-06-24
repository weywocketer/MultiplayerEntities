using System;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
//using Unity.Netcode;
//using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

public class HostGameManager
{
    const int MaxConnections = 20;
    const string GameSceneName = "Game";
    Allocation allocation;
    string joinCode;
    string lobbyId;
    public TextMeshProUGUI codeText;
    

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
            codeText.text = joinCode;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        //RelayServerData relayServerData = HostRelayData(allocation, "dtls");
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

        //RelayServerData relayServerData = new RelayServerData(allocation, "wss");




        //ClientServerBootstrap.CreateClientWorld("");
        //TransportType
        //NetworkStreamDriver

        //allocation.



        try
        {
            CreateLobbyOptions lobbyOptions = new();
            lobbyOptions.IsPrivate = false; // Private lobbies won't show up, but you can still connect via join code.
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
        await ApplicationController.Instance.LaunchClient(joinCode);











        //NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single); // Only the server has to change the scene (clients will follow).
    }

    //IEnumerator HeartbeatLobby(float waitTime)
    //{
    //    WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(waitTime);

    //    while (true)
    //    {
    //        Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
    //        yield return waitForSeconds;
    //    }

    //}

}
