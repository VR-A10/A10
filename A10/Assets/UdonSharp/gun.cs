
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class gun : UdonSharpBehaviour
{
    [SerializeField] private GameObject Manager;
    public ParticleSystem GunParticle;
    private float Shake;
    private GameObject bullet;
    private GameObject[] bullets;

    void Start()
    {
        bullet = this.gameObject.transform.Find("Bullet").gameObject;
        bullets = new GameObject[4];
        Shake = 0;
    }

    public override void OnPickup()
    {
        if (bullets[0] == null)
        {
            GameObject[] guns = Manager.GetComponent<gameManager>().GetGuns();
            for (int i = 0; i < 4; i++)
            {
                bullets[i] = guns[i].transform.Find("Bullet").gameObject;
            }
        }
        foreach (GameObject bul in bullets)
        {
            bul.GetComponent<bullet>().SetHandGun(this.gameObject);
        }
    }

    public override void OnDrop()
    {
        foreach (GameObject bul in bullets)
        {
            if (bul != null)
                bul.GetComponent<bullet>().ResetHandGun();
        }
    }

    public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();
        bullet.GetComponent<bullet>().Shot();
    }


    public void shakeToggle()
    {
        Shake += 10;
    }

    private void Update()
    {
        if (Shake > 0)
        {
            Shake -= Time.deltaTime;
            Transform gunTrans = this.transform;
            gunTrans.Rotate(0.0f, 2 * Mathf.Cos(40 * Time.time), 1.5f * Mathf.Cos(30 * Time.time));
        }
    }

    public void Restart()
    {
        Shake = 0;
    }
}
