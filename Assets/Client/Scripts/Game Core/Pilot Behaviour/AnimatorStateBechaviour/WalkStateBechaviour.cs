using UnityEngine;

public class WalkStateBechaviour : StateMachineBehaviour
{
	private Rigidbody2D _rigidbody2D;

	private readonly int _idleAnimation = Animator.StringToHash("");

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(_rigidbody2D == null)
			_rigidbody2D = animator.GetComponentInParent<Rigidbody2D>();
		
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(_rigidbody2D.velocity.x == .00f)
			animator.Play(_idleAnimation);

		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
