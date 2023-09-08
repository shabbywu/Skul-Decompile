using System.Collections;
using Characters.AI.YggdrasillElderEnt;
using UnityEngine;

namespace Characters.AI.Behaviours.Yggdrasill;

public sealed class PlayAnimations : Behaviour
{
	[SerializeField]
	private YggdrasillAnimationController _controller;

	[SerializeField]
	private YggdrasillAnimation.Tag[] _tags;

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
		if (_tags == null || _tags.Length == 0)
		{
			Debug.LogError((object)"tag list error in PlayAnimations Behaviour");
			base.result = Result.Fail;
			yield break;
		}
		YggdrasillAnimation.Tag[] tags = _tags;
		foreach (YggdrasillAnimation.Tag tag in tags)
		{
			yield return _controller.CPlayAndWaitAnimation(tag);
		}
		if (_invulnerable)
		{
			controller.character.cinematic.Detach(this);
		}
		if (_statusImmune)
		{
			controller.character.status.unstoppable.Detach(this);
		}
		base.result = Result.Success;
	}
}
