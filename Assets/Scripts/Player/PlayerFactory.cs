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
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            _playerName = player.name;
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
        }
    }
}