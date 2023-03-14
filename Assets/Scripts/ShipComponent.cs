using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [SerializeField] private PartType myType;
    [SerializeField] private ComponentPlacementPoint[] components;

    private float y;
    private void Update()
    {
        y += Time.deltaTime * 20;
        transform.eulerAngles = new Vector3(0,y,0);
    }


    private enum PartType
    {
        Special=0,
        Weapon=1,
        HeavyWeapon=2,
        Wing=4,
        Thruster = 8
    }
}
