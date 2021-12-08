using System;
using Mirror;
using UnityEngine;

    public class PlaneBehaviour : NetworkBehaviour
    {
        [SerializeField] 
        private Rigidbody2D rigidbody2D;
        
        public PlaneBase PlaneBase { get; private set; }
        
        public Action OnPlaneFixedUpdater = delegate {  };

        public void Start()
        {
            if (isLocalPlayer == true)
            {
                PlaneBase = new PlaneBase(ref rigidbody2D);
                OnPlaneFixedUpdater += PlaneBase.CustomFixedUpdate;
                Debug.Log("Plane Base Add " + isLocalPlayer);
                LevelInitializer.Instance.planeBase = PlaneBase;
                return;
            }

            Destroy(this);
        }

        private void OnDisable()
        {
            OnPlaneFixedUpdater = null;
        }

        private void FixedUpdate()
        {
            OnPlaneFixedUpdater?.Invoke();
        }
    }
