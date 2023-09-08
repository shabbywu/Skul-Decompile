using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class TimeGradientStatBonus : Ability
{
	public sealed class Instance : AbilityInstance<TimeGradientStatBonus>
	{
		private Stat.Values _stat;

		private float _remainUpdateTime;

		public Instance(Character owner, TimeGradientStatBonus ability)
			: base(owner, ability)
		{
			_stat = ability._startStat.Clone();
			_remainUpdateTime = ability._updateTime;
		}

		protected override void OnAttach()
		{
			owner.stat.AttachValues(_stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void Refresh()
		{
			base.Refresh();
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._startStat.values[i].value;
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainUpdateTime -= deltaTime;
			if (_remainUpdateTime <= 0f)
			{
				UpdateStat();
				_remainUpdateTime = ability._updateTime;
			}
		}

		private void UpdateStat()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value += ability._delta * 0.01f;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Stat.Values _startStat;

	[SerializeField]
	private float _updateTime = 1f;

	[SerializeField]
	private float _delta;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
