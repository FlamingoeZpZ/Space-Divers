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
    [SerializeField] private GameObject myGui;
    private static int _curCam;
   

    //Needs to be public so UI objects are able to access this.
    public void SwapCams(int dir)
    {
        StopAllCoroutines(); //If already moving, stop.
        StartCoroutine(HandleCamMovement( dir));
    }

    private IEnumerator HandleCamMovement(int dir)
    {
        do
        {
            camOrder[_curCam].SetActive(false);
            _curCam += dir;

            camOrder[_curCam].SetActive(true);
            yield return new WaitForSeconds(transitionTime);
        } while (_curCam > 0 && _curCam < camOrder.Length - 1);

        bool b = dir == 1;
        myGui.SetActive(b);
        if(!b)
            UpgradeStationController.IsInMenu = false;
    }
}
