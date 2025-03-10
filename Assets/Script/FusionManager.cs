using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private GameObject networkRunnerPrefab;

    [Header("Fusion接続設定")]
    [SerializeField] private GameMode gameMode = GameMode.AutoHostOrClient;
    [SerializeField] private string sessionName = "TestSession";
    [SerializeField] private int maxPlayerCount = 4;
    [SerializeField] private bool isPublicSession = true;

    private NetworkRunner networkRunner;

    private void Awake()
    {
        //NetwrkRunnerのインスタンス
        networkRunner = Instantiate(networkRunnerPrefab).GetComponent<NetworkRunner>();
    }

    private void Start()
    {
        //コールバックの設定(あると便利)
        networkRunner.AddCallbacks(this);

        //Fusionに接続
        TryConnectFusion(gameMode, sessionName, maxPlayerCount, isPublicSession).Forget();
    }

    /// <summary>
    /// Fusionに接続を試みる
    /// </summary>
    /// <returns></returns>
    private async UniTask TryConnectFusion(GameMode mode, string name, int playerCount, bool isVisible)
    {
        //接続設定(モードやルーム名など)
        StartGameArgs startGameArgs = new StartGameArgs();
        startGameArgs.GameMode = mode;              //ゲームモード設定
        startGameArgs.SessionName = name;           //セッション名。同じ名前のセッションを指定すると、同じセッションに接続する
        startGameArgs.PlayerCount = playerCount;    //最大プレイヤー数
        startGameArgs.IsVisible = isVisible;        //セッションを公開するかどうか。falseだとSessionName指定しないとは入れない

        //Fusionに接続
        var result = await networkRunner.StartGame(startGameArgs);

        if (result.Ok)
        {
            Debug.Log("Fusionに接続しました");
            var info = networkRunner.SessionInfo;
            Debug.Log($"セッション名:{info.Name} プレイヤー数:{info.PlayerCount} 公開:{info.IsVisible}");
        }
        else
        {
            Debug.LogError("Fusionに接続できませんでした");
        }
    }

    #region NetworkCallback
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Fusionに接続しました");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError("Fusionに接続できませんでした");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("Fusionから切断されました");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("InInput");   
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("OnObjectEnterAOI");   
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("OnObjectExitAOI");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerLeft");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log("OnReliableDataProgress");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        Debug.Log("OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShutdown");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("OnUserSimulationMessage");
    }
    #endregion
}
