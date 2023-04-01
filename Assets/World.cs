using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    private int playerLayer;
    private void Awake()
    {
        playerLayer = ( LayerMask.NameToLayer("Player"));

    }

    private void OnTriggerEnter(Collider collision)
    {
        print("comp: " + collision.gameObject.layer + " v "+ playerLayer);
        if (collision.gameObject.layer == playerLayer)
        {
            SceneManager.LoadScene(0);
        }
    }
}
