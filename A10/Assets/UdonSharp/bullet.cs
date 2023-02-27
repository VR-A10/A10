
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class bullet : SimpleNetworkUdonBehaviour
{
    [SerializeField] private GameObject Manager;
    private GameObject[] targets = new GameObject[4];
    private GameObject handgun;
    int damage_amount = 20;

    private ParticleSystem GunParticle;
    private Transform particleTrans, initialParticleTrans;
    private Vector3 shotPosition, shotForward;
    private bool shotPositionReceived = false;
    private GameObject visionBlock = null;
    private bool visionBlockDisplayed = false;
    [SerializeField] AudioSource shotSound;

    void Start()
    {
        GunParticle = this.gameObject.GetComponent<ParticleSystem>();
        particleTrans = this.gameObject.transform;
        GameObject tmp = Instantiate(particleTrans.gameObject);
        tmp.SetActive(false);
        initialParticleTrans = tmp.transform;
        SimpleNetworkInit(Publisher.All);
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
                visionBlock = other.gameObject.transform.parent.Find("Constriction").gameObject;
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                //visionBlock.GetComponent<MeshRenderer>().enabled = true;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
                HitSoundPlay(other);
                int isLeft = (other.name == "Left Vision") ? 1 : -1;  // 左なら1, 右なら-1
                SendEvent("Vision", isLeft * Networking.GetOwner(other).playerId);
            }
        }
        else if (other.name == "Left Hand" || other.name == "Right Hand")
        {
            //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            if ((Networking.IsOwner(Networking.LocalPlayer, other.gameObject)) && (handgun != null))
            {
                handgun.GetComponent<gun>().shakeToggle();
                int isLeft = (other.name == "Left Hand") ? 1 : -1;
                SendEvent("Hand", isLeft * Networking.GetOwner(other).playerId);
            }
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
                HitSoundPlay(other);
            }
        }
        else if (other.name == "Right Leg" || other.name == "Left Leg")
        {
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
                HitSoundPlay(other);
                int isLeft = (other.name == "Left Leg") ? 1 : -1;
                SendEvent("Leg", isLeft * Networking.GetOwner(other).playerId);
                if (Networking.LocalPlayer.GetWalkSpeed() > 1.5f)
                {
                    Networking.LocalPlayer.SetWalkSpeed(1.0f);
                    Networking.LocalPlayer.SetRunSpeed(2.0f);
                    Networking.LocalPlayer.SetStrafeSpeed(1.0f);
                    Networking.LocalPlayer.SetJumpImpulse(1.0f);
                } else
                {
                    Networking.LocalPlayer.SetWalkSpeed(0.1f);
                    Networking.LocalPlayer.SetRunSpeed(0.1f);
                    Networking.LocalPlayer.SetStrafeSpeed(0.1f);
                    Networking.LocalPlayer.SetJumpImpulse(0.0f);
                }
            }
        }
        else if (other.name == "A10")
        {
            if (Networking.IsOwner(Networking.LocalPlayer, other.gameObject))
            {
                GameObject damagePlane = other.gameObject.transform.parent.Find("damage").gameObject;
                damagePlane.GetComponent<Animator>().SetTrigger("damaged");
                other.gameObject.GetComponent<a10>().A10hit(damage_amount);
                HitSoundPlay(other);
            }
        }


    }

    public void Shot()
    {
        InitParticleTrans();
        SendEvent("ShotPosition", particleTrans.position);
        SendEvent("Shot", particleTrans.forward);
    }

    public override void ReceiveEvent(string name, string value)
    {
        if (targets[0] == null)
        {
            targets = Manager.GetComponent<gameManager>().GetTargets();
        }

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

        if (name == "Vision")
        {
            int val = GetInt(value);
            bool isLeft = (val > 0);
            Transform target = null;
            if (isLeft)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Left Vision");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                val *= -1;
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Right Vision");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
            if (!(target == null || target.parent.Find("Left Vision").gameObject.activeSelf || target.parent.Find("Right Vision").gameObject.activeSelf))
            {
                if (Networking.LocalPlayer.playerId == val)
                {
                    visionBlockDisplayed = true;
                }
            }
        }

        if (name == "Hand")
        {
            int val = GetInt(value);
            bool isLeft = (val > 0);
            Transform target = null;
            if (isLeft)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Left Hand");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                val *= -1;
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Right Hand");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (name == "Leg")
        {
            int val = GetInt(value);
            bool isLeft = (val > 0);
            Transform target = null;
            if (isLeft)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Left Leg");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                val *= -1;
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.GetOwner(targets[i]).playerId == val && targets[i] != null)
                    {
                        target = targets[i].transform.Find("Right Leg");
                        if (target != null) target.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (visionBlockDisplayed && visionBlock != null)
        {
            visionBlock.GetComponent<MeshRenderer>().enabled = true;
            visionBlockDisplayed = false;
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

    private void HitSoundPlay(GameObject obj)
    {
        AudioSource hitSound = obj.transform.parent.Find("Hit Sound").gameObject.GetComponent<AudioSource>();
        hitSound.Play();
    }

    private void Constriction(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().enabled = true;
    }
}
