using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AttachRacket : MonoBehaviour
{
    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        //print("Hover Begin");
        hand.ShowGrabHint();

    }


    private void OnHandHoverEnd(Hand hand)
    {
        //print("Hover End");
        hand.HideGrabHint();

    }

    private void HandHoverUpdate(Hand hand)
    {
        /*print("Start of OnHandHoverUpdate");*/
        GrabTypes grabtype = hand.GetGrabStarting(); //There are different types of grabs
        bool isGrabEnding = hand.IsGrabEnding(gameObject); //Boolean for if our hands/controllers are done grabbing any interactable (including the racket)

        /*print("Interactable: " + interactable);
        print("Interactable attached to hand: " + interactable.attachedToHand);
        print("Grabtype: " + grabtype.ToString());*/
        if (interactable.attachedToHand == null && grabtype != GrabTypes.None)
        {
            //print("If executed");
            hand.AttachObject(gameObject, grabtype);
            hand.HoverLock(interactable);
            hand.HideGrabHint();
            /*transform.LookAt(hand.transform.position);*/
        }
        else if(isGrabEnding)
        {
            //print("Else if executed");
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
        }

    }


    // Update is called once per frame
    //void Update()
    //{

    //}
}
