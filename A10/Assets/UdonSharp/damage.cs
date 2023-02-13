
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class damage : UdonSharpBehaviour
{
    public GameObject damageplane;
    private Animator anim;

    void Start()
    {
        damageplane.SetActive(false);
        anim = damageplane.GetComponent<Animator>();
    }

    void playDamage()
    {
        damageplane.SetActive(true);
        //anim.Play();
    }
}
