using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatByCountsWithinRange : Ability
{
	public class Instance : AbilityInstance<StatByCountsWithinRange>
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

		public Instance(Character owner, StatByCountsWithinRange ability)
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
			UpdateStat();
		}

		private void UpdateStat()
		{
			_count = ability.GetCountWithinRange(((Component)owner).gameObject);
			for (int i = 0; i < _stat.values.Length; i++)
			{
				double num = (double)_count * ability._statPerCount.values[i].value;
				if (ability._statPerCount.values[i].categoryIndex == Stat.Category.Percent.index)
				{
					num += 1.0;
				}
				_stat.values[i].value = num;
			}
			owner.stat.SetNeedUpdate();
		}
	}

	public const float _overlapInterval = 0.15f;

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private Stat.Values _statPerCount;

	[SerializeField]
	private TargetLayer _layer;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	[SerializeField]
	private bool _statusCheck;

	[SerializeField]
	private CharacterStatusKindBoolArray _statusKinds;

	[SerializeField]
	[Information("스탯 배수에 적용할 기본 배수", InformationAttribute.InformationType.Info, false)]
	private int _base;

	[Information("효과가 적용되기 시작하는 최소 개수", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private int _min;

	[SerializeField]
	private int _max;

	public override void Initialize()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		base.Initialize();
		if (_overlapper == null)
		{
			_overlapper = new NonAllocOverlapper(128);
		}
	}

	private int GetCountWithinRange(GameObject gameObject)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(gameObject));
		_overlapper.OverlapCollider(_range);
		ReadonlyBoundedList<Collider2D> results = _overlapper.results;
		int num = 0;
		foreach (Collider2D item in results)
		{
			Target component = ((Component)item).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null))
			{
				Character character = component.character;
				if ((!_statusCheck || (!((Object)(object)character.status == (Object)null) && character.status.IsApplying(_statusKinds))) && _characterTypes[character.type])
				{
					num++;
				}
			}
		}
		if (results == null)
		{
			return 0;
		}
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
