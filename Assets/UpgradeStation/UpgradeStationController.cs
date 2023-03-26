using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeStationController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cart;
    [SerializeField] private Transform myShippyDoo;

    //Lerp between points
    [Header("Dolly ship")] [SerializeField]
    private float dollyTime;

    [SerializeField] private float dollyExitTimeA;
    [SerializeField] private float dollyExitTimeB;
    [SerializeField] private Transform shipStartPoint;
    [SerializeField] private Transform shipEndPoint;
    [SerializeField] private Transform shipEndPointExitA;
    [SerializeField] private Transform shipEndPointExitB;
    [SerializeField] private Transform Twinkle;
    private Camera cam;
    public static bool IsInMenu;
    private UpgradeStationNode inController;

    [SerializeField] private GameObject returnButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private Material outlineMat;
    private readonly int outlineColorID = Shader.PropertyToID("_OutlineColor");
    
    private CinemachineTrackedDolly dolly;

    public static Transform playerShip { get; private set; }


    //On Awake, we should start the cart.
    private void Awake()
    {
        //Follow the ship
        playerShip = Instantiate(myShippyDoo, shipStartPoint.position, Quaternion.identity);
        playerShip.LookAt(shipEndPoint.position, Vector3.up);
        cart.Follow =  playerShip;

        cam = Camera.main;
        dolly = cart.GetCinemachineComponent<CinemachineTrackedDolly>();
        StartCoroutine(MoveShip());
    }

    private void Update()
    {
        //May not work in mobile...
        if (!IsInMenu && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out UpgradeStationNode n))
            {
                inController = n;
                inController.SwapCams(1);
                IsInMenu = true;
                outlineMat.SetColor(outlineColorID, Color.clear);
                returnButton.SetActive(true);
                quitButton.SetActive(false);
            }
        }
    }

    public void BackToMain()
    {
        inController.SwapCams(-1);
        inController = null;
        returnButton.SetActive(false);
        quitButton.SetActive(true);
        outlineMat.SetColor(outlineColorID, Color.green);
    }


    private IEnumerator MoveShip()
    {
        IsInMenu = true;
        Vector3 startPos = shipStartPoint.position;
        Vector3 endPos = shipEndPoint.position;
        float curTime = 0;
        while (curTime < dollyTime)
        {
            curTime += Time.deltaTime;
            float t = curTime / dollyTime;
            playerShip.position = Vector3.Lerp(startPos, endPos, t);
            dolly.m_PathPosition = Mathf.Lerp(0, 3, Mathf.Sqrt(t * 2));
            yield return null;
        }
        IsInMenu = false;
        outlineMat.SetColor(outlineColorID, Color.green);

    }

    public void ExitShop()
    {
        IsInMenu = true;
        outlineMat.SetColor(outlineColorID, Color.clear);
        StartCoroutine(ExitMoveShip());
        cart.LookAt =  playerShip;
    }


    private IEnumerator ExitMoveShip()
    {
        float curTime = 0;
        Vector3 start =  playerShip.position;
        Quaternion lr = Quaternion.LookRotation(shipEndPointExitB.position - start,  playerShip.up);
        Vector3 moveTo = shipEndPointExitA.position;
        while (curTime < dollyExitTimeA)
        {
            curTime += Time.deltaTime;

            float t = curTime / dollyExitTimeA;

            dolly.m_PathPosition = Mathf.Lerp(3, 7, t);
            playerShip.position = Vector3.Lerp(start, moveTo, t);


            //Dolly becomes automated, and follows the player & looks at based on distance.
            yield return null;
        }

        curTime = 0;
        float x = 0;
        while (curTime < dollyExitTimeB)
        {
            curTime += Time.deltaTime;
            x += curTime;
            playerShip.position += x * Time.deltaTime *  playerShip.forward;
            playerShip.rotation = Quaternion.Lerp( playerShip.rotation, lr, Time.deltaTime);
            yield return null;
        }
        
        //Play twinkle
        Transform kx = Instantiate(Twinkle,  playerShip.position, Quaternion.identity);
        kx.LookAt(cart.transform);
        cart.LookAt = kx;
        Destroy(kx.gameObject, 0.6f);
        StartCoroutine(BringToGameScene());
        
        //Hide ship
        playerShip.position = Vector3.zero;
        
    }

    //Temporary code
    private IEnumerator BringToGameScene()
    {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(1);
    }

    
    
}

