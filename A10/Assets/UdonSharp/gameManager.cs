
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class gameManager : UdonSharpBehaviour
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
    [UdonSynced] private int[] targetAssignment;
    private GameObject[] targets = new GameObject[4];
    private GameObject[] guns = new GameObject[4];
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
}
