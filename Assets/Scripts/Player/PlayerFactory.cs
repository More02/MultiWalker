using Mirror;
using Spawn;
using UnityEngine;

namespace Player
{
    public class PlayerFactory : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiateCard(conn);
        }

        private void InstantiateCard(NetworkConnectionToClient conn)
        {
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
        }
    }
}