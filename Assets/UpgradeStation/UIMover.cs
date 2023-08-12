using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMover : MonoBehaviour
{
    [SerializeField] private float travelTime;
    
    [SerializeField]  private Transform pointBTrans;
    [SerializeField] private UnityEvent onClose;
    private Vector3 pointA;
    private Vector3 pointB;
    private bool atPointB;

    private int prvMenu = -1;


    [SerializeField] private MenuObject[] menus;
    
    
    [Serializable]
    public struct MenuObject
    {
        [SerializeField] private GameObject menuObject;
        [SerializeField] private Image uiObject;

        public void SelectObject(bool state)
        {
            
            if (state)
            {
                menuObject.SetActive(true);
                uiObject.color = new Color(1f,1f,1f);
                uiObject.transform.SetAsLastSibling();
            }
            else
            {
                 menuObject.SetActive(false);
                uiObject.color = new Color(0.6f,0.6f,0.6f);
            }
        }
    }

    private void Start()
    {
        pointA = transform.position;
        pointB = pointBTrans.position;
        foreach (MenuObject m in menus)
        {
            m.SelectObject(false);
        }
    }
    public void SetMenu(int menu)
    {
        print("Setting menu: " + menu + ", " + prvMenu);
        if(prvMenu != -1 )
            menus[prvMenu].SelectObject(false);    
        
        
        if (prvMenu == menu || prvMenu == -1)
        {
            StopAllCoroutines();
            StartCoroutine(Move());
        }
        
        menus[menu].SelectObject(true);
        prvMenu = menu;



    }

    private IEnumerator Move()
    {
        
        float curTime = 0;

        Vector3 a = transform.position;
        Vector3 b = atPointB ? pointA : pointB;

        
        while (curTime < travelTime)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(a, b, curTime / travelTime);
            yield return null;
        }

        if (atPointB)
        {
            menus[prvMenu].SelectObject(false);
            onClose?.Invoke();
            prvMenu = -1;
        }

        atPointB = !atPointB;
    }
}
