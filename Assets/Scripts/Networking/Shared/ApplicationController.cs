using System.Threading.Tasks;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Collections;
using Unity.Services.Core;

/// <summary>
/// Singleton that manages the high level networking aspects.
/// </summary>
public class ApplicationController : MonoBehaviour
{
    bool authenticated = false;
    public static ApplicationController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("ApplicationController already exists!");
        }
    }

    async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            // TODO: Implement dedicated server init here.
        }
        else
        {
            authenticated = await InitAsync();
        }

    }

    public async Task LaunchHost()
    {
        if (!authenticated) { return; }

        HostUtils hostGameManager = new();
        await hostGameManager.StartHostAsync();
    }

    public async Task LaunchClient(string joinCode)
    {
        if (!authenticated) { return; }

        ClientUtils clientGameManager = new();
        await clientGameManager.StartClientAsync(joinCode);
    }

    public void StartHeartbeatLobby(float waitTime, string lobbyId)
    {
        StartCoroutine(HeartbeatLobby(waitTime, lobbyId));
    }

    IEnumerator HeartbeatLobby(float waitTime, string lobbyId)
    {
        WaitForSecondsRealtime waitForSeconds = new WaitForSecondsRealtime(waitTime);

        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return waitForSeconds;
        }
    }

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        AuthenticationState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthenticationState.Authenticated)
        {
            Debug.Log("UGS authentication complete!");
            return true;
        }

        return false;
    }
}
