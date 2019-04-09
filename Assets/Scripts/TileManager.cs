using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public int key = 0;

    void FixedUpdate()
    {
        /*
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0) // gone off the screen to the left position
        {
            // Destroy(this.gameObject);
        }
        */
    }

    private void OnDisable()
    {
        Debug.Log("Destroyed " + this.name + " as #" + this.key);
    }

}

