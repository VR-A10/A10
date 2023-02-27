
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class a10 : UdonSharpBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject damagePlane;
    [SerializeField] GameObject Manager;
    private int self;
    public int hp;
    private GameObject gameover;
    private GameObject[] hps = new GameObject[4];
    private Text[] hpTexts = new Text[4];

    void Start()
    {
        string name = this.gameObject.transform.parent.gameObject.name;
        if (name.Length < 7) self = 0;
        else self = (int)Char.GetNumericValue(name[6]);
        hp = 100;
        gameover = canvas.transform.Find("Gameover").gameObject;
        hps[0] = canvas.transform.Find("HP1").gameObject;
        hps[1] = canvas.transform.Find("HP2").gameObject;
        hps[2] = canvas.transform.Find("HP3").gameObject;
        hps[3] = canvas.transform.Find("HP4").gameObject;
        gameover.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            hps[i].SetActive(false);
            hpTexts[i] = hps[i].GetComponent<Text>();
        }
    }

    public void A10hit(int damage)
    {
        hp = hp - damage;
        if (hp < 0) hp = 0;
        HPTextChanger(self, false, "", hp.ToString());
        Manager.GetComponent<gameManager>().A10hit(Networking.LocalPlayer.playerId, hp);
        Debug.Log("HIT! Remaining HP:"+ hp.ToString());
        if (hp <= 0)
        {
            gameover.GetComponent<Text>().text = "GAME OVER";
            gameover.SetActive(true);
            Manager.GetComponent<gameManager>().GameEnd(Networking.LocalPlayer.playerId);
        }
    }

    public void Restart(int[] assignments)
    {
        hp = 100;
        gameover.SetActive(false);
        if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject.transform.parent.gameObject))
        {
            for (int i = 0; i < 4; i++)
            {
                if (assignments[i] > 0) hps[i].SetActive(true);
                if (assignments[i] == Networking.LocalPlayer.playerId) HPTextChanger(i, true, "YOU:", hp.ToString());
                else HPTextChanger(i, true, "Player" + (i+1).ToString() + ":", hp.ToString());
            }
        }
    }

    public void HPTextChanger(int num, bool changeName, string name, string hp)
    {
        if (!changeName) name = hpTexts[num].text.Split(' ')[0];
        hpTexts[num].text = name + ' ' + hp;
    }

    public void GameoverTextChanger(string txt)
    {
        gameover.SetActive(true);
        gameover.GetComponent<Text>().text = txt;
    }
}
