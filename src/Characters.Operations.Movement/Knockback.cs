using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public class Knockback : TargetedCharacterOperation
{
	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private Transform _transfromOverride;

	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: false, expireOnGround: false);

	public PushInfo pushInfo => _pushInfo;

	public override void Run(Character owner, Character target)
	{
		if (!((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			if ((Object)(object)_transfromOverride == (Object)null)
			{
				target.movement.push.ApplyKnockback(owner, _pushInfo);
			}
			else
			{
				target.movement.push.ApplyKnockback(_transfromOverride, _pushInfo);
			}
		}
	}
}
