using System.Collections;
using System.Collections.Generic;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.System.Sample
{
    public class ObjectLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab = null;
        [SerializeField] private Transform _firePoint = null;
        [SerializeField] private float _force = 10f;

        [SerializeField] private Transform _holder = null;
        [SerializeField] private int _defaultSize = 10;
        [SerializeField] private int _maxSize = 1000;

        private ObjectPool _pool = null;

        private void Awake()
        {
            _pool = new ObjectPool();
            _pool.OnAddToPool += InjectPool;
            _pool.OnReturnToPool += ResetProjectile;
        }

        private void Start()
        {
            _pool.AddPool(_prefab, _holder, Projectiles.Ball.ToString());
        }

        private void InjectPool(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Projectile>(out Projectile p))
                p.ReturnPool = _pool;
        }

        private void ResetProjectile(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.velocity = Vector3.zero;
        }

        public void Fire()
        {
            GameObject go = _pool.TakeFromPool(Projectiles.Ball.ToString());
            go.transform.position = _firePoint.position;
            go.SetActive(true);

            if (go.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.AddForce(_force * _firePoint.forward, ForceMode.Impulse);
        }
    }

    public enum Projectiles { Ball, }
}
