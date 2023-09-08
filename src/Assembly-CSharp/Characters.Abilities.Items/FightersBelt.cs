using System;
using Characters.Actions;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class FightersBelt : Ability
{
	public sealed class Instance : AbilityInstance<FightersBelt>
	{
		private int _count;

		private float _remainCooldownTime;

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

		public Instance(Character owner, FightersBelt ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += HandleOnStartAction;
		}

		private void HandleOnStartAction(Characters.Actions.Action action)
		{
			if (!(_remainCooldownTime > 0f) && ability._triggerActionFilter[action.type])
			{
				_count = ability.GetCountWithinRange(((Component)owner).gameObject);
				_remainCooldownTime = ability._cooldownTime - ability._cooldownTimeReducementPerTarget * (float)_count;
				((MonoBehaviour)owner).StartCoroutine(ability._onStartAction.CRun(owner));
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= HandleOnStartAction;
		}
	}

	private NonAllocOverlapper _overlapper;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private CharacterTypeBoolArray _targetCharacterTypeFilter;

	[SerializeField]
	private int _max;

	[Header("충격파")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _cooldownTimeReducementPerTarget;

	[SerializeField]
	private ActionTypeBoolArray _triggerActionFilter;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onStartAction;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	public override void Initialize()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		base.Initialize();
		_onStartAction.Initialize();
		if (_overlapper == null)
		{
			_overlapper = new NonAllocOverlapper(128);
		}
	}

	private int GetCountWithinRange(GameObject gameObject)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(gameObject));
		_overlapper.OverlapCollider(_range);
		ReadonlyBoundedList<Collider2D> results = _overlapper.results;
		int num = 0;
		Target target = default(Target);
		foreach (Collider2D item in results)
		{
			if (((Component)item).TryGetComponent<Target>(ref target) && !((Object)(object)target.character == (Object)null))
			{
				Character character = target.character;
				if (_targetCharacterTypeFilter[character.type])
				{
					num++;
				}
			}
		}
		if (results == null)
		{
			return 0;
		}
		if (num > _max)
		{
			return _max;
		}
		return num;
	}
}
