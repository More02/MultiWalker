using Mirror;
using Spawn;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerFactory : NetworkManager
    {
        [SerializeField] private GameObject _playerInfoPrefab;
        [SerializeField] private Transform _canvasPanel;
        private GameObject prefabplayerInfoPanel;
        
        private string _playerName;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
            var playerInfoPanel = Instantiate(_playerInfoPrefab, _canvasPanel);
            prefabplayerInfoPanel = playerInfoPanel;
            NetworkServer.Spawn(playerInfoPanel);
            PlayerInfo.FillPlayerInfo(playerInfoPanel, _playerName, _canvasPanel);
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