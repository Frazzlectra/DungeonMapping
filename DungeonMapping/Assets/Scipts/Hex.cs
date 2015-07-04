using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour
{
    Animator anim;
    int hexMask;

    public bool animating = false; 

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
}
