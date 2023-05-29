using System.Collections;
using UnityEngine;

namespace Game
{
    public class BeamWeapon : Weapon
    {
        private LineRenderer laserBeamEffect;
        private float distance;
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        private void Awake()
        {
            distance = projectile.Lifetime * 100;
            laserBeamEffect = transform.GetChild(0).GetComponent<LineRenderer>();
            laserBeamEffect.material.SetColor(ColorID, projectile.Color);
            laserBeamEffect.startWidth = projectile.ExplosionSize;
            
            StartFiring(null);
        }

        protected override void TryShoot(Transform target)
        {
            StartCoroutine(BeamLoop());
        }

        protected override void StartFiring(Transform target)
        {
            laserBeamEffect.enabled = true;
            base.StartFiring(target);
        }

        public override void StopFiring()
        {
            base.StopFiring();
            laserBeamEffect.enabled = false;
        }

        private IEnumerator BeamLoop()
        {
            //This will be automatically ended in stop firing.

            while (Owner.CanShoot(Stats.ammoType, Stats.fireCost, true)) // This could be optimized by seperating each can shoot...
            {
                if (Physics.SphereCast(transform.position, projectile.ExplosionSize, transform.forward,
                        out RaycastHit hit, distance, IgnoreLayer))
                {
                    Transform t = hit.transform;
                    if (t.TryGetComponent(out BaseCharacter c))
                    {
                        c.UpdateHealth(Owner, projectile.Damage);
                    }

                    laserBeamEffect.SetPosition(1, hit.distance * 1.57f * Vector3.forward);
                    MyProjectileHit(hit.point, hit.normal);
                }
                else
                {
                    laserBeamEffect.SetPosition(1, Vector3.forward * distance);
                }
                
                yield return null;
            }
            StopFiring(); // If we've escaped... Let's let's just stop firing...
        }

    }
}
