using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class LobbyViewWindow : BaseWindow
{
    public Guid SelectedMatch = Guid.Empty;
    
    
    [Inject] 
    private NetworkHandler _networkHandler;

    [Header("UI Groupe")]
    [SerializeField] 
    private GameObject _matchList;
    [SerializeField] 
    private MatchGUI _matchGUIPrefab;
    [SerializeField] 
    private Button _joinButton;
    [SerializeField]
    private ToggleGroup _toggleGroup;

    public void RequestStopClient()
    {
        _networkHandler.AllDisconnect();
    }
    
    public void RequestJoinMatch()
    {
        if (NetworkClient.active == false || SelectedMatch == Guid.Empty) return;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Join, matchId = SelectedMatch });
    }
    
    public void RequestCreateMatch()
    {
        if (!NetworkClient.active) return;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Create });
    }
    
    public void RefreshMatchList()
    {
        foreach (Transform child in _matchList.transform)
        {
            Destroy(child.gameObject);
        }

        _joinButton.interactable = false;

        foreach (MatchInfo matchInfo in _networkHandler.GetOpenMatches.Values)
        {
            var matchGUI = InstanceMatchObject();
            matchGUI.Init(this, matchInfo, _networkHandler, _joinButton);
            matchGUI.ToggleButton.group = _toggleGroup;
            if (matchInfo.matchId == SelectedMatch)
                matchGUI.ToggleButton.isOn = true;
        }
    }

    public void ShowLobbyView()
    {
        foreach (Transform child in _matchList.transform)
        {
            if (child.gameObject.GetComponent<MatchGUI>().GetMatchId != SelectedMatch) continue;
            
            Toggle toggle = child.gameObject.GetComponent<Toggle>();
            toggle.isOn = true;
            //toggle.onValueChanged.Invoke(true);
        }
    }

    private MatchGUI InstanceMatchObject()
    {
        var newMatchObject = Instantiate(_matchGUIPrefab, Vector3.zero, Quaternion.identity).gameObject;
        newMatchObject.transform.SetParent(_matchList.transform, false);
        return newMatchObject.GetComponent<MatchGUI>();
    }
}
