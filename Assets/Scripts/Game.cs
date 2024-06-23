using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;

// Create a custom bootstrap, which enables auto-connect.
// The bootstrap can also be used to configure other settings as well as to
// manually decide which worlds (client and server) to create based on user input
[UnityEngine.Scripting.Preserve]
public class GameBootstrap : ClientServerBootstrap
{
    public override bool Initialize(string defaultWorldName)
    {
        //AutoConnectPort = 7979; // Enabled auto connect
        //return base.Initialize(defaultWorldName); // Use the regular bootstrap


        //CreateDefaultClientServerWorlds();

        //Create only a local simulation world without any multiplayer and netcode system in it.
        CreateLocalWorld(defaultWorldName);
        return true;


        // wait until relay is ready






        // server only for now hyhyhyhyhy

        //AutoConnectPort = 7979; // Enabled auto connect

        //NetworkStreamDriver a = new NetworkStreamDriver();



        //World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        //EntityQuery drvQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
        ////drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ClientServerBootstrap.DefaultListenAddress.WithPort(Port));


        //RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

        //drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(relayServerData.Endpoint);


        //return true;


        //DefaultDriverBuilder

        //ClientServerBootstrap.ServerWorld.GetOr

        //ClientServerBootstrap.ClientWorld.
        

        //ClientServerBootstrap.DefaultConnectAddress = 2;
        //DefaultConnectAddress =
        



        //return base.Initialize(defaultWorldName); // Use the regular bootstrap
    }




    public static void CreateServerWorldWithRelayData(RelayServerData relayServerData)
    {
        //NetworkStreamReceiveSystem.DriverConstructor = new RelayDriverConstructor(relayServerData, relayClientData);
        
        
        var oldConstructor = NetworkStreamReceiveSystem.DriverConstructor;
        NetworkStreamReceiveSystem.DriverConstructor = new RelayDriverConstructor(relayServerData, new RelayServerData());

        World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        NetworkStreamReceiveSystem.DriverConstructor = oldConstructor;



        //SceneManager.LoadScene("Game");
        //Destroy the local simulation world to avoid the game scene to be loaded into it
        //This prevent rendering (rendering from multiple world with presentation is not greatly supported)
        //and other issues.
        DestroyLocalSimulationWorld();
        if (World.DefaultGameObjectInjectionWorld == null)
            World.DefaultGameObjectInjectionWorld = serverWorld;



        //EntityQuery drvQueryServer = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
        //drvQueryServer.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ClientServerBootstrap.DefaultListenAddress.WithPort(7979));



        //On the server world, create a new entity with a NetworkStreamRequestConnect. It will be consumed by NetworkStreamReceiveSystem later.
        Entity listenRequest = serverWorld.EntityManager.CreateEntity(typeof(NetworkStreamRequestListen));
        serverWorld.EntityManager.SetComponentData(listenRequest, new NetworkStreamRequestListen { Endpoint = ClientServerBootstrap.DefaultListenAddress.WithPort(7979) });
        //serverWorld.EntityManager.SetComponentData(listenRequest, new NetworkStreamRequestListen { Endpoint = relayServerData.Endpoint });



















        //drvQueryServer.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(NetworkEndpoint.AnyIpv4);

        //serverWorld.

        //serverWorld.Flags

        //Unity.Entities.WorldFlags.ma

        //Unity.Entities.WorldFlags.Game.

        ////Debug.Log("is Web: " + relayServerData.ho);
        //World serverWorld = ClientServerBootstrap.CreateServerWorld("ServerWorld");
        //World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        //EntityQuery drvQueryServer = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
        //EntityQuery drvQueryClient = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());


        ////drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(relayServerData.Endpoint);

        //drvQueryServer.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ClientServerBootstrap.DefaultListenAddress.WithPort(7979));

        ////drvQueryServer.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(NetworkEndpoint.AnyIpv4);
        ////drvQueryClient.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, relayServerData.Endpoint);
        //drvQueryClient.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, ClientServerBootstrap.DefaultConnectAddress.WithPort(7979));


        //drvQueryClient.GetSingletonRW<NetworkStreamConnection>().ValueRW.

        //Debug.Log( "use relay: " + drvQueryClient.GetSingletonRW<NetworkStreamDriver>().ValueRO.UseRelay(drvQueryClient.GetSingletonRW<NetworkStreamConnection>().ValueRO) );


        //Unity.NetCode.TransportType.
        //transport.SetRelayServerData(relayServerData);


        //EntityQuery drvQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());

        //DefaultDriverBuilder.
        //DefaultDriverBuilder.ClientUseSocketDriver();



        //DefaultDriverBuilder.DefaultDriverConstructor.



        //var networkStreamEntity = serverWorld.EntityManager.CreateEntity(ComponentType.ReadWrite<NetworkStreamRequestConnect>());





        //drvQuery.GetSingletonRW<NetworkStreamConnection>().ValueRW.

        //NetworkDriver

        //drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.





        //EntityQuery drvQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<Unity.NetCode.drivce>());

        //DefaultDriverBuilder.RegisterClientUdpDriver(world, ref driverStore, netDebug, settings);
        //EntityQuery drvQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStre>());

        //drvQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(ClientServerBootstrap.DefaultListenAddress.WithPort(Port));





    }


    public static void CreateClientWorldWithRelayData(RelayServerData relayServerData)
    {
        var oldConstructor = NetworkStreamReceiveSystem.DriverConstructor;

        NetworkStreamReceiveSystem.DriverConstructor = new RelayDriverConstructor(new RelayServerData(), relayServerData);


        World clientWorld = ClientServerBootstrap.CreateClientWorld("ClientWorld");
        //EntityQuery drvQueryClient = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
        //drvQueryClient.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, relayServerData.Endpoint);

        NetworkStreamReceiveSystem.DriverConstructor = oldConstructor;

        SceneManager.LoadScene("Game");
        //Destroy the local simulation world to avoid the game scene to be loaded into it
        //This prevent rendering (rendering from multiple world with presentation is not greatly supported)
        //and other issues.
        DestroyLocalSimulationWorld();
        if (World.DefaultGameObjectInjectionWorld == null)
            World.DefaultGameObjectInjectionWorld = clientWorld;



        //On the client world, create a new entity with a NetworkStreamRequestConnect. It will be consumed by NetworkStreamReceiveSystem later.
        var connectRequest = clientWorld.EntityManager.CreateEntity(typeof(NetworkStreamRequestConnect));
        clientWorld.EntityManager.SetComponentData(connectRequest, new NetworkStreamRequestConnect { Endpoint = relayServerData.Endpoint });



        //drvQueryClient.get

        //drvQueryClient.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, ClientServerBootstrap.DefaultConnectAddress.WithPort(7979));

    }

    static void DestroyLocalSimulationWorld()
    {
        foreach (var world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                //OldFrontendWorldName = world.Name;
                world.Dispose();
                break;
            }
        }
    }
}