using System.Collections;
using System.Collections.Generic;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.System.Sample
{
    public class Projectile : MonoBehaviour
    {
        public ObjectPool ReturnPool = null;

        private IEnumerator _selfDestructCoroutine = null;

        private void OnCollisionEnter(Collision collision)
        {
            if (_selfDestructCoroutine != null)
                StopCoroutine(_selfDestructCoroutine);

            _selfDestructCoroutine = SelfDestruct();
            StartCoroutine(_selfDestructCoroutine);

            IEnumerator SelfDestruct()
            {
                yield return new WaitForSeconds(3);

                if (ReturnPool != null)
                    ReturnPool.ReturnToPool(Projectiles.Ball.ToString(), gameObject);
            }
        }
    }
}
