
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
<<<<<<< Updated upstream
=======
<<<<<<< Updated upstream
using tutinoco;
>>>>>>> Stashed changes

public class a10 : UdonSharpBehaviour
{
    [SerializeField] Text gameoverText;
    [SerializeField] GameObject damagePlane;
    public int hp;
<<<<<<< Updated upstream
=======
=======
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class a10 : UdonSharpBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject damagePlane;
    [SerializeField] GameObject Manager;
    public int hp;
    [UdonSynced] private int[] hparray = new int[4];
    private int self;
>>>>>>> Stashed changes
    private GameObject gameover;
    private GameObject[] hps = new GameObject[4];
    private Text[] hpTexts = new Text[4];
>>>>>>> Stashed changes

    void Start()
    {
        string name = this.gameObject.transform.parent.parent.gameObject.name;
        if (name.Length < 7) self = 0;
        else self = (int)Char.GetNumericValue(name[6]);
        hp = 100;
<<<<<<< Updated upstream
=======
        gameover = canvas.transform.Find("Gameover").gameObject;
        hps[0] = canvas.transform.Find("HP1").gameObject;
        hps[1] = canvas.transform.Find("HP2").gameObject;
        hps[2] = canvas.transform.Find("HP3").gameObject;
        hps[3] = canvas.transform.Find("HP4").gameObject;
        gameover.SetActive(false);
        for(int i = 0; i < 4; i++)
        {
            hps[i].SetActive(false);
<<<<<<< Updated upstream
            Debug.Log("initializing A10");
            hpTexts[i] = hps[i].GetComponent<Text>();
        }
        SimpleNetworkInit(Publisher.All);
=======
            hpTexts[i] = hps[i].GetComponent<Text>();
        }
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    }

    public void A10hit(int damage)
    {
        hp = hp - damage;
        if (hp < 0) hp = 0;
        hparray[self] = hp;
        RequestSerialization();
        HPTextChanger(self, false, "", hp.ToString());
        Debug.Log("HIT! Remaining HP:"+ hp.ToString());
        if (hp <= 0)
        {
<<<<<<< Updated upstream
            //gameoverText.SetActive(true);
            gameoverText.enabled = true;
=======
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            gameoverText.SetActive(true);
=======
            gameover.GetComponent<Text>().text = "GAME OVER";
            gameover.SetActive(true);
            Manager.GetComponent<gameManager>().GameEnd(Networking.LocalPlayer.playerId);
>>>>>>> Stashed changes
=======
            Manager.GetComponent<gameManager>().GameEnd(Networking.LocalPlayer.playerId);
>>>>>>> Stashed changes
>>>>>>> Stashed changes
        }
    }

    public void Restart()
    {
        hp = 100;
<<<<<<< Updated upstream
        gameoverText.enabled = false;
    }

=======
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
        gameover.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            if (assignments[i] > 0) hps[i].SetActive(true);
<<<<<<< Updated upstream
            if (assignments[i] == Networking.LocalPlayer.playerId) hpTextChanger(i, true, "YOU:", hp.ToString());
            else hpTextChanger(i, true, "Player" + i.ToString() + ":", hp.ToString());
        }
>>>>>>> Stashed changes
    }

    private void hpTextChanger(int num, bool changeName, string name, string hp)
=======
            if (assignments[i] == Networking.LocalPlayer.playerId) HPTextChanger(i, true, "YOU:", hp.ToString());
            else HPTextChanger(i, true, "Player" + i.ToString() + ":", hp.ToString());
        }
    }

    public override void OnDeserialization()
    {
        for (int i = 0; i < 4; i++)
        {
            HPTextChanger(i, false, "", hparray[i].ToString());
        }
    }

    public void HPTextChanger(int num, bool changeName, string name, string hp)
>>>>>>> Stashed changes
    {
        //string[] target = hpTexts[num].text.Split(' ');
        if (!changeName) name = hpTexts[num].text.Split(' ')[0];
        hpTexts[num].text = name + ' ' + hp;
    }
<<<<<<< Updated upstream
=======

    public void GameoverTextChanger(string txt)
    {
        gameover.SetActive(true);
        gameover.GetComponent<Text>().text = txt;
    }
>>>>>>> Stashed changes
>>>>>>> Stashed changes
}
