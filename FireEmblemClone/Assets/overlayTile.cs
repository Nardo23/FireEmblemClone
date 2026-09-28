using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlayTile : MonoBehaviour
{
    private void Start()
    {
        showTile();
    }
    public void showTile()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, .6f);
    }

    public void hideTile()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0f);
    }
    public float value;
}
