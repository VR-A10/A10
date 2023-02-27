
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class a10 : UdonSharpBehaviour
{
    [SerializeField] Text gameoverText;
    [SerializeField] GameObject damagePlane;
    public int hp;

    void Start()
    {
        hp = 100;
    }

    public void A10hit(int damage)
    {
        hp = hp - damage;
        Debug.Log("HIT! Remaining HP:"+ hp.ToString());
        if (hp <= 0)
        {
            //gameoverText.SetActive(true);
            gameoverText.enabled = true;
        }
    }

    public void Restart()
    {
        hp = 100;
        gameoverText.enabled = false;
    }

}
