using System.Collections;
using Stats;
using UnityEngine;
using UnityEngine.VFX;

namespace Game
{
    public class Weapon : MonoBehaviour
    {
        //First add self to owner
        [field: SerializeField] public WeaponStats Stats { get; private set; }
        [SerializeField] protected Projectile projectile; // Eventually this will be moved elsewhere to a SO.

        [SerializeField] private VisualEffect onHitFX;
        [SerializeField] private int toSpawn = 3;

        //Array pool structure... Is this smart or even necessary??
        private VisualEffect[] onHitEffects;
        private int index;


        protected int IgnoreLayer;
        protected BaseCharacter Owner;
        private Transform parent;
        private VisualEffect vfx;

        protected static readonly int ShootID = Shader.PropertyToID("Fire");
        protected static readonly int LoopDurID = Shader.PropertyToID("LoopDur");
        protected static readonly int SecondarySizeID = Shader.PropertyToID("SecondarySize");
        private static readonly int ColorID = Shader.PropertyToID("Color");

        [SerializeField] private bool useLoopDur;
        [SerializeField] private bool useColor = true;
        [SerializeField] private bool useSecondaryAmount;

        [SerializeField] protected float loopDur;
        [SerializeField] private Vector2 secondaryAmount;

        private float timeBetweenShots;

        private bool isAlreadyShooting;

        private void Start() //Should be start, This is a function that depends other objects.
        {
            //Player has two layers in order for placements. This is essential to prevent collisions with all parts of the player, while still working for enemies
            int rL = 1 << transform.root.gameObject.layer;

            IgnoreLayer = 1 << gameObject.layer;

            if (rL != LayerMask.NameToLayer("PlayerRoot") || rL != LayerMask.NameToLayer("EnemyRoot")) return;
            
            print("layerCheck: " + rL + " + " + IgnoreLayer);

            if (rL != IgnoreLayer)
                IgnoreLayer += rL;


            IgnoreLayer = ~IgnoreLayer;

            Owner = transform.root.GetComponent<BaseCharacter>();
            if (transform.parent.parent.TryGetComponent(out Turret t))
            {
                parent = transform.parent.parent;
                t.AppendWeapon(this);

            }
            else //Not connected to turret
            {
                parent = Owner.transform;
            }

            //Allows for weapons to force fire.
            Owner.AddWeapon(this, Stats.isTargeting || projectile.isHoming, Stats.isAutomatic);


            vfx = transform.GetChild(0).GetComponent<VisualEffect>();
            Color c = projectile.Color;
            vfx.SetVector4(ColorID, c);

            onHitEffects = new VisualEffect[toSpawn];

            for (int i = 0; i < toSpawn; ++i)
            {
                onHitEffects[i] = Instantiate(onHitFX, GameManager.instance.bulletParent);
                if (useColor)
                    onHitEffects[i].SetVector4(ColorID, c);
                if (useLoopDur)
                    onHitEffects[i].SetFloat(LoopDurID, loopDur);
                if (useSecondaryAmount)
                    onHitEffects[i].SetVector2(SecondarySizeID, secondaryAmount);
            }

            timeBetweenShots = Stats.timeBetweenShots;
        }

        protected void MyProjectileHit(Transform other)
        {
            MyProjectileHit(other.position, other.forward);
        }

        protected void MyProjectileHit(Vector3 pos, Vector3 forward)
        {
            Transform t = onHitEffects[index].transform;
            t.position = pos;
            t.forward = forward;
            onHitEffects[index++].SendEvent(ShootID);
            if (index == toSpawn)
                index = 0;
        }
        

        /// <summary>
        /// Shoots towards a target
        /// </summary>
        /// <param name="target"></param>
        protected virtual void TryShoot(Transform target)
        {


            Vector3 tp = transform.position;
            Projectile instance = Instantiate(projectile, tp, transform.rotation,
                GameManager.instance.bulletParent);

            print("activated VFX?");


            //NO CODE BELOW
            Transform iT = instance.transform;
            if (target)
            {

                if (projectile.isHoming)
                {
                    iT.forward = iT.forward;
                    instance.Init(IgnoreLayer, Owner, () => MyProjectileHit(iT), target);
                    StartCoroutine(ShootLoop(target));
                    return;
                }

                if (Stats.isTargeting)
                {
                    iT.forward = (target.position - tp).normalized;
                }
            }

            instance.Init(IgnoreLayer, Owner, () => MyProjectileHit(iT));

            //Restart loop...
            StartCoroutine(ShootLoop(target));

        }

        public void TryStartFiring(Transform target)
        {
            if (!Owner.CanShoot(Stats.ammoType, Stats.fireCost) || isAlreadyShooting) return;
            isAlreadyShooting = true;
            StartFiring(target);
        }

        protected virtual void StartFiring(Transform target)
        {
            
            timeBetweenShots = Mathf.Max(timeBetweenShots - Stats.chargeDelay, -Stats.chargeDelay);
            StartCoroutine(ShootLoop(target));
        }


        private IEnumerator ShootLoop(Transform target)
        {
            while(timeBetweenShots < Stats.timeBetweenShots)
            {
                timeBetweenShots += Time.deltaTime;
                yield return null;
            }
            if (!Owner.CanShoot(Stats.ammoType, Stats.fireCost, true)) yield break;
            timeBetweenShots = 0;
            TryShoot(target);
            vfx.SendEvent(ShootID);
        }

        public virtual void StopFiring()
        {
            StopAllCoroutines();
            isAlreadyShooting = false;
        }

        private void OnDestroy()
        {
            if (onHitEffects == null) return;
            foreach (VisualEffect v in onHitEffects)
            {
                Destroy(v.gameObject,projectile.Lifetime + 1); // 1s Wait for it to finish (Just in case)  
            }
        }
    }
}
