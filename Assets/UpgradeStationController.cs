using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeStationController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera[] CineMachineCams;
    [SerializeField] private CinemachineVirtualCamera cart;
    [SerializeField] private Transform myShippyDoo;

    //Lerp between points
    [Header("Dolly ship")]
    [SerializeField] private float dollyTime;
    [SerializeField] private Transform shipStartPoint;
    [SerializeField] private Transform shipEndPoint;
    private Transform shipTrans;   
    private Camera cam;
    public static bool IsInMenu;
    private UpgradeStationNode inController;

    [SerializeField] private GameObject mainGUI;
    

    //On Awake, we should start the cart.
    private void Awake()
    {
        //Follow the ship
        shipTrans = Instantiate(myShippyDoo, shipStartPoint.position, Quaternion.identity);
        shipTrans.LookAt(shipEndPoint.position, Vector3.up);
        cart.Follow = shipTrans;
        
        cam = Camera.main;
        
        StartCoroutine(MoveShip(cart.GetCinemachineComponent<CinemachineTrackedDolly>()));
    }
    

    private void Update()
    {
        //May not work in mobile...
        if (!IsInMenu && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out UpgradeStationNode n))
            {
                inController =  n;
                inController.SwapCams(1);
                IsInMenu = true;
                mainGUI.SetActive(true);
            }
        }
    }

    public void BackToMain()
    {
        inController.SwapCams(-1);
        inController = null;
        mainGUI.SetActive(false);
    }


    private IEnumerator MoveShip(CinemachineTrackedDolly cartDolly)
    {
        Vector3 startPos = shipStartPoint.position;
        Vector3 endPos = shipEndPoint.position;
        float curTime = 0;
        while (curTime < dollyTime)
        {
            curTime += Time.deltaTime;
            float t = curTime / dollyTime;
            shipTrans.position = Vector3.Lerp(startPos, endPos, t);
            cartDolly.m_PathPosition = Mathf.Lerp(0, 3, Mathf.Sqrt(t*2));
            yield return null;
        }
    }
}
