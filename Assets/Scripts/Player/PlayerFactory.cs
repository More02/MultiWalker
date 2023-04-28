using Mirror;
using Spawn;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerFactory : NetworkManager
    {
        [SerializeField] private GameObject _playerInfoPrefab;
        
        //    private GameObject prefabPlayerInfoPanel;

        private string _playerName;
        private NetworkConnectionToClient _conn;
        private GameObject _player;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            
            InstantiatePlayer(conn);
            //var playerInfoObject = InfoCanvas.Instance.transform.root.GetComponent<InfoCanvas>();
            //playerInfoObject.InstantiatePlayerInfoPanel(_playerName, _canvasPanelHolder);
            // _canvasPanelHolder.transform.root.GetComponent<PlayerInfo>()
            //     .RpcInstantiatePlayerInfoPanel(_playerName, _canvasPanelHolder);
            //playerInfoObject.CmdInstantiatePlayerInfoPanel(_playerName);
        }

        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            _conn = conn;
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            _player = player;
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            _playerName = player.name;
            // player.GetComponent<CreateInfoPrefab>().CmdSetPlayerName(player.GetComponent<NetworkIdentity>(), _playerName);
            //NetworkClient.localPlayer.gameObject.name = _playerName;
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
            player.GetComponent<PlayerName>().CmdChangeName(_playerName);
        }

        // public override void OnStartClient()
        // {
        //     _player.name = $"{playerPrefab.name} [connId={_conn.connectionId}]";
        //     _playerName = _player.name;
        //     _player.GetComponent<CreateInfoPrefab>().CmdSetPlayerName(_player.GetComponent<NetworkIdentity>(), _playerName);
        // }
    }
}