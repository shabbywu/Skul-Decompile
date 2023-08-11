using System.Collections;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Fortress : InscriptionInstance
{
	private const float _checkInterval = 0.1f;

	[SerializeField]
	[Header("2세트 효과")]
	private float[] _shieldByStep;

	[SerializeField]
	private int _refreshInterval;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Characters.Abilities.Shield _shield;

	[Header("4세트 효과")]
	[SerializeField]
	private StatBonus _statBonus;

	[SerializeField]
	private Nothing _cooldownAbility;

	protected override void Initialize()
	{
		_statBonus.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		_shield.amount = _shieldByStep[keyword.step];
		if (keyword.step < keyword.steps.Count - 1)
		{
			base.character.ability.Remove(_statBonus);
		}
	}

	public override void Attach()
	{
		((MonoBehaviour)this).StartCoroutine("CApplyStatBonus");
		((MonoBehaviour)this).StartCoroutine("CRefreshShield");
	}

	public override void Detach()
	{
		((MonoBehaviour)this).StopCoroutine("CApplyStatBonus");
		((MonoBehaviour)this).StopCoroutine("CRefreshShield");
		base.character.ability.Remove(_shield);
		base.character.ability.Remove(_cooldownAbility);
		base.character.ability.Remove(_statBonus);
	}

	private IEnumerator CRefreshShield()
	{
		yield return null;
		while (true)
		{
			if (keyword.step < 1)
			{
				yield return null;
				continue;
			}
			base.character.ability.Add(_shield);
			base.character.ability.Add(_cooldownAbility);
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)base.character.chronometer.master, _cooldownAbility.duration);
		}
	}

	private IEnumerator CApplyStatBonus()
	{
		yield return null;
		while (true)
		{
			if (keyword.step < keyword.steps.Count - 1)
			{
				yield return null;
				continue;
			}
			if (base.character.health.shield.hasAny)
			{
				base.character.ability.Add(_statBonus);
			}
			else
			{
				base.character.ability.Remove(_statBonus);
			}
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)base.character.chronometer.master, 0.1f);
		}
	}
}
