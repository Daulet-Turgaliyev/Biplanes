using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class PlayerGUI : MonoBehaviour
    {
        public TextMeshProUGUI playerName;

        public void SetPlayerInfo(PlayerInfo info)
        {
            playerName.text = $"Player {info.playerIndex}";
            playerName.color = info.ready ? Color.green : Color.red;
        }
    }
