using Characters;
using Characters.Abilities;
using UnityEngine;

namespace UI.Hud;

public class AbilityIconDisplay : MonoBehaviour
{
	private const int _maxIcons = 27;

	[SerializeField]
	private AbilityIcon _abilityIconPrefab;

	private AbilityIcon[] _icons;

	private Character _character;

	private void Awake()
	{
		_icons = new AbilityIcon[27];
		bool activeSelf = ((Component)_abilityIconPrefab).gameObject.activeSelf;
		((Component)_abilityIconPrefab).gameObject.SetActive(false);
		for (int i = 0; i < 27; i++)
		{
			_icons[i] = Object.Instantiate<AbilityIcon>(_abilityIconPrefab, ((Component)this).transform);
		}
		((Component)_abilityIconPrefab).gameObject.SetActive(activeSelf);
	}

	public void Initialize(Character character)
	{
		_character = character;
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private IAbilityInstance FindNextAbilityWithIcon(ref int index)
	{
		while (index < _character.ability.Count)
		{
			IAbilityInstance abilityInstance = _character.ability[index];
			if (!((Object)(object)abilityInstance.icon == (Object)null))
			{
				index++;
				return abilityInstance;
			}
			index++;
		}
		return null;
	}

	private void Update()
	{
		if ((Object)(object)_character == (Object)null)
		{
			return;
		}
		int index = 0;
		for (int i = 0; i < 27; i++)
		{
			AbilityIcon abilityIcon = _icons[i];
			IAbilityInstance abilityInstance = FindNextAbilityWithIcon(ref index);
			if (abilityInstance == null)
			{
				((Component)abilityIcon).gameObject.SetActive(false);
				continue;
			}
			((Component)abilityIcon).gameObject.SetActive(true);
			abilityIcon.icon = abilityInstance.icon;
			abilityIcon.fillAmount = abilityInstance.iconFillAmount;
			if (abilityInstance.iconFillInversed)
			{
				abilityIcon.fillAmount = 1f - abilityIcon.fillAmount;
			}
			abilityIcon.clockwise = !abilityInstance.iconFillFlipped;
			abilityIcon.stacks = abilityInstance.iconStacks;
		}
	}
}
