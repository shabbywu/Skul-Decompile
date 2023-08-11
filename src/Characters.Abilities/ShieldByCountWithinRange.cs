using System;
using System.Collections.Generic;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ShieldByCountWithinRange : Ability
{
	public class Instance : AbilityInstance<ShieldByCountWithinRange>
	{
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

		public Instance(Character owner, ShieldByCountWithinRange ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			UpdateShield();
		}

		protected override void OnDetach()
		{
			owner.health.shield.Remove(ability);
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCheckTime -= deltaTime;
			if (_remainCheckTime < 0f)
			{
				_remainCheckTime += 0.25f;
				UpdateShield();
			}
		}

		private void UpdateShield()
		{
			_count = ability.GetCountWithinRange(((Component)owner).gameObject);
			int num = ability._shieldAmountPerCount * _count;
			owner.health.shield.AddOrUpdate(ability, num);
		}
	}

	public const float _overlapInterval = 0.25f;

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private int _shieldAmountPerCount;

	[SerializeField]
	private TargetLayer _layer;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	[SerializeField]
	private bool _statusCheck;

	[SerializeField]
	private CharacterStatusKindBoolArray _statusKinds;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
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
		int num = ((IEnumerable<Collider2D>)_overlapper.results).Where(delegate(Collider2D result)
		{
			Character component = ((Component)result).GetComponent<Character>();
			if ((Object)(object)component == (Object)null)
			{
				return false;
			}
			return (!_statusCheck || ((!((EnumArray<CharacterStatus.Kind, bool>)_statusKinds)[CharacterStatus.Kind.Burn] || component.status.burning) && (!((EnumArray<CharacterStatus.Kind, bool>)_statusKinds)[CharacterStatus.Kind.Freeze] || component.status.freezed) && (!((EnumArray<CharacterStatus.Kind, bool>)_statusKinds)[CharacterStatus.Kind.Poison] || component.status.poisoned) && (!((EnumArray<CharacterStatus.Kind, bool>)_statusKinds)[CharacterStatus.Kind.Wound] || component.status.wounded) && (!((EnumArray<CharacterStatus.Kind, bool>)_statusKinds)[CharacterStatus.Kind.Stun] || component.status.stuned))) && ((EnumArray<Character.Type, bool>)_characterTypes)[component.type];
		}).Count();
		((Behaviour)_range).enabled = false;
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
