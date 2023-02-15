
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class button : UdonSharpBehaviour
{
    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
    [SerializeField] GameObject spawn4;
    private GameObject[] spawns = new GameObject[4];

    private void Start()
    {
        spawns[0] = spawn1;
        spawns[1] = spawn2;
        spawns[2] = spawn3;
        spawns[3] = spawn4;
    }

    public override void Interact()
    {
        var player = Networking.LocalPlayer;
        player.TeleportTo(spawns[player.playerId-1].transform.position, spawns[player.playerId - 1].transform.rotation);
        
    }
}
