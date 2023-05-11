using System;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private int targetLayers;
    [SerializeField] private LayerMask otherLayers;
    [SerializeField] private int maxWeapons;
    [SerializeField] private float swivelSpeed;
    [SerializeField, Range(0, 1)] private float angBeforeShooting = 0.04f;
    [SerializeField, Range(0, 1)] private float maxAng = 0.1f;
    private readonly Collider [] hits =new Collider[1];
    private Weapon[] weapons;
    private int id;
    private Transform root;
    private void Awake()
    {
        targetLayers = 1<<LayerMask.NameToLayer("Enemy");
        weapons = new Weapon[maxWeapons];
        root = transform.root;
    }

    public void AppendWeapon(Weapon w)
    {
        weapons[id++] = w;
    }


    // Update is called once per frame
    void Update()
    {
        //Scan for targets
        //bool h = Physics.SphereCast(transform.position, 50, transform.forward, out hit, 50,d);
        int h = Physics.OverlapSphereNonAlloc(transform.position, 250, hits,  targetLayers);
        if (id == 0 || h == 0) return;
        
        //If able to look at target
        Vector3 forward = transform.forward;
        //If we have LOS
        Vector3 lookVec = hits[0].bounds.center - transform.position; // Look in direction
        Debug.DrawRay(transform.position, lookVec, Color.yellow);
        if (Physics.Raycast(transform.position, lookVec, out RaycastHit t,lookVec.magnitude,  otherLayers))// Cringe as hell.
        {
            print("Blocked by: " + t.transform.name);
            Debug.DrawRay(transform.position - forward, forward * 50, Color.red);
            return;
        }
        
        Debug.DrawRay(transform.position - forward, forward * 50, Color.green);
        
        

        Quaternion preRot = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookVec), Time.deltaTime * swivelSpeed);

        //If Exceeding max rotation, don't allow rotation
        float dot = Vector3.Dot(forward, root.up);
        //If we are currently out of bounds AND we are trying to go further out of bounds
        if (Mathf.Abs(dot) > maxAng && Mathf.Abs(dot) < Mathf.Abs(Vector3.Dot(transform.forward, root.up)))
        {
            transform.rotation = preRot;
        }

        
        if (Vector3.Dot(transform.forward, lookVec) > angBeforeShooting)
        {
            for (int i = 0; i < id; ++i)
            {
                weapons[i].TryShoot(hits[0].transform);
            }
        }
        //


    }
}
