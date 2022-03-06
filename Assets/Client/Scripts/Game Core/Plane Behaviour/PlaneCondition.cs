using System;
using System.Threading.Tasks;
using UnityEngine;


public sealed class PlaneCondition : MonoBehaviour
{
    [field: SerializeField] private ParticleSystem[] particleCondition;

    [SerializeField] private ParticleSystem _particleDestroy;

    private ParticleSystem _currentParticle;

    public Action OnDestroy;
    public Action OnRespawnPlane;

    private void Start()
    {
        _currentParticle = particleCondition[0];
    }

    public void TrySetCondition(int healthStatus, bool hasAuthority)
    {
        if (healthStatus > particleCondition.Length - 1 || healthStatus < 0)
        {
            Debug.LogWarning($"healthStatus: {healthStatus}");
            healthStatus = 0;
        }

        if (healthStatus == 0)
        {
            DiePlane(hasAuthority);
            return;
        }

        _currentParticle.Stop();
        _currentParticle = particleCondition[healthStatus];
        _currentParticle.Play();
    }

    public void DiePlane(bool hasAuthority, bool isSystemDestroy = false)
    {
        _currentParticle.Stop();
        
        DieAnimation();

        if (hasAuthority)
            GameManager.Instance.CloseCurrentWindow();

        if (isSystemDestroy == false)
            OnRespawnPlane?.Invoke();

    }

    private async void DieAnimation()
    {
        _particleDestroy.Play();
        await Task.Delay(2000);
        OnDestroy?.Invoke();
    }

    private void OnDisable()
    {
        OnDestroy = null;
    }
}
