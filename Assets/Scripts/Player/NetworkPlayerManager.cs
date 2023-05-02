using System.Collections;
using System.Collections.Generic;
using Mirror;
using Movement;
using Spawn;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class NetworkPlayerManager : NetworkManager
    {
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
            yield return new WaitForSeconds(EndGame.Instance.DelayTime);

            var networkManager = gameObject.GetComponent<NetworkManager>();
            if (networkManager is null)
            {
                yield break;
            }

            SpawnPointsHolder.Instanse.FillListOfPoints();

            for (var i = 0; i < InfoCanvas.Instance.PlayerScore.Count; i++)
            {
                InfoCanvas.Instance.PlayerScore[i] = 0;
            }
            
            foreach (var playerIdentity in NetworkClient.spawned.Values)
            {
                var randomPlace = Random.Range(0, SpawnPointsHolder.Instanse.AllSpawnPoints.Count - 1);
                var player = playerIdentity.transform;
                player.position = SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].position;
                player.rotation = SpawnPointsHolder.Instanse.AllSpawnPoints[randomPlace].rotation;
                SpawnPointsHolder.Instanse.AllSpawnPoints.RemoveAt(randomPlace);
                player.gameObject.GetComponent<Stats>().CmdChangeScore(0, player.gameObject.name);
                player.gameObject.GetComponent<DashAbility>().CountOfSuccessDash = 0;
                EndGame.Instance.WinCanvas.SetActive(false);
            }





            // foreach (var conn in NetworkServer.connections.Values)
            // {
            //     conn.Disconnect();
            // }

            // // networkManager.StopServer();
            // // networkManager.StartServer();
            //
            // const string host = "localhost";
            // const int port = 8080;
            // foreach (var uri in _clientConnections.Select(conn => $"tcp://{host}:{port}/{conn.connectionId}").Select(url => new Uri(url)))
            // {
            //     networkManager.StartClient(uri);
            // }
            //
            // singleton.ServerChangeScene(singleton.onlineScene);
            // _clientConnections.Clear();
        }
    }
}