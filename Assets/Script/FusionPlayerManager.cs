using UnityEngine;
using Fusion;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class FusionPlayerManager : NetworkBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    //�l�b�g���[�N���L�����f�[�^
    [Networked] public int HP { get; set; } = 100;

    private async UniTask DamageHP(int damage)
    {
        HP -= damage;

        //�ȈՃ_���[�W�\���i�F�ς��j
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
