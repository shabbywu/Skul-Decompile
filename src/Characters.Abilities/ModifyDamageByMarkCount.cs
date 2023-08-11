using System;
using Characters.Marks;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyDamageByMarkCount : Ability
{
	public class Instance : AbilityInstance<ModifyDamageByMarkCount>
	{
		public Instance(Character owner, ModifyDamageByMarkCount ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
		}

		protected override void OnAttach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if ((Object)(object)target.character == (Object)null || !damage.key.Equals(ability._key))
			{
				return false;
			}
			int num = math.min((int)target.character.mark.TakeAllStack(ability._mark), ability._mark.maxStack);
			damage.percentMultiplier *= ability._damagePercents[num];
			return false;
		}
	}

	[SerializeField]
	private MarkInfo _mark;

	[SerializeField]
	private string _key;

	[Tooltip("표식이 없을 때인 0개부터 시작")]
	[SerializeField]
	private float[] _damagePercents;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
