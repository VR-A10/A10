
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class bullet : SimpleNetworkUdonBehaviour
{
    private GameObject handgun;
    int damage_amount = 10;

    private ParticleSystem GunParticle;
    private Transform particleTrans, initialParticleTrans;
    private Vector3 shotPosition, shotForward;
    private bool shotPositionReceived = false;
    [SerializeField] AudioSource shotSound;

    [SerializeField] GameObject damage0;

    void Start()
    {
        GunParticle = this.gameObject.GetComponent<ParticleSystem>();
        particleTrans = this.gameObject.transform;
        GameObject tmp = Instantiate(particleTrans.gameObject);
        tmp.SetActive(false);
        initialParticleTrans = tmp.transform;
        SimpleNetworkInit(Publisher.Owner);
    }

    public void SetHandGun(GameObject gun)
    {
        handgun = gun;
    }

    public void ResetHandGun()
    {
        handgun = null;
    }

    private void OnParticleCollision(GameObject other)
    {

        if (other.name != "Plane")
        {
            Debug.Log(other.name);

        }

        if (other.name == "Left Vision" || other.name == "Right Vision")
        {
            //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject visionBlock = other.gameObject.transform.parent.Find("Constriction").gameObject;
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                visionBlock.GetComponent<MeshRenderer>().enabled = !visionBlock.GetComponent<MeshRenderer>().enabled;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
            }
        }
        else if (other.name == "Left Hand" || other.name == "Right Hand")
        {
            //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            if ((Networking.IsOwner(Networking.LocalPlayer, other.gameObject)) && (handgun != null))
            {
                handgun.GetComponent<gun>().shakeToggle();
            }
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
            }
        }
        else if (other.name == "A10")
        {
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
                other.gameObject.GetComponent<a10>().A10hit(damage_amount);
            }
        }


    }

    public void Shot()
    {
        InitParticleTrans();
        SendEvent("ShotPosition", particleTrans.position, true);
        SendEvent("Shot", particleTrans.forward, true);
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
                Debug.Log("hihihi");
                particleTrans.position = shotPosition;
                particleTrans.forward = GetVector3(value);
                GunParticle.Play();
                shotSound.Play();
                shotPositionReceived = false;
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
            GunParticle.Play();
            shotSound.Play();
            shotPositionReceived = false;
        }
        //else SendCustomEventDelayedSeconds(nameof(DelayedShot), 0.05f);
    }

    private void InitParticleTrans()
    {
        particleTrans.localPosition = initialParticleTrans.localPosition;
        particleTrans.localRotation = initialParticleTrans.localRotation;
    }
}
