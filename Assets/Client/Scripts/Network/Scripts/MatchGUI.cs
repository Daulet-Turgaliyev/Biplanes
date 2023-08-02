using System;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


    public class MatchGUI : MonoBehaviour
    {
        public Guid GetMatchId { get; private set; }

        [Header("GUI Elements")]
        [SerializeField]
        private Image _image;
        
        [field:SerializeField]
        public Toggle ToggleButton { get; private set; }
        
        [SerializeField]
        private TextMeshProUGUI _matchName;
        
        [SerializeField]
        private TextMeshProUGUI _playerCount;

        private Button _joinButton;
        private LobbyViewWindow _lobbyViewWindow;
        private NetworkHandler _networkHandler;
        
        public void Init(LobbyViewWindow lobbyViewWindow, MatchInfo infos, NetworkHandler networkHandler, Button joinButton)
        {
            _networkHandler = networkHandler;
            _lobbyViewWindow = lobbyViewWindow;
            _joinButton = joinButton;
            SetMatchInfo(infos);
            ToggleButton.onValueChanged.AddListener(delegate { OnToggleClicked(); });
        }

        private void OnToggleClicked()
        {
            SelectMatch(ToggleButton.isOn ? GetMatchId : Guid.Empty);
            _image.color = ToggleButton.isOn ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 1f, 1f, 0.2f);
        }

        private void SetMatchInfo(MatchInfo infos)
        {
            GetMatchId = infos.matchId;
            _matchName.text = $"Match {infos.matchId.ToString().Substring(0, 8)}";
            _playerCount.text = $"{infos.players} / {infos.maxPlayers}";
        }

        private void SelectMatch(Guid matchId)
        {
            if (NetworkClient.active == false) return;

            if (matchId == Guid.Empty)
            {
                _lobbyViewWindow.SelectedMatch = Guid.Empty;
                _joinButton.interactable = false;
            }
            else
            {
                if (!_networkHandler.GetOpenMatches.Keys.Contains(matchId))
                {
                    _joinButton.interactable = false;
                    return;
                }

                _lobbyViewWindow.SelectedMatch = matchId;
                MatchInfo infos = _networkHandler.GetOpenMatches[matchId];
                _joinButton.interactable = infos.players < infos.maxPlayers;
            }
        }
    }

