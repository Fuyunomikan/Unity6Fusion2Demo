using Fusion;
using TMPro;
using UnityEngine;

public class DebugUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sessionName;
    [SerializeField] private TextMeshProUGUI Mode;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI isPublicSession;

    public void SetData(NetworkRunner runner)
    {
        sessionName.text = $"SessionName: {runner.SessionInfo.Name}";
        if(runner.GameMode == GameMode.Shared)
        {
            var authority = runner.IsSharedModeMasterClient ? "Master" : "Client";
            Mode.text = $"Mode: Shered {authority}";
        }
        else
        {
            var authority = runner.IsClient ? "Client" : "Host";
            Mode.text = $"Mode: {authority}";
        }
        playerCount.text = $"Player: {runner.SessionInfo.PlayerCount}/{runner.SessionInfo.MaxPlayers}";
        isPublicSession.text = $"Public: {runner.SessionInfo.IsVisible}";
    }
}
