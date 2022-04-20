using System;
using System.Threading.Tasks;
using UnityEngine;


public sealed class PlaneCondition : MonoBehaviour
{
    [field: SerializeField] private ParticleSystem[] particleCondition;

    [SerializeField] private ParticleSystem _particleDestroy;

    private ParticleSystem _currentParticle;
    
    private void Start()
    {
        _currentParticle = particleCondition[0];
    }

    public void TrySetCondition(int healthStatus)
    {
        if (healthStatus > particleCondition.Length - 1 || healthStatus < 0)
        {
            Debug.LogWarning($"healthStatus: {healthStatus}");
            healthStatus = 0;
        }

        ChangeConditionEffect(healthStatus);
    }

    private void ChangeConditionEffect(int effectIndex)
    {
        
        if (effectIndex > particleCondition.Length || effectIndex < 0)
            throw new ArgumentOutOfRangeException($"{nameof(effectIndex)}");
        _currentParticle.Stop();
        _currentParticle = particleCondition[effectIndex];
        _currentParticle.Play();
    }

    public void DestroyAnimation()
    {
        if (_currentParticle != null)
            _currentParticle.Stop();

        _particleDestroy.Play();
    }
}
