﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class bullet : UdonSharpBehaviour
{
    //public GameObject visionBlock;
    public GameObject handgun;


    private void OnParticleCollision(GameObject other)
    {

        if (other.name != "Plane")
        {
            Debug.Log(other.name);

        }

        if (other.name == "Visibility")
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            GameObject visionBlock = other.gameObject.transform.parent.Find("Constriction").gameObject;
            visionBlock.GetComponent<MeshRenderer>().enabled = !visionBlock.GetComponent<MeshRenderer>().enabled;
        }
        else if (other.name == "Hand")
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            handgun.GetComponent<gun>().shakeToggle();


        }


    }
}