using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour
{
    Animator anim;
    int hexMask;

    public bool animating = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        hexMask = LayerMask.GetMask("HexMask");
    }

    void HoverAnimation()
    {
        anim.SetBool(("hover"), true);
        animating = true;
    }
    void Update()
    {
        if (animating && HexGrid.hoverHex != this.name)
        {
            anim.SetBool(("hover"), false);
            anim.Play(("unHover"));
        }
    }
}
