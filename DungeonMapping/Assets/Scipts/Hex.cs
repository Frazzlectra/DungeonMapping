using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Hex : MonoBehaviour, IComparable<Hex>
{
    Animator anim;
    int hexMask;

    public bool animating = false;

    public int hexX;
    public int hexY;
    public string typeSpawn;


    void Start()
    {
        //get objects from scene
        anim = GetComponent<Animator>();
        hexMask = LayerMask.GetMask("HexMask");        
    }

    void HoverAnimation()
    {
        anim.SetBool(("hover"), true);//turn green becasue the mouse is hovering over it
        animating = true; //so I can turn it off for this hex inparticular
    }
    void Update()
    {
        if (animating && HexGrid.hoverHex != this.name) //hoverHex is a string that changes to the hex the mouse is hovering over
        {
            //turn off animations
            anim.SetBool(("hover"), false);
            anim.Play(("unHover"));
        }
    }
    public int CompareTo(Hex other)
    {
        //If the other instance is null, asume taht it is at 0, 0 you need to make that determination.
        if (other == null)
            return 1;

        int comparison = hexX.CompareTo(other.hexX);
        if (comparison < 0 || comparison > 0)
            return comparison;

        return hexY.CompareTo(other.hexY);
    }

}