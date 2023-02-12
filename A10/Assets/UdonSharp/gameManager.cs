
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
    [SerializeField] GameObject head;
    [UdonSynced] private int[] targetAssignment;
    private GameObject[] targets = new GameObject[4];
    bool playerJoined = false;

    void Start()
    {
        targetAssignment = new int[4] { -1, -1, -1, -1 };
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
                    targets[i].transform.parent = head.transform;  // 頭に付ける
                    targets[i].transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        playerJoined = true;
        if (targets[0] == null)
        {
            targets[0] = target0;
            targets[1] = target1;
            targets[2] = target2;
            targets[3] = target3;
        }
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
        }
    }


    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
        {
            for (int i = 0; i < 4; i++)
            {
                if (targetAssignment[i] == player.playerId)
                {
                    targetAssignment[i] = -1;
                    targets[i].SetActive(false);
                }
            }
        }            
    }
}
