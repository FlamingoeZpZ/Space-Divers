using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [field: SerializeField] public PartType myType;
    [SerializeField] private ComponentPlacementPoint[] components;
    public PartType GetPartType => myType;

    private float y;
    private void Update()
    {
        y += Time.deltaTime * 20;
        transform.eulerAngles = new Vector3(0,y,0);
    }

}
[Flags, Serializable]
public enum PartType
{
    Special=0,
    Weapon=1,
    HeavyWeapon=2,
    Wing=4,
    Thruster = 8
}