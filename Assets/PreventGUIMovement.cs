using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreventGUIMovement : MonoBehaviour
{

    private CinemachineFreeLook cvm;
    // Start is called before the first frame update
    void Start()
    {
        cvm = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Utilities.IsPointerOverUIObject())
        {
            cvm.m_XAxis.m_MaxSpeed = 0;
            cvm.m_YAxis.m_MaxSpeed = 0;
            return;
        }
        cvm.m_XAxis.m_MaxSpeed = 450;
        cvm.m_YAxis.m_MaxSpeed = 2;
    }
}
