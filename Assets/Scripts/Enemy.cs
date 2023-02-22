using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : BaseCharacter
{
    public static readonly Dictionary<uint, Blip> Blips = new();
    private static uint _shipID;
    private uint id;
    private Vector3 target;
    public static readonly Dictionary<uint, bool> Targeting = new();
    
    protected void Start()
    {
        curSpeed = stats.maxSpeed * 0.8f;
        do
        {
            id = _shipID++;
        } while (Blips.ContainsKey(id));
        Blips.Add(id, new Blip(transform, stats.blip, id));
        target = Random.insideUnitSphere * 100;
        Targeting.Add(id, false);

    }


    private float huntTime;
    protected override void Move()
    {
        
        Vector3 v = (target - transform.position); // 
        float mag = v.sqrMagnitude;

        if (curTarget)
        {
            target = curTarget.position;
            Targeting[id] = true;
        }

        if (mag < 4)
        {
            v = Random.insideUnitSphere;
            v.y *= 0.7f;
            target = v * 100;
        }
        
        //This may be stupid, but if we can get it as a vector 3, then we can conform to the rules.
        Quaternion toRotation = Quaternion.LookRotation(v);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, stats.baseHandling * Time.deltaTime); //This is a cheat, but I'm struggling to get things to work w/ euler
        
        base.Move();

        Debug.DrawLine(transform.position, target, Color.green);

        
    }

    public override void UpdateHealth(Transform attacker, float damage)
    {
        base.UpdateHealth(attacker, damage);

        print("I've been hit!" + damage +" by: " + attacker );
        if (damage < 0)
        {
            curTarget = attacker;
            Targeting[id] = false;
        }
    }

    private void OnDestroy()
    {
        Blips[id].DestroyBlip();
        Blips.Remove(id);
        Targeting.Remove(id);
        #if UNITY_EDITOR
                if (Application.isEditor && !Application.isFocused)
                    return;
        #endif
        if (Blips.Count == 0)
        {
            GameManager.instance.TEMP_END_GAME(true);
        }
    }

    public override void OnTargeted()
    {
        //Apply materials.
        //Record time, and perform evaisive manuevers 
    }

    public override void OnUnTargeted()
    {
        //Remove materials
        //Reset time
    }
}
