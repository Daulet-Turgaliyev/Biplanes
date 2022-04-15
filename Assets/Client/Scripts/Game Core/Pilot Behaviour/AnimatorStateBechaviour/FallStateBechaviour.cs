using UnityEngine;

public class FallStateBechaviour : StateMachineBehaviour
{
	private Rigidbody2D _rigidbody2D;
	
	private readonly int _walkAnimation = Animator.StringToHash("walk_side");
	private readonly int _idleAnimation = Animator.StringToHash("idle_side");
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if(_rigidbody2D == null)
			_rigidbody2D = animator.GetComponentInParent<Rigidbody2D>();
		
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (_rigidbody2D.velocity.y > -0.1f)
		{
			animator.Play(_idleAnimation);
			
			if(_rigidbody2D.velocity.x.Equals(.0f) == false)
				animator.Play(_walkAnimation);
		}

		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}
