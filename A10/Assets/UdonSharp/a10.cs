
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

public class a10 : SimpleNetworkUdonBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] GameObject gameoverText;
=======
    [SerializeField] GameObject canvas;
>>>>>>> Stashed changes
    [SerializeField] GameObject damagePlane;
    [SerializeField] GameObject Manager;
    public int hp;
    private GameObject gameover;
    private GameObject[] hps = new GameObject[4];
    private Text[] hpTexts = new Text[4];

    void Start()
    {
        hp = 100;
        gameover = canvas.transform.Find("Gameover").gameObject;
        hps[0] = canvas.transform.Find("HP1").gameObject;
        hps[1] = canvas.transform.Find("HP2").gameObject;
        hps[2] = canvas.transform.Find("HP3").gameObject;
        hps[3] = canvas.transform.Find("HP4").gameObject;
        gameover.SetActive(false);
        for(int i = 0; i < 4; i++)
        {
            hps[i].SetActive(false);
            Debug.Log("initializing A10");
            hpTexts[i] = hps[i].GetComponent<Text>();
        }
        SimpleNetworkInit(Publisher.All);
    }

    public void A10hit(int damage)
    {
        hp = hp - damage;
        Debug.Log("HIT! Remaining HP:"+ hp.ToString());
        if (hp <= 0)
        {
<<<<<<< Updated upstream
            gameoverText.SetActive(true);
=======
            gameover.GetComponent<Text>().text = "GAME OVER";
            gameover.SetActive(true);
            Manager.GetComponent<gameManager>().GameEnd(Networking.LocalPlayer.playerId);
>>>>>>> Stashed changes
        }
    }

    public void Restart(int[] assignments)
    {
        Start();  // active(false)のgameobjectにアタッチされているUdonBehaviourはStart()が呼ばれないらしい
        hp = 100;
<<<<<<< Updated upstream
=======
        gameover.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            if (assignments[i] > 0) hps[i].SetActive(true);
            if (assignments[i] == Networking.LocalPlayer.playerId) hpTextChanger(i, true, "YOU:", hp.ToString());
            else hpTextChanger(i, true, "Player" + i.ToString() + ":", hp.ToString());
        }
>>>>>>> Stashed changes
    }

    private void hpTextChanger(int num, bool changeName, string name, string hp)
    {
        //string[] target = hpTexts[num].text.Split(' ');
        if (!changeName) name = hpTexts[num].text.Split(' ')[0];
        hpTexts[num].text = name + ' ' + hp;
    }
}
