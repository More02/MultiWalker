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

        private string _playerName;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
            InstantiatePlayerInfo();
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

        private void InstantiatePlayerInfo()
        {
            var playerInfoPanel = Instantiate(_playerInfoPrefab, _canvasPanel);
            var itemFromInfoPanel = playerInfoPanel.transform;
            itemFromInfoPanel.GetChild(0).GetComponent<TMP_Text>().text = _playerName;
            itemFromInfoPanel.GetChild(1).GetComponent<TMP_Text>().text = 0.ToString();
        }
    }
}