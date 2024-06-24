using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Collections;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] ClientSingleton clientPrefab;
    [SerializeField] HostSingleton hostPrefab;
    [SerializeField] TextMeshProUGUI codeText;
    bool authenticated = false;
    ClientGameManager clientGameManager;
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

        }
        else
        {
            //HostSingleton hostSingleton = Instantiate(hostPrefab);
            //hostSingleton.CreateHost();

            //ClientSingleton clientSingleton = Instantiate(clientPrefab);
            //bool authenticated = await clientSingleton.CreateClient();

            //if (authenticated)
            //{
            //    clientSingleton.GameManager.GoToMenu();
            //}

            clientGameManager = new();
            authenticated = await clientGameManager.InitAsync();

            //await clientGameManager.StartClientAsync();

        }

         

    }

    public async Task LaunchHost()
    {
        HostGameManager hostGameManager = new();
        hostGameManager.codeText = codeText;
        await hostGameManager.StartHostAsync();
    }

    public async Task LaunchClient(string joinCode)
    {
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
}
