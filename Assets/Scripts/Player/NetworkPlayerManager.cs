using System.Collections;
using Mirror;
using Spawn;
using UnityEngine;

namespace Player
{
    public class NetworkPlayerManager : NetworkManager
    {
        public float sceneRestartDelay = 5f;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
        }

        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} {conn.connectionId+1}";
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
        }
        

        private IEnumerator RestartSceneDelayed()
        {
            yield return new WaitForSeconds(sceneRestartDelay);
            
            foreach (var conn in NetworkServer.connections.Values)
            {
                conn.Disconnect();
            }
            singleton.ServerChangeScene(NetworkManager.singleton.onlineScene);
        }
        
        public void EndOfGameSession()
        {
            StartCoroutine(RestartSceneDelayed());
        }
    }
}