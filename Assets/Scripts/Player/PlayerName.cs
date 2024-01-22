using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Player
{
    /// <summary>
    /// Класс, отвечающий за изменение имени пользователей
    /// </summary>
    public class PlayerName : NetworkBehaviour
    {
        [field: SyncVar(OnChange = nameof(OnNameUpdate))]
        private string Name { get; set; }

        private void Start()
        {
            var playerGameObject = gameObject;
            FillPlayerInfo.Instance.PlayerNames.Add(playerGameObject.name);
            Name = playerGameObject.name;
        }

        private void OnNameUpdate(string oldName, string newName)
        {
            var playerNames = FillPlayerInfo.Instance.PlayerNames;
            if (playerNames.Contains(gameObject.name)) playerNames[playerNames.IndexOf(gameObject.name)] = Name;
            gameObject.name = Name;
        }

       
    }
}