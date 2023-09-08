using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Antique : SimpleStatBonusKeyword
{
	[Header("2세트 효과")]
	[SerializeField]
	private double[] _healthBonusByStep;

	[SerializeField]
	[Range(0f, 1f)]
	[Header("4세트 효과")]
	private double _healthCondition;

	[SerializeField]
	private Sprite _maxStepStatBonusIcon;

	[SerializeField]
	[Information("Percent", InformationAttribute.InformationType.Info, false)]
	private double _takingDamagePercent;

	[SerializeField]
	private Characters.Abilities.CharacterStat.StatBonus _maxStepStatBonus;

	protected override double[] statBonusByStep => _healthBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.Constant;

	protected override Stat.Kind statKind => Stat.Kind.Health;

	protected override void Initialize()
	{
		base.Initialize();
		_maxStepStatBonus.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step == _healthBonusByStep.Length - 1)
		{
			base.character.health.onChanged += OnHealthChanged;
			OnHealthChanged();
		}
		else
		{
			base.character.health.onChanged -= OnHealthChanged;
			base.character.ability.Remove(_maxStepStatBonus);
		}
	}

	private void OnHealthChanged()
	{
		if (base.character.health.percent >= _healthCondition)
		{
			if (!base.character.ability.Contains(_maxStepStatBonus))
			{
				base.character.ability.Add(_maxStepStatBonus);
			}
		}
		else
		{
			base.character.ability.Remove(_maxStepStatBonus);
		}
	}
}
