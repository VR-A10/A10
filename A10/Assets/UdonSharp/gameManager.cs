
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using tutinoco;

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
    [SerializeField] GameObject spawn0;
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
    [UdonSynced] private int[] targetAssignment;
    [UdonSynced] private int winner;
    private GameObject[] targets = new GameObject[4], a10s = new GameObject[4], rightHands = new GameObject[4], leftHands = new GameObject[4], rightEyes = new GameObject[4], leftEyes = new GameObject[4], rightLegs = new GameObject[4], leftLegs = new GameObject[4];
    private GameObject[] guns = new GameObject[4];
    private GameObject[] spawns = new GameObject[4];
    private MeshRenderer[] visionBlocks = new MeshRenderer[4];
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
        spawns[0] = spawn0;
        spawns[1] = spawn1;
        spawns[2] = spawn2;
        spawns[3] = spawn3;

        for (int i = 0; i < 4; i++)
        {
            a10s[i] = targets[i].transform.Find("A10").gameObject;
            rightHands[i] = targets[i].transform.Find("Right Hand").gameObject;
            leftHands[i] = targets[i].transform.Find("Left Hand").gameObject;
            rightEyes[i] = targets[i].transform.Find("Right Vision").gameObject;
            leftEyes[i] = targets[i].transform.Find("Left Vision").gameObject;
            visionBlocks[i] = targets[i].transform.Find("Constriction").gameObject.GetComponent<MeshRenderer>();
            rightLegs[i] = targets[i].transform.Find("Right Leg").gameObject;
            leftLegs[i] = targets[i].transform.Find("Left Leg").gameObject;
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
                if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) targetAssignment[i] = -1;
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
            a10s[i].SetActive(true);
            a10s[i].GetComponent<a10>().Restart();
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
            player.TeleportTo(spawns[(player.playerId - 1) % 4].transform.position, spawns[(player.playerId - 1) % 4].transform.rotation);

        }
    }
}
