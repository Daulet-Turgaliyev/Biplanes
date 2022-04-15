using UnityEngine;

public class IdleStateBechaviour: StateMachineBehaviour
{
	private Rigidbody2D _rigidbody2D;
	private readonly int _fallAnimation = Animator.StringToHash("fall_side");
	private readonly int _walkAnimation = Animator.StringToHash("walk_side");

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_rigidbody2D == null)
			_rigidbody2D = animator.GetComponentInParent<Rigidbody2D>();

		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_rigidbody2D.velocity.y < 0)
			animator.Play(_fallAnimation);

		animator.Play(_walkAnimation);

		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
