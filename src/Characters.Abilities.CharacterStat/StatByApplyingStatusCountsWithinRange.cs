using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatByApplyingStatusCountsWithinRange : Ability
{
	public class Instance : AbilityInstance<StatByApplyingStatusCountsWithinRange>
	{
		private Stat.Values _stat;

		private int _count;

		private float _remainCheckTime;

		public override int iconStacks => _count;

		public override Sprite icon
		{
			get
			{
				if (_count <= 0)
				{
					return null;
				}
				return base.icon;
			}
		}

		public Instance(Character owner, StatByApplyingStatusCountsWithinRange ability)
			: base(owner, ability)
		{
			_stat = ability._statPerCount.Clone();
		}

		protected override void OnAttach()
		{
			UpdateStat();
			owner.stat.AttachValues(_stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCheckTime -= deltaTime;
			if (_remainCheckTime < 0f)
			{
				_remainCheckTime += 0.25f;
				UpdateStat();
			}
		}

		private void UpdateStat()
		{
			_count = ability.GetCountWithinRange(((Component)owner).gameObject);
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				double num = (double)_count * ((ReorderableArray<Stat.Value>)ability._statPerCount).values[i].value;
				if (((ReorderableArray<Stat.Value>)ability._statPerCount).values[i].categoryIndex == Stat.Category.Percent.index)
				{
					num += 1.0;
				}
				((ReorderableArray<Stat.Value>)_stat).values[i].value = num;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	public const float _overlapInterval = 0.25f;

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private Stat.Values _statPerCount;

	[SerializeField]
	private TargetLayer _layer;

	[SerializeField]
	private CharacterStatusKindBoolArray _targetStatusFilter;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private int _base;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private int _min;

	[SerializeField]
	private int _max;

	public override void Initialize()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		base.Initialize();
		if (_overlapper == null)
		{
			_overlapper = new NonAllocOverlapper(_max);
		}
	}

	private int GetCountWithinRange(GameObject gameObject)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(gameObject));
		((Behaviour)_range).enabled = true;
		_overlapper.OverlapCollider(_range);
		List<Character> components = _overlapper.GetComponents<Character>(true);
		int num = 0;
		foreach (Character item in components)
		{
			if (!((Object)(object)item.status == (Object)null) && item.status.IsApplying(_targetStatusFilter))
			{
				num++;
			}
		}
		((Behaviour)_range).enabled = false;
		if (num > 0)
		{
			num += _base;
		}
		if (num < _min)
		{
			return 0;
		}
		if (num > _max)
		{
			return _max;
		}
		return num;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
