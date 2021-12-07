using System;
using Mirror;
using UnityEngine;

    public class PlaneBehaviour : NetworkBehaviour
    {
        [SerializeField] 
        private Rigidbody2D rigidbody2D;
        
        public PlaneBase PlaneBase { get; private set; }
        
        public Action OnPlaneFixedUpdater = delegate {  };

        public void StartLocalPlayerInit()
        {
            if(isLocalPlayer == false) return;
            PlaneBase = new PlaneBase(ref rigidbody2D);
            OnPlaneFixedUpdater += PlaneBase.CustomFixedUpdate;
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
