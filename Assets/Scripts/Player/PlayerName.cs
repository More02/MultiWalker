using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using TMPro;

namespace Player
{
    public class PlayerName : NetworkBehaviour
    {
        [field: SyncVar(hook = nameof(OnNameUpdate))]
        private string Name { get; set; }

        private string _oldName;

        private void Awake()
        {
            _oldName = gameObject.name;
        }

        private void Start()
        {
            InfoCanvas.Instance.PlayerNames.Add(gameObject.name);
        }

        private async void OnNameUpdate(string oldName, string name)
        {
            //oldName = gameObject.name;
            var playerNames = InfoCanvas.Instance.PlayerNames;
            if (playerNames.Contains(gameObject.name)) playerNames[playerNames.IndexOf(gameObject.name)] = Name;
            gameObject.name = Name;
            await InfoCanvas.Instance.RenameAllPlayers();
            //await RenameAllPlayers();
            // var holder = InfoCanvas.Instance.CanvasPanelHolder;
            // for (var i = 0; i < holder.childCount; i++)
            // {
            //     var playerNamePrefab = holder.GetChild(i).GetChild(0).GetComponent<TMP_Text>();
            //     if (playerNamePrefab.text == oldName) playerNamePrefab.SetText(Name);
            //     Debug.Log(playerNamePrefab.text);
            // }
        }

        

        [Command(requiresAuthority = false)]
        public void CmdChangeName(string playerName)
        {
            RpcChangeName(playerName);
            //ChangeName(playerName);
        }

        [ClientRpc]
        private void RpcChangeName(string playerName)
        {
            ChangeName(playerName);
        }

        private async void ChangeName(string playerName)
        {
            gameObject.name = playerName;
            Name = playerName;
            await InfoCanvas.Instance.RenameAllPlayers();
           // await RenameAllPlayers();
        }
    }
}