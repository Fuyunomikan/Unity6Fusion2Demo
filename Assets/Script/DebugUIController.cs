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
        Mode.text = $"HosrMode: {!runner.IsClient}";
        playerCount.text = $"Player: {runner.SessionInfo.PlayerCount}/{runner.SessionInfo.MaxPlayers}";
        isPublicSession.text = $"Public: {runner.SessionInfo.IsVisible}";
    }
}
