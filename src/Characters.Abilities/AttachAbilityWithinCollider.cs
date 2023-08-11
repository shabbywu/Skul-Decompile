using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AttachAbilityWithinCollider : Ability
{
	public enum ChronometerType
	{
		Master,
		Animation,
		Effect,
		Projectile
	}

	public class Instance : AbilityInstance<AttachAbilityWithinCollider>
	{
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		private DoubleBuffered<List<Character>> _charactersWithinCollider;

		private float _remainCheckTime;

		public Instance(Character owner, AttachAbilityWithinCollider ability)
			: base(owner, ability)
		{
			if (ability._optimizedCollider)
			{
				((Behaviour)ability._collider).enabled = false;
			}
			_charactersWithinCollider = new DoubleBuffered<List<Character>>(new List<Character>(_sharedOverlapper.capacity), new List<Character>(_sharedOverlapper.capacity));
			ability._abilityComponents.Initialize();
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
			for (int i = 0; i < _charactersWithinCollider.Current.Count; i++)
			{
				Character character = _charactersWithinCollider.Current[i];
				AbilityComponent[] components = ((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components;
				if (!((Object)(object)character == (Object)null))
				{
					for (int j = 0; j < components.Length; j++)
					{
						character.ability.Remove(components[j].ability);
					}
				}
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCheckTime -= deltaTime;
			if (_remainCheckTime < 0f)
			{
				_remainCheckTime = ability._checkInterval;
				Check();
			}
		}

		private void Check()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			((Behaviour)ability._collider).enabled = true;
			((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(ability._layer.Evaluate(((Component)owner).gameObject));
			_sharedOverlapper.OverlapCollider(ability._collider);
			if (ability._optimizedCollider)
			{
				((Behaviour)ability._collider).enabled = false;
			}
			for (int i = 0; i < _sharedOverlapper.results.Count; i++)
			{
				Target component = ((Component)_sharedOverlapper.results[i]).GetComponent<Target>();
				Character character;
				if ((Object)(object)component == (Object)null)
				{
					Minion component2 = ((Component)_sharedOverlapper.results[i]).GetComponent<Minion>();
					if ((Object)(object)component2 == (Object)null || !((EnumArray<Character.Type, bool>)ability._characterTypeFilter)[Character.Type.PlayerMinion])
					{
						continue;
					}
					character = component2.character;
				}
				else
				{
					if ((Object)(object)component.character == (Object)null || !((EnumArray<Character.Type, bool>)ability._characterTypeFilter)[component.character.type])
					{
						continue;
					}
					character = component.character;
				}
				_charactersWithinCollider.Next.Add(character);
				int num = _charactersWithinCollider.Current.IndexOf(character);
				if (num >= 0)
				{
					_charactersWithinCollider.Current.RemoveAt(num);
					continue;
				}
				for (int j = 0; j < ((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components.Length; j++)
				{
					character.ability.Add(((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components[j].ability);
				}
			}
			for (int k = 0; k < _charactersWithinCollider.Current.Count; k++)
			{
				Character character2 = _charactersWithinCollider.Current[k];
				if (!((Object)(object)character2 == (Object)null))
				{
					for (int l = 0; l < ((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components.Length; l++)
					{
						character2.ability.Remove(((SubcomponentArray<AbilityComponent>)ability._abilityComponents).components[l].ability);
					}
				}
			}
			_charactersWithinCollider.Current.Clear();
			_charactersWithinCollider.Swap();
		}
	}

	[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
	[SerializeField]
	[Range(0.1f, 1f)]
	private float _checkInterval = 0.33f;

	[SerializeField]
	[Header("Filter")]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, true, true, true);

	[SerializeField]
	[Header("Collider")]
	private Collider2D _collider;

	[SerializeField]
	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	private bool _optimizedCollider = true;

	[Header("Abilities")]
	[Space]
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
