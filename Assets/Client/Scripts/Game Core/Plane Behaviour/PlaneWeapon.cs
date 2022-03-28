
    using System;
    using Mirror;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class PlaneWeapon : NetworkBehaviour
    {
        public int PlaneId { get; private set; }
        
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
            PlaneId = Random.Range(1000, 9999);
            // Временно не работает
            _cooldown = settings.CoolDown;
            _bulletAcceleration = settings.BulletAcceleration;
        }

        private void OnEnable() => OnFire += Fire;

        private void OnDisable() => OnFire = null;

        private void Fire()
        {
            if (_networkIdentity.hasAuthority)
                CmdFire(PlaneId);
        }
        
        [Command(requiresAuthority = false)]
        private void CmdFire(int planeId)
        {
            var projectile = Instantiate(bulletPrefab, projectileMount.position, transform.rotation);
            projectile.GetComponent<NetworkMatch>().matchId = MatchController.Instance.GetNetworkMath;
            projectile.OwnerId = planeId;
            NetworkServer.Spawn(projectile.gameObject);
        }
    }
