using System;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;


    public sealed class PlaneCondition : MonoBehaviour
    {
        [field:SerializeField]
        public ParticleSystem[] particleCondition { private set; get; }

        [field:SerializeField]
        public ParticleSystem particleDestroy { private set; get; }
        
        private ParticleSystem _currentParticle;

        public Action OnDestroy;
        public Action OnDie;

        private void Start()
        {
            _currentParticle = particleCondition[0];
        }
        
        public void TrySetCondition(int healthStatus, bool hasAuthority)
        {
            if (healthStatus > particleCondition.Length - 1|| healthStatus < 0)
            {
                Debug.LogWarning($"healthStatus: {healthStatus}");
                healthStatus = 0;
            }
            
            if (healthStatus == 0)
            {
                OnDie?.Invoke();
                _currentParticle.Stop();
                
                if(hasAuthority)
                    GameManager.Instance.CloseCurrentWindow();
                
                return;
            }
            
            _currentParticle.Stop();
            _currentParticle = particleCondition[healthStatus];
            _currentParticle.Play();
        }

        private async void DieAnimation()
        {
            particleDestroy.Play();
            await Task.Delay(2000);
            OnDestroy?.Invoke();
        }
        
        private void OnEnable()
        {
            OnDie += DieAnimation;
        }

        private void OnDisable()
        {
            OnDie = null;
            OnDestroy = null;
        }
    }
