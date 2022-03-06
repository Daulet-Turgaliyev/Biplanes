
    using System;
    using Mirror;
    using UnityEngine;

    public class PlaneWeapon : NetworkBehaviour
    {
        private NetworkIdentity _networkIdentity;
        
        [SerializeField] 
        private ABullet bulletPrefab;

        [SerializeField] 
        private Transform projectileMount;
        
        private float _cooldown;
        private float _bulletAcceleration;

        public Action OnFire = delegate { };

        private void Awake()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
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
            if (_networkIdentity.hasAuthority)
                CmdFire();
        }
        
        [Command]
        private void CmdFire()
        {
            ABullet projectile = Instantiate(bulletPrefab, projectileMount.position, transform.rotation);
            projectile.OwnerId =  _networkIdentity.connectionToClient.connectionId;
            NetworkServer.Spawn(projectile.gameObject);
        }
    }
