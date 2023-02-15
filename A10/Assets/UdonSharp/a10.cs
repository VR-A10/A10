
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class a10 : UdonSharpBehaviour
{
    [SerializeField] GameObject gameoverText;
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
            gameoverText.SetActive(true);
        }
    }



}
