using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField joinCodeField;

    public async void StartHost()
    {
        await ApplicationController.Instance.LaunchHost();
    }

    public async void StartClient()
    {
        await ApplicationController.Instance.LaunchClient(joinCodeField.text);
    }
}
