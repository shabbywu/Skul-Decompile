using System.Collections;
using System.Collections.Generic;
using Characters.Abilities;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class AttachAbilityWithinCollider : CharacterOperation
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

	[SerializeField]
	private float _duration;

	[SerializeField]
	[Range(0.1f, 1f)]
	[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
	private float _checkInterval = 0.33f;

	[Header("Filter")]
	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(true, true, true, true, true, true, true, true);

	[SerializeField]
	private bool _excludeSelf;

	[Header("Collider")]
	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	private bool _optimizedCollider = true;

	[SerializeField]
	[Header("Abilities")]
	[Space]
	[AbilityComponent.Subcomponent]
	private AbilityComponent.Subcomponents _abilityComponents;

	private DoubleBuffered<List<Character>> _charactersWithinCollider;

	private CoroutineReference _cCheckReference;

	private void Awake()
	{
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
		_charactersWithinCollider = new DoubleBuffered<List<Character>>(new List<Character>(_sharedOverlapper.capacity), new List<Character>(_sharedOverlapper.capacity));
		_abilityComponents.Initialize();
		if (_duration <= 0f)
		{
			_duration = float.PositiveInfinity;
		}
	}

	public override void Run(Character owner)
	{
		_charactersWithinCollider.Current.Clear();
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	public override void Stop()
	{
		((CoroutineReference)(ref _cCheckReference)).Stop();
		foreach (Character item in _charactersWithinCollider.Current)
		{
			AbilityComponent[] components = ((SubcomponentArray<AbilityComponent>)_abilityComponents).components;
			foreach (AbilityComponent abilityComponent in components)
			{
				item.ability.Remove(abilityComponent.ability);
			}
		}
		_charactersWithinCollider.Current.Clear();
	}

	private IEnumerator CRun(Character owner)
	{
		((CoroutineReference)(ref _cCheckReference)).Stop();
		_cCheckReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CCheck(owner));
		yield return (object)new WaitForSeconds(_duration);
		((CoroutineReference)(ref _cCheckReference)).Stop();
	}

	private IEnumerator CCheck(Character owner)
	{
		while (true)
		{
			((Behaviour)_collider).enabled = true;
			((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
			_sharedOverlapper.OverlapCollider(_collider);
			if (_optimizedCollider)
			{
				((Behaviour)_collider).enabled = false;
			}
			for (int i = 0; i < _sharedOverlapper.results.Count; i++)
			{
				Target component = ((Component)_sharedOverlapper.results[i]).GetComponent<Target>();
				Character character;
				if ((Object)(object)component == (Object)null)
				{
					Minion component2 = ((Component)_sharedOverlapper.results[i]).GetComponent<Minion>();
					if ((Object)(object)component2 == (Object)null || !((EnumArray<Character.Type, bool>)_characterTypeFilter)[Character.Type.PlayerMinion])
					{
						continue;
					}
					character = component2.character;
				}
				else
				{
					if ((Object)(object)component.character == (Object)null || !((EnumArray<Character.Type, bool>)_characterTypeFilter)[component.character.type])
					{
						continue;
					}
					character = component.character;
				}
				if (_excludeSelf && (Object)(object)character == (Object)(object)owner)
				{
					continue;
				}
				_charactersWithinCollider.Next.Add(character);
				int num = _charactersWithinCollider.Current.IndexOf(character);
				if (num >= 0)
				{
					_charactersWithinCollider.Current.RemoveAt(num);
					continue;
				}
				for (int j = 0; j < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; j++)
				{
					character.ability.Add(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[j].ability);
				}
			}
			for (int k = 0; k < _charactersWithinCollider.Current.Count; k++)
			{
				Character character2 = _charactersWithinCollider.Current[k];
				for (int l = 0; l < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; l++)
				{
					character2.ability.Remove(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[l].ability);
				}
			}
			_charactersWithinCollider.Current.Clear();
			_charactersWithinCollider.Swap();
			yield return (object)new WaitForSecondsRealtime(_checkInterval);
		}
	}
}
