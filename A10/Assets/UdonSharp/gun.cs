
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class gun : SimpleNetworkUdonBehaviour
{
    public ParticleSystem GunParticle;
    public int Shake = -1;
    private Transform particleTrans, initialParticleTrans;
    private Vector3 shotPosition, shotForward;
    private bool shotPositionReceived = false;

    void Start()
    {
        particleTrans = this.gameObject.transform.Find("Bullet");
        GameObject tmp = Instantiate(particleTrans.gameObject);
        tmp.SetActive(false);
        initialParticleTrans = tmp.transform;
        SimpleNetworkInit(Publisher.Owner);
    }

    public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();
        SendEvent("ShotPosition", particleTrans.position);
        SendEvent("Shot", particleTrans.forward);
    }

    public override void ReceiveEvent(string name, string value)
    {
        if (name == "ShotPosition")
        {
            // 受け取った座標を記憶
            shotPositionReceived = true;
            shotPosition = GetVector3(value);
        }

        if (name == "Shot")
        {
            if (shotPositionReceived)
            {
                particleTrans.position = shotPosition;
                particleTrans.forward = GetVector3(value);
                Shot();
                shotPositionReceived = false;
                InitParticleTrans();
            }
            else
            {
                shotForward = GetVector3(value);
                SendCustomEventDelayedSeconds(nameof(DelayedShot), 0.05f);
            }
        }
    }

    public void DelayedShot()
    {
        if (shotPositionReceived)
        {
            particleTrans.position = shotPosition;
            particleTrans.forward = shotForward;
            Shot();
            shotPositionReceived = false;
            InitParticleTrans();
        }
        else SendCustomEventDelayedSeconds(nameof(DelayedShot), 0.05f);
    }

    private void InitParticleTrans()
    {
        particleTrans.localPosition = initialParticleTrans.localPosition;
        particleTrans.localRotation = initialParticleTrans.localRotation;
    }

    public void Shot()
    {
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
