
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Out : UdonSharpBehaviour
{
    [SerializeField] GameObject Manager;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        Manager.GetComponent<gameManager>().Out(player.playerId);
    }
}
