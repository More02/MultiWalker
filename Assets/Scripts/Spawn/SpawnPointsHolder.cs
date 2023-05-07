using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    /// <summary>
    /// Класс, отвечающий за хранение точек спавна
    /// </summary>
    public class SpawnPointsHolder : MonoBehaviour
    {
        public List<Transform> AllSpawnPoints { get; } = new();

        public static SpawnPointsHolder Instance { get; private set; }

        [SerializeField] private Transform _spawnPointsHolder;

        private void Awake()
        {
            Instance = this;
            FillListOfPoints();
        }

        public void FillListOfPoints()
        {
            foreach (var point in _spawnPointsHolder.GetComponentsInChildren<Transform>())
            {
                if (point == _spawnPointsHolder) continue;
                AllSpawnPoints.Add(point);
            }
        }
    }
}