
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class gameManager : SimpleNetworkUdonBehaviour
{
    [SerializeField] GameObject target0;
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;
    [SerializeField] GameObject headAnchor;
    [SerializeField] GameObject gun0;
    [SerializeField] GameObject gun1;
    [SerializeField] GameObject gun2;
    [SerializeField] GameObject gun3;
<<<<<<< Updated upstream
    [SerializeField] GameObject battleField;
    [SerializeField] GameObject waitingField;
    //[SerializeField] GameObject spawn0;
    //[SerializeField] GameObject spawn1;
    //[SerializeField] GameObject spawn2;
    //[SerializeField] GameObject spawn3;
=======
<<<<<<< Updated upstream
    [SerializeField] GameObject spawn0;
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
=======
    [SerializeField] GameObject gun4;
    [SerializeField] GameObject gun5;
    [SerializeField] GameObject gun6;
    [SerializeField] GameObject gun7;
    [SerializeField] GameObject battleField;
    [SerializeField] GameObject waitingField;
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    [UdonSynced] private int[] targetAssignment;
    [UdonSynced] private int loser;
    [UdonSynced] private bool isGame = false;
    private GameObject[] targets = new GameObject[4], a10s = new GameObject[4], rightHands = new GameObject[4], leftHands = new GameObject[4], rightEyes = new GameObject[4], leftEyes = new GameObject[4], rightLegs = new GameObject[4], leftLegs = new GameObject[4];
<<<<<<< Updated upstream
    private GameObject[] guns = new GameObject[4];
<<<<<<< Updated upstream
    private GameObject[] battleSpawns = new GameObject[4], waitingSpawns = new GameObject[4];
=======
    private GameObject[] spawns = new GameObject[4];
=======
    private GameObject[] guns = new GameObject[8];
    private GameObject[] battleSpawns = new GameObject[4], waitingSpawns = new GameObject[4];
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    private MeshRenderer[] visionBlocks = new MeshRenderer[4];
    private Vector3[] gunSpawns = new Vector3[8] { new Vector3(1.5f, 0.8f, 1.8f), new Vector3(1.5f, 0.8f, 1.4f), new Vector3(1.5f, 0.8f, 1.0f), new Vector3(1.5f, 0.8f, 0.6f), new Vector3(-1.5f, 0.8f, 1.8f), new Vector3(-1.5f, 0.8f, 1.4f), new Vector3(-1.5f, 0.8f, 1.0f), new Vector3(-1.5f, 0.8f, 0.6f) };
    bool playerJoined = false;

    void Start()
    {
        targetAssignment = new int[4] { -1, -1, -1, -1 };
        targets[0] = target0;
        targets[1] = target1;
        targets[2] = target2;
        targets[3] = target3;
        guns[0] = gun0;
        guns[1] = gun1;
        guns[2] = gun2;
        guns[3] = gun3;
<<<<<<< Updated upstream
        //spawns[0] = spawn0;
        //spawns[1] = spawn1;
        //spawns[2] = spawn2;
        //spawns[3] = spawn3;
=======
<<<<<<< Updated upstream
        spawns[0] = spawn0;
        spawns[1] = spawn1;
        spawns[2] = spawn2;
        spawns[3] = spawn3;
=======
        guns[4] = gun4;
        guns[5] = gun5;
        guns[6] = gun6;
        guns[7] = gun7;
>>>>>>> Stashed changes
>>>>>>> Stashed changes

        for (int i = 0; i < 4; i++)
        {
            targets[i].SetActive(false);
            a10s[i] = targets[i].transform.Find("A10/Script").gameObject;
            rightHands[i] = targets[i].transform.Find("Right Hand").gameObject;
            leftHands[i] = targets[i].transform.Find("Left Hand").gameObject;
            rightEyes[i] = targets[i].transform.Find("Right Vision").gameObject;
            leftEyes[i] = targets[i].transform.Find("Left Vision").gameObject;
            visionBlocks[i] = targets[i].transform.Find("Constriction").gameObject.GetComponent<MeshRenderer>();
            rightLegs[i] = targets[i].transform.Find("Right Leg").gameObject;
            leftLegs[i] = targets[i].transform.Find("Left Leg").gameObject;
            battleSpawns[i] = battleField.transform.Find("Spawn Points/Spawn" + i.ToString()).gameObject;
            waitingSpawns[i] = waitingField.transform.Find("Spawn Points/Spawn" + i.ToString()).gameObject;
        }
        SimpleNetworkInit(Publisher.All);
    }

    void Update()
    {
        if (playerJoined)  // プレイヤーが参加した際の処理(同期の関係上、owner権限の移譲から1フレーム後に処理する必要がある)
        {
            playerJoined = false;
            for (int i = 0; i < 4; i++)
            {
                if (targetAssignment[i] != -1) targets[i].SetActive(true);  // プレイヤーに割り当てられている的を有効化
                else targets[i].SetActive(false);
                if (targetAssignment[i] == Networking.LocalPlayer.playerId)
                {
                    targets[i].transform.parent = headAnchor.transform;  // 頭に付ける
                    targets[i].transform.localPosition = new Vector3(0, 0, 0);
                    targets[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        playerJoined = true;
        // Joinしたプレイヤーの同期
        if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
        {
            // 誰も使用していない的を割り当てる
            int targetNum = -1;

            for (int i = 0; i < 4; i++)
            {
                if (targetAssignment[i] == -1)
                {
                    targetAssignment[i] = player.playerId;
                    targetNum = i;
                    break;
                }
            }

            if (targetNum != -1)
            {
                GameObject newTarget = targets[targetNum];  // 的を取得する
                if (!Networking.IsOwner(player, newTarget)) Networking.SetOwner(player, newTarget);  // 同期させるための権限を取得
            }
            RequestSerialization();
        }
    }


    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        for (int i = 0; i < 4; i++)
        {
            if (targetAssignment[i] == player.playerId)
            {
<<<<<<< Updated upstream
                if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
                {
                    targetAssignment[i] = -1;
                    RequestSerialization();
                }
=======
<<<<<<< Updated upstream
                if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) targetAssignment[i] = -1;
=======
                targetAssignment[i] = -1;
                RequestSerialization();
>>>>>>> Stashed changes
>>>>>>> Stashed changes
                targets[i].SetActive(false);
            }
        }
    }

    public GameObject[] GetGuns()
    {
        return guns;
    }

    public GameObject[] GetTargets()
    {
        return targets;
    }

    public void StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
<<<<<<< Updated upstream
            a10s[i].SetActive(true);
<<<<<<< Updated upstream
            a10s[i].GetComponent<a10>().Restart();
=======
=======
            a10s[i].transform.parent.gameObject.SetActive(true);
>>>>>>> Stashed changes
            if (startGame) a10s[i].GetComponent<a10>().Restart(targetAssignment);
>>>>>>> Stashed changes
            rightHands[i].SetActive(true);
            leftHands[i].SetActive(true);
            rightEyes[i].SetActive(true);
            leftEyes[i].SetActive(true);
            visionBlocks[i].enabled = false;
            rightLegs[i].SetActive(true);
            leftLegs[i].SetActive(true);
            Networking.LocalPlayer.SetWalkSpeed(2.0f);
            Networking.LocalPlayer.SetRunSpeed(4.0f);
            Networking.LocalPlayer.SetStrafeSpeed(2.0f);
            Networking.LocalPlayer.SetJumpImpulse(3.0f);
            
        }
    }

    public override void Interact()
    {
        SendEvent("GameStart", true, true);
    }

    public override void ReceiveEvent(string name, string value)
    {
        if (name == "GameStart")
        {
            StartGame(); 
            var player = Networking.LocalPlayer;
            player.TeleportTo(battleSpawns[(player.playerId - 1) % 4].transform.position, battleSpawns[(player.playerId - 1) % 4].transform.rotation);
            if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
            {
                isGame = true;
                RequestSerialization();
            }
        }

        if (name == "GameEnd")
        {
            var player = Networking.LocalPlayer;
            player.TeleportTo(waitingSpawns[(player.playerId - 1) % 4].transform.position, waitingSpawns[(player.playerId - 1) % 4].transform.rotation);
            for (int i = 0; i < 8; i++)
            {
                guns[i].transform.position = gunSpawns[i];
            }
            if (isGame)
            {
                isGame = false;
                RequestSerialization();
                for (int i = 0; i < 4; i++)
                {
                    if (Networking.IsOwner(Networking.LocalPlayer, targets[i]))
                    {
                        a10s[i].GetComponent<a10>().HPTextChanger(i, false, "", "0");
                        if (Networking.LocalPlayer.playerId == loser) a10s[i].GetComponent<a10>().GameoverTextChanger("GAME OVER");
                        else a10s[i].GetComponent<a10>().GameoverTextChanger("YOU WIN");
                    }
                }
            }
        }
    }

    public void GameEnd(int _loser)
    {
        if (isGame) loser = _loser;
        SendEvent("GameEnd", true);
        RequestSerialization();
    }
}
