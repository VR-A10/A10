
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class gun : UdonSharpBehaviour
{
    [SerializeField] private GameObject Manager;
    public ParticleSystem GunParticle;
    public int Shake = -1;
    private GameObject bullet;
    private GameObject[] bullets;

    void Start()
    {
        bullet = this.gameObject.transform.Find("Bullet").gameObject;
        bullets = new GameObject[4];
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
