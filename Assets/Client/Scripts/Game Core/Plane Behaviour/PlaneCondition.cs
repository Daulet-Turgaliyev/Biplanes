using System;
using System.Threading.Tasks;
using UnityEngine;


public sealed class PlaneCondition : MonoBehaviour
{
    [field: SerializeField] private ParticleSystem[] particleCondition;

    [SerializeField] private ParticleSystem _particleDestroy;

    private ParticleSystem _currentParticle;

    public Action<int> OnDestroy;
    public Action OnRespawnPlane;

    private void Start()
    {
        _currentParticle = particleCondition[0];
    }

    public void TrySetCondition(int healthStatus, bool hasAuthority, bool isNeedRespawn = true)
    {
        if(_particleDestroy.isPlaying == true) return;

        if (healthStatus > particleCondition.Length - 1 || healthStatus < 0)
        {
            Debug.LogWarning($"healthStatus: {healthStatus}");
            healthStatus = 0;
        }

        if (healthStatus == 0)
        {
            if (isNeedRespawn == true)
                OnRespawnPlane?.Invoke();
            
            DestroyPlane(hasAuthority);
            return;
        }

        _currentParticle.Stop();
        _currentParticle = particleCondition[healthStatus];
        _currentParticle.Play();
    }

    private void DestroyPlane(bool hasAuthority)
    {
        _currentParticle.Stop();
        
        DestroyAnimation();

        if (hasAuthority)
            GameManager.Instance.CloseCurrentWindow();
        
    }

    private void DestroyAnimation()
    {
        _particleDestroy.Play();
        OnDestroy?.Invoke(2);
    }

    private void OnDisable()
    {
        OnDestroy = null;
    }
}
