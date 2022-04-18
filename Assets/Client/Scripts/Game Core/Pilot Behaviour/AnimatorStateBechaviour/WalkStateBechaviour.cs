using UnityEngine;

public class WalkStateBechaviour : StateMachineBehaviour
{
	private PilotBehaviour _pilotBehaviour;

	private readonly int _idleAnimation = Animator.StringToHash("idle_side");

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(_pilotBehaviour == null)
			_pilotBehaviour = animator.GetComponentInParent<PilotBehaviour>();
		
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(Mathf.RoundToInt(_pilotBehaviour.Velocity.x) == 0)
			animator.Play(_idleAnimation);

		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
