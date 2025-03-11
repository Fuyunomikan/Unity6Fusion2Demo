using UnityEngine;
using Fusion;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;

public class FusionPlayerManager : NetworkBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform hpRoot;
    [SerializeField] private TextMeshPro hpText;

    //ネットワーク共有されるデータ
    [Networked, OnChangedRender(nameof(UpdateHpUI))] 
    public int HP { get; set; } = 100;

    private Vector3 move;

    public override void FixedUpdateNetwork()
    {
        //HPのUIをカメラに向ける
        hpRoot.LookAt(Camera.main.transform);

        //自分に状態権限がなければここで処理中断
        if (!HasStateAuthority) return;

        //WASDで移動
        move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z += 1;
        if (Input.GetKey(KeyCode.S)) move.z -= 1;
        if (Input.GetKey(KeyCode.D)) move.x += 1;
        if (Input.GetKey(KeyCode.A)) move.x -= 1;

        //移動
        transform.position += move * 0.1f;
    }

    /// <summary>
    /// HPが変更されると自動で呼ばれる
    /// </summary>
    public void UpdateHpUI()
    {
        hpText.text = HP.ToString();
    }

    private async UniTask DamageHP(int damage)
    {
        HP -= damage;

        //簡易ダメージ表現（色変え）
        var preColor = meshRenderer.material.color;
        meshRenderer.material.color = Color.red;

        await UniTask.Delay(1000);
        
        meshRenderer.material.color = preColor;
    }

    [Rpc(RpcSources.Proxies, RpcTargets.StateAuthority)]
    public void RPC_Damage()
    {
        DamageHP(10).Forget();
    }
}
