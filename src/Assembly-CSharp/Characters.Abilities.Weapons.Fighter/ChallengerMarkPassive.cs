using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Weapons.Fighter;

[Serializable]
public sealed class ChallengerMarkPassive : Ability
{
	public class Instance : AbilityInstance<ChallengerMarkPassive>
	{
		public Instance(Character owner, ChallengerMarkPassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (ability._component.Count() >= ability._maxCount)
			{
				Detach();
				return;
			}
			ability._component.Add(this);
			owner.stat.AttachValues(ability._stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stat);
			ability._component.Remove(this);
			if (owner.health.dead)
			{
				FindNext();
			}
		}

		private void FindNext()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref ability._overalpper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
			Character closestTarget = GetClosestTarget(ability._overalpper.OverlapCircle(Vector2.op_Implicit(((Component)owner).transform.position), ability._findTargetRadius).GetComponents<Target>(true), Vector2.op_Implicit(((Component)owner).transform.position));
			if ((Object)(object)closestTarget != (Object)null)
			{
				closestTarget.ability.Add(ability);
			}
		}

		private Character GetClosestTarget(List<Target> results, Vector2 center)
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			if (results.Count == 0)
			{
				return null;
			}
			if (results.Count == 1)
			{
				if ((Object)(object)results[0].character == (Object)(object)owner)
				{
					return null;
				}
				return results[0].character;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < results.Count; i++)
			{
				if (!((Object)(object)results[i].character == (Object)(object)owner))
				{
					Collider2D collider = results[i].collider;
					if ((Object)(object)results[i].character != (Object)null)
					{
						collider = (Collider2D)(object)results[i].character.collider;
					}
					float num2 = Vector2.Distance(center, Vector2.op_Implicit(((Component)collider).transform.position));
					if (num > num2)
					{
						index = i;
						num = num2;
					}
				}
			}
			if ((Object)(object)results[index].character == (Object)(object)owner)
			{
				return null;
			}
			return results[index].character;
		}
	}

	[SerializeField]
	private ChallengerMarkPassiveComponent _component;

	[SerializeField]
	private int _maxCount = 1;

	[SerializeField]
	private float _findTargetRadius;

	[SerializeField]
	private Stat.Values _stat;

	private NonAllocOverlapper _overalpper = new NonAllocOverlapper(32);

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
