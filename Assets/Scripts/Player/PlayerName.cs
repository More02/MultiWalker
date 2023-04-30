using Mirror;

namespace Player
{
    public class PlayerName : NetworkBehaviour
    {
        [field: SyncVar(hook = nameof(OnNameUpdate))]
        private string Name { get; set; }

        private void Start()
        {
            InfoCanvas.Instance.PlayerNames.Add(gameObject.name);
        }

        private async void OnNameUpdate(string oldName, string name)
        {
            var playerNames = InfoCanvas.Instance.PlayerNames;
            if (playerNames.Contains(gameObject.name)) playerNames[playerNames.IndexOf(gameObject.name)] = Name;
            gameObject.name = Name;
            await InfoCanvas.Instance.RenameAllPlayers();
        }

        [Command(requiresAuthority = false)]
        public void CmdChangeName(string playerName)
        {
            RpcChangeName(playerName);
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
        }
    }
}