using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbiesList : MonoBehaviour
{
    [SerializeField] Transform lobbyItemParent;
    [SerializeField] LobbyItem lobbyItemPrefab;
    bool isJoining = false;
    bool isRefreshing = false;

    void OnEnable()
    {
        RefreshList();
    }

    public async void RefreshList()
    {
        if (isRefreshing) { return; }
        isRefreshing = true;

        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25; // how many lobbies we'll get

            options.Filters = new List<QueryFilter>()
            {
                // Filter out lobbies without avaliable slots.
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT, // GT - greater
                    value: "0"
                ),
                // Filter out locked lobbies.
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ, // EQ - equal
                    value: "0"
                )
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            // Remove old lobbies from UI.
            foreach (Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
                LobbyItem lobbyItem = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyItem.Initialize(this, lobby);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }

        isRefreshing = false;
    }

    public async void JoinAsync(Lobby lobby) // void vs Task: when calling void async you cannot await (but the method is still async)
    {
        if (isJoining) { return; }
        isJoining = true;

		try
		{
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value;

            //await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
            await ApplicationController.Instance.LaunchClient(joinCode);
		}
		catch (LobbyServiceException e)
		{
            Debug.LogError(e);
        }

        isJoining = false;
    }

}
