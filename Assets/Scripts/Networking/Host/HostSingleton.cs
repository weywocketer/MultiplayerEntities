using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    public HostGameManager GameManager { get; private set; }
    static HostSingleton instance;
    public static HostSingleton Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindFirstObjectByType<HostSingleton>();

            if (instance == null)
            {
                Debug.LogError("No HostSingleton!");
                return null;
            }

            return instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
