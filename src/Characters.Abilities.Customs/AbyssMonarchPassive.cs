using System;
using System.Linq;
using Characters.Abilities.Constraints;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class AbyssMonarchPassive : Ability
{
	public class Instance : AbilityInstance<AbyssMonarchPassive>
	{
		private readonly Stat.Values _statPerStack;

		private static readonly NonAllocOverlapper _enemyOverlapper;

		private static readonly int _maximum;

		private Stat.Values _stat;

		private float _remainFindTime;

		static Instance()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			_maximum = 255;
			_enemyOverlapper = new NonAllocOverlapper(_maximum);
			((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		}

		public Instance(Character owner, AbyssMonarchPassive ability)
			: base(owner, ability)
		{
			_statPerStack = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, ability._speedBonusPerTarget));
		}

		protected override void OnAttach()
		{
			_stat = _statPerStack.Clone();
			owner.stat.AttachValues(_stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (ability._constraints.Pass())
			{
				_remainFindTime -= deltaTime;
				if (_remainFindTime <= 0f)
				{
					UpdateStack(GetAbyssEnemyCountAround());
					_remainFindTime = ability._findInterval;
				}
			}
		}

		private int GetAbyssEnemyCountAround()
		{
			Collider2D findRange = ability._findRange;
			((Behaviour)findRange).enabled = true;
			int result = (from character in _enemyOverlapper.OverlapCollider(findRange).GetComponents<Character>(true)
				where character.ability.GetInstance<Abyss>() != null
				select character).Count();
			((Behaviour)findRange).enabled = false;
			return result;
		}

		private void UpdateStack(int stack)
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = _statPerStack.values[i].GetStackedValue(stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	[Space]
	private Collider2D _findRange;

	[SerializeField]
	private float _findInterval;

	[SerializeField]
	private float _speedBonusPerTarget;

	[Constraint.Subcomponent]
	[SerializeField]
	[Space]
	private Constraint.Subcomponents _constraints;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
