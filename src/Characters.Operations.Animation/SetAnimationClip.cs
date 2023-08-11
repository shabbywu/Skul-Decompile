using UnityEngine;

namespace Characters.Operations.Animation;

public class SetAnimationClip : Operation
{
	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _idleClip;

	[SerializeField]
	private AnimationClip _walkClip;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_idleClip = null;
		_walkClip = null;
	}

	public override void Run()
	{
		if (!((Object)(object)_characterAnimation == (Object)null))
		{
			if ((Object)(object)_idleClip != (Object)null)
			{
				_characterAnimation.SetIdle(_idleClip);
			}
			if ((Object)(object)_walkClip != (Object)null)
			{
				_characterAnimation.SetWalk(_walkClip);
			}
		}
	}
}
