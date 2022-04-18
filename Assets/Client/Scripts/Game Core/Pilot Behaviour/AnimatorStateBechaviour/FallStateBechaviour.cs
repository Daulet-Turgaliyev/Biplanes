using UnityEngine;

public class FallStateBechaviour : StateMachineBehaviour
{
	private PilotBehaviour _pilotBehaviour;
	
	private readonly int _walkAnimation = Animator.StringToHash("walk_side");
	private readonly int _idleAnimation = Animator.StringToHash("idle_side");
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(_pilotBehaviour == null)
			_pilotBehaviour = animator.GetComponentInParent<PilotBehaviour>();
		
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_pilotBehaviour.Velocity.y > -0.1f)
		{
			animator.Play(_idleAnimation);
			
			if(_pilotBehaviour.Velocity.x != 0.0f)
				animator.Play(_walkAnimation);
		}

		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
