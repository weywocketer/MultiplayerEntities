using System.Threading.Tasks;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Collections;
using Unity.Services.Core;

public class ApplicationController : MonoBehaviour
{
    //[SerializeField] ClientSingleton clientPrefab;
    //[SerializeField] HostSingleton hostPrefab;
    //[SerializeField] TextMeshProUGUI codeText;
    bool authenticated = false;
    //ClientGameManager clientGameManager;
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
            //clientGameManager = new();
            //authenticated = await clientGameManager.InitAsync();
        }

    }

    public async Task LaunchHost()
    {
        if (!authenticated) { return; }
        HostGameManager hostGameManager = new();
        //hostGameManager.codeText = codeText;
        await hostGameManager.StartHostAsync();
    }

    public async Task LaunchClient(string joinCode)
    {
        if (!authenticated) { return; }
        ClientGameManager clientGameManager = new();
        await clientGameManager.StartClientAsync(joinCode);
        //clientGameManager.GoToMenu();
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
            Debug.Log("auth complete! :)");
            return true;
        }

        return false;
    }
}
