using System.Collections;
using System.Collections.Generic;
using Characters.Gear.Weapons.Gauges;
using Characters.Usables;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Customs;

public class EntColliderAbilityAttacher : AbilityAttacher
{
	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

	[SerializeField]
	private float _checkInterval = 0.1f;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
	private bool _optimizedCollider = true;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	[Header("Abilities")]
	private AbilityComponent.Subcomponents _abilityComponents;

	[SerializeField]
	private bool _gaugeControls;

	[SerializeField]
	private Color _defaultBarColor;

	[SerializeField]
	private Color _buffBarColor;

	[SerializeField]
	private ValueGauge _gauge;

	private CoroutineReference _cCheckReference;

	private float _gaugeAnimationTime;

	private void Awake()
	{
		if (_optimizedCollider)
		{
			((Behaviour)_collider).enabled = false;
		}
	}

	public override void OnIntialize()
	{
		_abilityComponents.Initialize();
	}

	public override void StartAttach()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		((CoroutineReference)(ref _cCheckReference)).Stop();
		_cCheckReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CCheck());
	}

	public override void StopAttach()
	{
		((CoroutineReference)(ref _cCheckReference)).Stop();
		if (!((Object)(object)base.owner == (Object)null))
		{
			AbilityComponent[] components = ((SubcomponentArray<AbilityComponent>)_abilityComponents).components;
			foreach (AbilityComponent abilityComponent in components)
			{
				base.owner.ability.Remove(abilityComponent.ability);
			}
		}
	}

	private IEnumerator CCheck()
	{
		while (true)
		{
			((Behaviour)_collider).enabled = true;
			((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)base.owner).gameObject));
			List<Liquid> components = _sharedOverlapper.OverlapCollider(_collider).GetComponents<Liquid>(true);
			if (_optimizedCollider)
			{
				((Behaviour)_collider).enabled = false;
			}
			if (components.Count > 0)
			{
				for (int i = 0; i < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; i++)
				{
					base.owner.ability.Add(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[i].ability);
				}
				if (_gaugeControls)
				{
					_gauge.defaultBarGaugeColor.baseColor = _defaultBarColor;
				}
			}
			else
			{
				if (_gaugeControls)
				{
					float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
					_gaugeAnimationTime += deltaTime * 2f;
					if (_gaugeAnimationTime > 2f)
					{
						_gaugeAnimationTime = 0f;
					}
					_gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(_defaultBarColor, _buffBarColor, (_gaugeAnimationTime < 1f) ? _gaugeAnimationTime : (2f - _gaugeAnimationTime));
				}
				for (int j = 0; j < ((SubcomponentArray<AbilityComponent>)_abilityComponents).components.Length; j++)
				{
					base.owner.ability.Remove(((SubcomponentArray<AbilityComponent>)_abilityComponents).components[j].ability);
				}
			}
			yield return Chronometer.global.WaitForSeconds(_checkInterval);
		}
	}
}
