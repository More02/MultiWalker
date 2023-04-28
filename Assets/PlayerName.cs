using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [field: SyncVar] private string Name { get; set; }
    
    
    [Command(requiresAuthority = false)]
    public void CmdChangeName(string playerName)
    {
        RpcChangeName(playerName);
        ChangeName(playerName);
    }

    [ClientRpc]
    private void RpcChangeName(string playerName)
    {
        ChangeName(playerName);
    }

    private void ChangeName(string playerName)
    {
        gameObject.name = playerName;
        Name = playerName;
    }

}
