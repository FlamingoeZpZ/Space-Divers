using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rotator : MonoBehaviour
{
    [SerializeField] private float speed;
    private void LateUpdate()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
