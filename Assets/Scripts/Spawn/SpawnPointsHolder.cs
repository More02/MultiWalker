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

        public static SpawnPointsHolder Instanse { get; private set; }

        [SerializeField] private Transform _plawnPointsHolder;

        private void Awake()
        {
            Instanse = this;
            FillListOfPoints();
        }

        public void FillListOfPoints()
        {
            foreach (var point in _plawnPointsHolder.GetComponentsInChildren<Transform>())
            {
                if (point == _plawnPointsHolder) continue;
                AllSpawnPoints.Add(point);
            }
        }
    }
}