using System.Collections;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs;

public class LivingArmor2PassiveAttacher : AbilityAttacher
{
	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	[Header("Passive Components")]
	private LivingArmorPassiveComponent _livingArmorPassive;

	[SerializeField]
	private LivingArmorPassiveComponent _livingArmorPassive2;

	[Range(0f, 1f)]
	[SerializeField]
	private float _enhancedGaugePercent;

	private float _targetValue;

	private bool alreadyAttached;

	public override void OnIntialize()
	{
		_targetValue = _gauge.maxValue * _enhancedGaugePercent;
		_livingArmorPassive.Initialize();
		_livingArmorPassive2.Initialize();
	}

	public override void StartAttach()
	{
		((MonoBehaviour)base.owner).StartCoroutine(CStartAttach());
	}

	private IEnumerator CStartAttach()
	{
		yield return null;
		_gauge.onChanged += OnGaugeValueChanged;
		if (!alreadyAttached)
		{
			OnGaugeValueChanged(0f, _gauge.currentValue);
			alreadyAttached = true;
		}
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			_gauge.onChanged -= OnGaugeValueChanged;
			base.owner.ability.Remove(_livingArmorPassive.ability);
			base.owner.ability.Remove(_livingArmorPassive2.ability);
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}

	private void OnGaugeValueChanged(float oldValue, float newValue)
	{
		if (!(oldValue > newValue))
		{
			if (newValue == _gauge.maxValue && !base.owner.ability.Contains(_livingArmorPassive2.ability))
			{
				base.owner.ability.Remove(_livingArmorPassive.ability);
				base.owner.ability.Add(_livingArmorPassive2.ability);
			}
			else if (_targetValue <= newValue && !base.owner.ability.Contains(_livingArmorPassive.ability) && !base.owner.ability.Contains(_livingArmorPassive2.ability))
			{
				base.owner.ability.Add(_livingArmorPassive.ability);
			}
		}
	}
}
