
    using System;
    using Mirror;
    using UnityEngine;

    public class PlaneWeapon : NetworkBehaviour
    {
        [SerializeField] 
        private GameObject bulletPrefab;

        [SerializeField] 
        private Transform projectileMount;
        
        private float _cooldown;
        private float _bulletAcceleration;

        public Action OnFire = delegate { };

        private void Start()
        {
            NetworkClient.RegisterPrefab(bulletPrefab);
        }

        public void Init(PlaneWeaponSettings settings)
        {
            // Временно не работает
            _cooldown = settings.CoolDown;
            _bulletAcceleration = settings.BulletAcceleration;
        }

        private void OnEnable() => OnFire += Fire;

        private void OnDisable() => OnFire = null;

        private void Fire()
        {
            if (isLocalPlayer)
                CmdFire();
        }
        
        [Command]
        private void CmdFire()
        {
            GameObject projectile = Instantiate(bulletPrefab, projectileMount.position, transform.rotation);
            NetworkServer.Spawn(projectile);
        }
    }
