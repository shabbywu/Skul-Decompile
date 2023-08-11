using System.Collections.Generic;
using Characters.Gear;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using UnityEngine;

namespace UI;

public class StatTexts : MonoBehaviour
{
	[SerializeField]
	private TextLayoutElement _statText;

	[SerializeField]
	private QuintessenceDesc _quintessenceDescription;

	[SerializeField]
	private SkillDesc _skillDescription;

	private Gear _gear;

	private void Awake()
	{
		_gear = ((Component)this).GetComponentInParent<Gear>();
		SetSkillDescription();
		SetQuintessenceDescription();
		SetStatText(_gear.stat.strings);
	}

	private void SetSkillDescription()
	{
		Weapon weapon = _gear as Weapon;
		if (!((Object)(object)weapon == (Object)null))
		{
			for (int i = 0; i < weapon.currentSkills.Count; i++)
			{
				Object.Instantiate<SkillDesc>(_skillDescription, ((Component)this).transform, false).info = weapon.currentSkills[i];
			}
		}
	}

	private void SetQuintessenceDescription()
	{
		Quintessence quintessence = _gear as Quintessence;
		if (!((Object)(object)quintessence == (Object)null))
		{
			Object.Instantiate<QuintessenceDesc>(_quintessenceDescription, ((Component)this).transform, false).text = quintessence.description;
		}
	}

	private void SetStatText(IList<string> _texts)
	{
		for (int i = 0; i < _texts.Count; i++)
		{
			Object.Instantiate<TextLayoutElement>(_statText, ((Component)this).transform, false).text = _texts[i];
		}
	}
}
