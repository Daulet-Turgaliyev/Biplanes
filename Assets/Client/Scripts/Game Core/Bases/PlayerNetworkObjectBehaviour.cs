    using System;
    using Mirror;

    public abstract class PlayerNetworkObjectBehaviour: NetworkBehaviour
    {
        protected Action OnFixedUpdater = delegate { };
        public Action<NetworkIdentity, bool, bool, int, int> OnDie;
        
        protected Action<bool> OnGround = b => { };
        
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

        private void FixedUpdate()
        {
            OnFixedUpdater?.Invoke();
        }
        
        private void OnDestroy()
        {
            OnFixedUpdater = null;
            OnGround = null;
            OnDie = null;
        }
    }
