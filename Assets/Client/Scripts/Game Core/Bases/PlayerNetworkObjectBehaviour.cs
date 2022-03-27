
    using System;
    using Mirror;
    using UnityEngine;

    public abstract class PlayerNetworkObjectBehaviour: NetworkBehaviour
    {
        protected Action OnFixedUpdater = delegate { };
        
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Initialize();
        }

        protected virtual void Initialize()
        {
            LocalSubscribe();
            GlobalSubscribe();
        }
        
        protected abstract void LocalSubscribe();
        protected abstract void GlobalSubscribe();
        
        protected bool CanSendCommand(NetworkIdentity networkIdentity)
        {
            // != false
            return networkIdentity.hasAuthority && isClient;
        }
        
        private void FixedUpdate()
        {
            OnFixedUpdater?.Invoke();
        }
        
        private void OnDestroy()
        {
            OnFixedUpdater = null;
        }
    }
