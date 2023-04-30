using Mirror;
using Spawn;
using UnityEngine;

namespace Player
{
    public class PlayerFactory : NetworkManager
    {
        private string _playerName;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
        }

        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} {conn.connectionId}";
            _playerName = player.name;
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
            if (NetworkClient.active) player.GetComponent<PlayerName>().CmdChangeName(_playerName);
            else player.GetComponent<PlayerName>().RpcChangeName(_playerName);
        }
    }
}