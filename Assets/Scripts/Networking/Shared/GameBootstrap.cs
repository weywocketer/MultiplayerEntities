using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;

/// <summary>
/// Creates local world when initialized. Contains methonds for creating server/client worlds with connection parameters based on RelayServerData.
/// </summary>
[UnityEngine.Scripting.Preserve]
public class GameBootstrap : ClientServerBootstrap
{
    public override bool Initialize(string defaultWorldName)
    {
        //Create only a local simulation world without any multiplayer and netcode system in it.
        CreateLocalWorld(defaultWorldName);
        return true;
    }

    public static void CreateServerWorldWithRelayData(RelayServerData relayServerData)
    {
        var oldConstructor = NetworkStreamReceiveSystem.DriverConstructor;
        NetworkStreamReceiveSystem.DriverConstructor = new RelayDriverConstructor(relayServerData, new RelayServerData());

        World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        NetworkStreamReceiveSystem.DriverConstructor = oldConstructor;

        DestroyLocalSimulationWorld();
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            World.DefaultGameObjectInjectionWorld = serverWorld;
        }

        Entity listenRequest = serverWorld.EntityManager.CreateEntity(typeof(NetworkStreamRequestListen));
        serverWorld.EntityManager.SetComponentData(listenRequest, new NetworkStreamRequestListen { Endpoint = ClientServerBootstrap.DefaultListenAddress.WithPort(7979) });
    }

    public static void CreateClientWorldWithRelayData(RelayServerData relayServerData)
    {
        var oldConstructor = NetworkStreamReceiveSystem.DriverConstructor;
        NetworkStreamReceiveSystem.DriverConstructor = new RelayDriverConstructor(new RelayServerData(), relayServerData);

        World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        NetworkStreamReceiveSystem.DriverConstructor = oldConstructor;

        SceneManager.LoadScene("Game");

        DestroyLocalSimulationWorld();
        if (World.DefaultGameObjectInjectionWorld == null)
        {
            World.DefaultGameObjectInjectionWorld = clientWorld;
        }

        var connectRequest = clientWorld.EntityManager.CreateEntity(typeof(NetworkStreamRequestConnect));
        clientWorld.EntityManager.SetComponentData(connectRequest, new NetworkStreamRequestConnect { Endpoint = relayServerData.Endpoint });
    }

    static void DestroyLocalSimulationWorld()
    {
        foreach (var world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        }
    }

}