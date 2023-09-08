using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class ModifyDamageByDashDistance : Ability
{
	private class Instance : AbilityInstance<ModifyDamageByDashDistance>
	{
		private bool _dashing;

		private float _start;

		private float _end;

		private float _buffRemainTime;

		private float _attachedValue;

		private Stat.Values _stats = new Stat.Values(new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.AttackDamage, 1.0));

		private bool _attached;

		public override Sprite icon
		{
			get
			{
				if (!_attached)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override float iconFillAmount => _buffRemainTime / ability._buffDuration;

		public override int iconStacks => (int)_attachedValue;

		public Instance(Character owner, ModifyDamageByDashDistance ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += OnStartAction;
			owner.onEndAction += OnEndAction;
			_buffRemainTime = 0f;
			_attached = false;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_attached)
			{
				_buffRemainTime -= deltaTime;
				if (_buffRemainTime <= 0f)
				{
					DetachStat();
				}
			}
		}

		private void OnStartAction(Characters.Actions.Action obj)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (_dashing && obj.type != 0)
			{
				_dashing = false;
				_end = ((Component)owner).transform.position.x;
				AttachStat();
			}
			if (obj.type == Characters.Actions.Action.Type.Dash)
			{
				_dashing = true;
				_start = ((Component)owner).transform.position.x;
			}
		}

		private void OnEndAction(Characters.Actions.Action obj)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (obj.type == Characters.Actions.Action.Type.Dash)
			{
				_dashing = false;
				_end = ((Component)owner).transform.position.x;
				AttachStat();
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= OnStartAction;
			owner.onEndAction -= OnEndAction;
			DetachStat();
		}

		private void AttachStat()
		{
			float num = Mathf.Abs(_start - _end);
			_attachedValue = Mathf.Lerp(0f, ability._maxStatBonusValue, num / ability._maxDistance);
			for (int i = 0; i < _stats.values.Length; i++)
			{
				_stats.values[i].value = _attachedValue;
			}
			owner.stat.AttachOrUpdateValues(_stats);
			_attached = true;
			_buffRemainTime = ability._buffDuration;
		}

		private void DetachStat()
		{
			owner.stat.DetachValues(_stats);
			_attached = false;
		}
	}

	[SerializeField]
	private float _buffDuration;

	[SerializeField]
	private float _maxDistance;

	[SerializeField]
	[Information("Percent, 100% = 100", InformationAttribute.InformationType.Info, false)]
	private float _maxStatBonusValue;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
