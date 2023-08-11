using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Yaksha;

[Serializable]
public class YakshaHomePassive : Ability, IAbilityInstance
{
	[SerializeField]
	[Header("게이지 바 설정")]
	private Color _chargedBuffColor;

	[SerializeField]
	private ValueGauge _gauge;

	private Color _chargedBaseColor;

	private float _gaugeAnimationTime;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 0f;

	public int iconStacks { get; protected set; }

	public bool expired => false;

	public override void Initialize()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		base.Initialize();
		_chargedBaseColor = _gauge.defaultBarGaugeColor.chargedColor;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if (!(_gauge.gaugePercent < 1f))
		{
			_gaugeAnimationTime += deltaTime * 2f;
			if (_gaugeAnimationTime > 2f)
			{
				_gaugeAnimationTime = 0f;
			}
			_gauge.defaultBarGaugeColor.chargedColor = Color.LerpUnclamped(_chargedBaseColor, _chargedBuffColor, (_gaugeAnimationTime < 1f) ? _gaugeAnimationTime : (2f - _gaugeAnimationTime));
		}
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
	}

	public void Detach()
	{
	}
}
