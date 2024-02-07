using FishNet;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.UI;

public class MainNetworkMenu : MonoBehaviour
{
    [SerializeField] private Button _connectToServerButton;
    [SerializeField] private Button _connectAsClientButton;
    [SerializeField] private Button _startAsHostButton;

    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = InstanceFinder.NetworkManager;
        _connectToServerButton.onClick.AddListener(ConnectToServer);
        _connectAsClientButton.onClick.AddListener(ConnectAsClient);
        _startAsHostButton.onClick.AddListener(StartHost);
    }

    private void ConnectToServer()
    {
        _networkManager.ServerManager.StartConnection();
    }

    private void ConnectAsClient()
    {
        _networkManager.ClientManager.StartConnection();
    }

    private void StartHost()
    {
        _networkManager.ServerManager.StartConnection();
        _networkManager.ClientManager.StartConnection();
    }
}