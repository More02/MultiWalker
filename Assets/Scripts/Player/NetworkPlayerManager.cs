using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Spawn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class NetworkPlayerManager : NetworkManager
    {
        private const float SceneRestartDelay = 5f;
        private readonly List<NetworkConnection> _clientConnections = new();

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            InstantiatePlayer(conn);
            _clientConnections.Add(conn);
        }

        private void InstantiatePlayer(NetworkConnectionToClient conn)
        {
            var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
            var player = Instantiate(playerPrefab, SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position,
                SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation);
            player.name = $"{playerPrefab.name} {conn.connectionId + 1}";
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
        }

        public void EndOfGameSession()
        {
            StartCoroutine(RestartSceneDelayed());
        }

        private IEnumerator RestartSceneDelayed()
        {
            yield return new WaitForSeconds(SceneRestartDelay);

            var networkManager = gameObject.GetComponent<NetworkManager>();
            if (networkManager is null)
            {
                yield break;
            }
            
            foreach (var conn in NetworkServer.connections.Values)
            {
                conn.Disconnect();
            }

            // networkManager.StopServer();
            // networkManager.StartServer();

            const string host = "localhost";
            const int port = 8080;
            foreach (var uri in _clientConnections.Select(conn => $"tcp://{host}:{port}/{conn.connectionId}").Select(url => new Uri(url)))
            {
                networkManager.StartClient(uri);
            }
            
            singleton.ServerChangeScene(singleton.onlineScene);
            _clientConnections.Clear();
        }
    }
}