using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Player;
using TMPro;
using UnityEngine;

public class CreateInfoPrefab : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        //SyncListOfPlayers - нужно синхронизировать список плееров
        CmdInstantiatePlayerInfoPanel(gameObject.name);
    }

    [Command]
    private void CmdInstantiatePlayerInfoPanel(string playerName)
    {
        RpcInstantiatePlayerInfoPanel(playerName);
        InstantiatePlayerInfoPanel(playerName);
    }
    [ClientRpc]
    private void RpcInstantiatePlayerInfoPanel(string playerName)
    {
        if (!isServer) InstantiatePlayerInfoPanel(playerName);
    }

    private void InstantiatePlayerInfoPanel(string playerName)
    {
        var playerInfoPanel = Instantiate(InfoCanvas.Instance.PlayerInfoPrefab, InfoCanvas.Instance.CanvasPanelHolder);
        //NetworkServer.Spawn(playerInfoPanel);
        InfoCanvas.FirstFillPlayerInfo(playerName, playerInfoPanel);
    }
}
