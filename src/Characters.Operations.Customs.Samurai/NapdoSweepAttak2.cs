using System.Collections;
using Characters.Marks;
using Characters.Operations.Attack;
using UnityEngine;

namespace Characters.Operations.Customs.Samurai;

public class NapdoSweepAttak2 : SweepAttack2
{
	[SerializeField]
	private MarkInfo _mark;

	[SerializeField]
	[Tooltip("표식 개수 * _attackLengthMultiplier값까지의 AttackInfo가 적용됨")]
	private float _attackLengthMultiplier;

	protected override IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int index = 0;
		float time = 0f;
		if ((Object)(object)target.character == (Object)null)
		{
			yield break;
		}
		float stack = target.character.mark.GetStack(_mark);
		int length = (int)Mathf.Min(stack * _attackLengthMultiplier, (float)_attackAndEffect.components.Length);
		while ((Object)(object)this != (Object)null && index < length)
		{
			for (; index < length; index++)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				if (!(time >= (castAttackInfoSequence = _attackAndEffect.components[index]).timeToTrigger))
				{
					break;
				}
				target.character.mark.TakeStack(_mark, 1f / _attackLengthMultiplier);
				Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
			}
			yield return null;
			time += base.owner.chronometer.animation.deltaTime;
		}
		target.character.mark.TakeAllStack(_mark);
	}
}
