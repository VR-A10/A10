
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class hitPoint : UdonSharpBehaviour
{
    [SerializeField] private float maxHP;
    public float HP;

    void Start()
    {
        HP = maxHP;
    }

    public void Hit(float damage)
    {
        HP -= damage;
        HitEffect();

        if (HP > maxHP) HP = maxHP;  // もし回復要素を加えるならこれが必要
        
        if (HP < 0)
        {
            HP = 0;
            Destroy();
        }
    }

    private void HitEffect()  // 命中エフェクト
    {
        /*TODO*/
    }

    private void Destroy()  // 破壊エフェクト
    {
        /*TODO*/
    }
}
