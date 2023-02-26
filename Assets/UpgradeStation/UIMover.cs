using System.Collections;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    [SerializeField] private float travelTime;
    
    [SerializeField]  private Transform pointBTrans;
    
    private Vector3 pointA;
    private Vector3 pointB;
    private bool atPointB;

    private void Start()
    {
        pointA = transform.position;
        pointB = pointBTrans.position;
    }

    public void ChangePoints()
    {
        StopAllCoroutines();
        StartCoroutine(Move());
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

        atPointB = !atPointB;
        
    }
}
