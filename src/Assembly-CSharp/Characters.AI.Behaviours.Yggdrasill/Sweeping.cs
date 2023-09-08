using System.Collections;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Behaviours.Yggdrasill;

public sealed class Sweeping : Behaviour
{
	[SerializeField]
	private YggdrasillAnimationController _animationController;

	[SerializeField]
	private SweepHandController _sweepHandcontroller;

	[SerializeField]
	private YggdrasillAnimation.Tag _left;

	[SerializeField]
	private YggdrasillAnimation.Tag _right;

	[SerializeField]
	private bool _invulnerable;

	[SerializeField]
	[Header("저지불가")]
	private bool _statusImmune;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (_invulnerable)
		{
			controller.character.cinematic.Attach(this);
		}
		if (_statusImmune)
		{
			controller.character.status.unstoppable.Attach(this);
		}
		_sweepHandcontroller.Select();
		if (_sweepHandcontroller.left)
		{
			yield return _animationController.CPlayAndWaitAnimation(_left);
		}
		else
		{
			yield return _animationController.CPlayAndWaitAnimation(_right);
		}
		if (_invulnerable)
		{
			controller.character.cinematic.Detach(this);
		}
		if (_statusImmune)
		{
			controller.character.status.unstoppable.Detach(this);
		}
		base.result = Result.Done;
	}
}
