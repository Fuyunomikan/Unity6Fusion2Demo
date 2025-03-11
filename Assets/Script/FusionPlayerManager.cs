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

    //�l�b�g���[�N���L�����f�[�^
    [Networked, OnChangedRender(nameof(UpdateHpUI))] 
    public int HP { get; set; } = 100;

    private Vector3 move;

    public override void FixedUpdateNetwork()
    {
        //HP��UI���J�����Ɍ�����
        hpRoot.LookAt(Camera.main.transform);

        //�����ɏ�Ԍ������Ȃ���΂����ŏ������f
        if (!HasStateAuthority) return;

        //WASD�ňړ�
        move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z += 1;
        if (Input.GetKey(KeyCode.S)) move.z -= 1;
        if (Input.GetKey(KeyCode.D)) move.x += 1;
        if (Input.GetKey(KeyCode.A)) move.x -= 1;

        //�ړ�
        transform.position += move * 0.1f;
    }

    /// <summary>
    /// HP���ύX�����Ǝ����ŌĂ΂��
    /// </summary>
    public void UpdateHpUI()
    {
        hpText.text = HP.ToString();
    }

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
