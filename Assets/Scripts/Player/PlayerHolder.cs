using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public List<NetworkConnectionToClient> AllPlayers { get; } = new();
    
            // public static PlayerHolder Instanse { get; private set; }
            //
            // private void Awake()
            // {
            //     Instanse = this;
            //     FillListOfPlayer();
            // }
            //
            // private void FillListOfPlayer()
            // {
            //     // foreach (var player in transform.GetComponentsInChildren<Transform>())
            //     // {
            //     //     if (point == transform) continue;
            //     //     AllPlayers.Add(point);
            //     // }
            // }
}
