using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private GameObject networkRunnerPrefab;
    [SerializeField] private DebugUIController debugUIController;

    [Header("Fusion�ڑ��ݒ�")]
    [SerializeField] private GameMode gameMode = GameMode.AutoHostOrClient;
    [SerializeField] private string sessionName = "TestSession";
    [SerializeField] private int maxPlayerCount = 4;
    [SerializeField] private bool isPublicSession = true;

    private NetworkRunner networkRunner;
    public NetworkRunner NetworkRunner => networkRunner;

    private void Awake()
    {
        //NetwrkRunner�̃C���X�^���X
        networkRunner = Instantiate(networkRunnerPrefab).GetComponent<NetworkRunner>();
    }

    private void Start()
    {
        //�R�[���o�b�N�̐ݒ�(����ƕ֗�)
        networkRunner.AddCallbacks(this);
    }

    private void TryConnectLobby()
    {
    }

    /// <summary>
    /// Fusion�ɐڑ������݂�
    /// </summary>
    /// <returns></returns>
    private async UniTask TryConnectSession(GameMode mode, string name, int playerCount, bool isVisible)
    {
        //�ڑ��ݒ�(���[�h�⃋�[�����Ȃ�)
        StartGameArgs startGameArgs = new StartGameArgs();
        startGameArgs.GameMode = mode;              //�Q�[�����[�h�ݒ�
        startGameArgs.SessionName = name;           //�Z�b�V�������B�������O�̃Z�b�V�������w�肷��ƁA�����Z�b�V�����ɐڑ�����
        startGameArgs.PlayerCount = playerCount;    //�ő�v���C���[��
        startGameArgs.IsVisible = isVisible;        //�Z�b�V���������J���邩�ǂ����Bfalse����SessionName�w�肵�Ȃ��Ƃ͓���Ȃ�

        //Fusion�ɐڑ�
        var result = await networkRunner.StartGame(startGameArgs);

        if (result.Ok)
        {
            Debug.Log("Fusion�ɐڑ����܂���");
            var info = networkRunner.SessionInfo;
            Debug.Log($"�Z�b�V������:{info.Name} �v���C���[��:{info.PlayerCount} ���J:{info.IsVisible}");
        }
        else
        {
            Debug.LogError("Fusion�ɐڑ��ł��܂���ł���");
        }
    }

    #region NetworkCallback
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Fusion�ɐڑ����܂���");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError("Fusion�ɐڑ��ł��܂���ł���");
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
        Debug.Log("Fusion����ؒf����܂���");
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
        debugUIController.SetData(runner);
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

    #region DEBUG
    public void DEBUG_JoinSessionBySetting()
    {
        TryConnectSession(gameMode, sessionName, maxPlayerCount, isPublicSession).Forget();
    }

    public void DEBUG_DisconnectSession()
    {
        networkRunner.Disconnect(networkRunner.LocalPlayer);
    }
    #endregion
}

//Inspector�g��
[CustomEditor(typeof(FusionManager))]
public class FusionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var fusionManager = target as FusionManager;

        if (fusionManager.NetworkRunner == null) return;

        //Fusion�ɐڑ���
        if (fusionManager.NetworkRunner.State == NetworkRunner.States.Shutdown)
        {
            if (GUILayout.Button("Fusion�ɐڑ�"))
            {
                fusionManager.DEBUG_JoinSessionBySetting();
            }
        }
        else
        {
            if (GUILayout.Button("�ؒf"))
            {
                fusionManager.DEBUG_DisconnectSession();
            }
        }
    }
}
