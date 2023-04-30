using Mirror;
using Movement;

namespace Player
{
    public class PlayerName : NetworkBehaviour
    {
        [field: SyncVar(hook = nameof(OnNameUpdate))]
        private string Name { get; set; }

        private void Start()
        {
            InfoCanvas.Instance.PlayerNames.Add(gameObject.name);
            InfoCanvas.Instance.PlayerScore.Add(GetComponent<DashAbility>().CountOfSuccessDash);
            Name = gameObject.name;
        }

        private void OnNameUpdate(string oldName, string name)
        {
            var playerNames = InfoCanvas.Instance.PlayerNames;
            if (playerNames.Contains(gameObject.name)) playerNames[playerNames.IndexOf(gameObject.name)] = Name;
            gameObject.name = Name;
        }

        [Command(requiresAuthority = false)]
        public void CmdChangeName(string playerName)
        {
            RpcChangeName(playerName);
            ChangeName(playerName);
        }
        
        [ClientRpc]
        public void RpcChangeName(string playerName)
        {
            if (!isServer) ChangeName(playerName);
        }
        
        private async void ChangeName(string playerName)
        {
            gameObject.name = playerName;
            Name = playerName;
            await InfoCanvas.Instance.RenameAllPlayers();
        }
    }
}