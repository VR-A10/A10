
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
    private Transform gunTrans;
    private Transform initialGunTrans;
    private GameObject laser;

    void Start()
    {
        gunTrans = this.gameObject.transform.Find("Visual");
        bullet = gunTrans.Find("Bullet").gameObject;
        laser = bullet.transform.Find("RaycastLaser *").gameObject;
        laser.SetActive(false);
        bullets = new GameObject[4];
        GameObject tmp = Instantiate(gunTrans.gameObject);
        tmp.SetActive(false);
        initialGunTrans = tmp.transform;
        Shake = 0;
    }

    public override void OnPickup()
    {
        if (bullets[0] == null)
        {
            GameObject[] guns = Manager.GetComponent<gameManager>().GetGuns();
            for (int i = 0; i < 4; i++)
            {
                bullets[i] = guns[i].transform.Find("Visual/Bullet").gameObject;
            }
        }
        foreach (GameObject bul in bullets)
        {
            bul.GetComponent<bullet>().SetHandGun(this.gameObject);
        }
        laser.SetActive(true);
    }

    public override void OnDrop()
    {
        foreach (GameObject bul in bullets)
        {
            if (bul != null)
                bul.GetComponent<bullet>().ResetHandGun();
        }
        laser.SetActive(false);
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
            gunTrans.Rotate(0.0f, 2 * Mathf.Cos(40 * Time.time), 1.5f * Mathf.Cos(30 * Time.time));
        }
        else InitGunTrans();

    }

    public void Restart()
    {
        Shake = 0;
    }

    private void InitGunTrans()
    {
        gunTrans.localPosition = initialGunTrans.localPosition;
        gunTrans.localRotation = initialGunTrans.localRotation;
    }
}
