using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public sealed class AttachAbilityToMinionWithinCollider : Ability
{
	[SerializeField]
	public enum ChronometerType
	{
		Master,
		Animation,
		Effect,
		Projectile
	}

	public class Instance : AbilityInstance<AttachAbilityToMinionWithinCollider>
	{
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

		private DoubleBuffered<List<Character>> _charactersWithinCollider;

		private float _remainCheckTime;

		public Instance(Character owner, AttachAbilityToMinionWithinCollider ability)
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
				AbilityComponent[] components = ability._abilityComponents.components;
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
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			((Behaviour)ability._collider).enabled = true;
			((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
			_sharedOverlapper.OverlapCollider(ability._collider);
			if (ability._optimizedCollider)
			{
				((Behaviour)ability._collider).enabled = false;
			}
			for (int i = 0; i < _sharedOverlapper.results.Count; i++)
			{
				Minion component = ((Component)_sharedOverlapper.results[i]).GetComponent<Minion>();
				if ((Object)(object)component == (Object)null)
				{
					continue;
				}
				bool flag = false;
				Key[] targetKeys = ability._targetKeys;
				foreach (Key key in targetKeys)
				{
					if (component.character.key == key)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					continue;
				}
				_charactersWithinCollider.Next.Add(component.character);
				int num = _charactersWithinCollider.Current.IndexOf(component.character);
				if (num >= 0)
				{
					_charactersWithinCollider.Current.RemoveAt(num);
					continue;
				}
				for (int k = 0; k < ability._abilityComponents.components.Length; k++)
				{
					component.character.ability.Add(ability._abilityComponents.components[k].ability);
				}
			}
			for (int l = 0; l < _charactersWithinCollider.Current.Count; l++)
			{
				Character character = _charactersWithinCollider.Current[l];
				if (!((Object)(object)character == (Object)null))
				{
					for (int m = 0; m < ability._abilityComponents.components.Length; m++)
					{
						character.ability.Remove(ability._abilityComponents.components[m].ability);
					}
				}
			}
			_charactersWithinCollider.Current.Clear();
			_charactersWithinCollider.Swap();
		}
	}

	[SerializeField]
	[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
	[Range(0.1f, 1f)]
	private float _checkInterval = 0.33f;

	[SerializeField]
	private Key[] _targetKeys;

	[SerializeField]
	[Header("Collider")]
	private Collider2D _collider;

	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	[SerializeField]
	private bool _optimizedCollider = true;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	[Space]
	[Header("Abilities")]
	private AbilityComponent.Subcomponents _abilityComponents;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
