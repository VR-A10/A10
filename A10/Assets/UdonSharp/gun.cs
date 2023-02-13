
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class gun : UdonSharpBehaviour
{
    public ParticleSystem GunParticle;
    public int Shake = -1;

    public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();

        GunParticle.Play();


    }

    public void shakeToggle()
    {
        Shake = Shake * (-1);
    }

    private void Update()
    {
        if (Shake == 1)
        {
            Transform gunTrans = this.transform;
            gunTrans.Rotate(0.0f, 2 * Mathf.Cos(40 * Time.time), 1.5f * Mathf.Cos(30 * Time.time));

        }
    }
}
