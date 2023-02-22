using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class UpgradeStationNode : MonoBehaviour
{
    [SerializeField] private GameObject [] camOrder; // The camera we will go to when we click here
    [SerializeField] private float transitionTime;
    private int curCam;
   

    //Needs to be public so UI objects are able to access this.
    public void SwapCams(int dir)
    {
        StopAllCoroutines(); //If already moving, stop.
        StartCoroutine(HandleCamMovement( dir));
    }

    private IEnumerator HandleCamMovement(int dir)
    {
            int i = camOrder.Length;
            while (--i > 0)
            {
                camOrder[curCam].SetActive(false);
                curCam += dir;
                camOrder[curCam].SetActive(true);
                yield return new WaitForSeconds(transitionTime);
            }
            
            if (dir == -1) UpgradeStationController.IsInMenu = false;
    }
}
