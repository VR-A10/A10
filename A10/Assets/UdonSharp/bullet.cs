﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class bullet : UdonSharpBehaviour
{
    public GameObject handgun;

    int damage_amount = 10;


    private void OnParticleCollision(GameObject other)
    {

        if (other.name != "Plane")
        {
            Debug.Log(other.name);

        }

        if (other.name == "Left Vision" || other.name == "Right Vision")
        {
            //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            GameObject visionBlock = other.gameObject.transform.parent.Find("Constriction").gameObject;
            visionBlock.GetComponent<MeshRenderer>().enabled = !visionBlock.GetComponent<MeshRenderer>().enabled;
        }
        else if (other.name == "Left Hand" || other.name == "Right Hand")
        {
            //other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            handgun.GetComponent<gun>().shakeToggle();
        }
        else if (other.name == "A10")
        {
            other.gameObject.GetComponent<a10>().A10hit(damage_amount);
        }


    }
}
