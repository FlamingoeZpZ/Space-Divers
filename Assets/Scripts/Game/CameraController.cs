using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineBrain brain;
    void Start()
    {
        brain = GetComponent<CinemachineBrain>();
        brain.m_WorldUpOverride = ModularPlayerScript.Instance.transform;
    }
}
