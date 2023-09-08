using Characters;
using UnityEngine;

public class AdventurerHealthBarUIController : MonoBehaviour
{
	public enum AdventurerClass
	{
		Hero,
		Archer,
		Cleric,
		Warrior,
		Thief,
		Magician
	}

	[SerializeField]
	private AdventurerHealthBar _hero;

	[SerializeField]
	private AdventurerHealthBar _archer;

	[SerializeField]
	private AdventurerHealthBar _cleric;

	[SerializeField]
	private AdventurerHealthBar _warrior;

	[SerializeField]
	private AdventurerHealthBar _thief;

	[SerializeField]
	private AdventurerHealthBar _magician;

	public void Initialize(Character character, AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerClass.Hero:
			((Component)_hero).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		case AdventurerClass.Archer:
			((Component)_archer).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		case AdventurerClass.Cleric:
			((Component)_cleric).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		case AdventurerClass.Warrior:
			((Component)_warrior).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		case AdventurerClass.Thief:
			((Component)_thief).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		case AdventurerClass.Magician:
			((Component)_magician).GetComponent<CharacterHealthBar>().Initialize(character);
			break;
		}
	}

	public void ShowHealthBarOf(AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerClass.Hero:
			((Component)_hero).gameObject.SetActive(true);
			break;
		case AdventurerClass.Archer:
			((Component)_archer).gameObject.SetActive(true);
			break;
		case AdventurerClass.Cleric:
			((Component)_cleric).gameObject.SetActive(true);
			break;
		case AdventurerClass.Warrior:
			((Component)_warrior).gameObject.SetActive(true);
			break;
		case AdventurerClass.Thief:
			((Component)_thief).gameObject.SetActive(true);
			break;
		case AdventurerClass.Magician:
			((Component)_magician).gameObject.SetActive(true);
			break;
		}
	}

	public void HideHealthBarOf(AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerClass.Hero:
			((Component)_hero).gameObject.SetActive(false);
			break;
		case AdventurerClass.Archer:
			((Component)_archer).gameObject.SetActive(false);
			break;
		case AdventurerClass.Cleric:
			((Component)_cleric).gameObject.SetActive(false);
			break;
		case AdventurerClass.Warrior:
			((Component)_warrior).gameObject.SetActive(false);
			break;
		case AdventurerClass.Thief:
			((Component)_thief).gameObject.SetActive(false);
			break;
		case AdventurerClass.Magician:
			((Component)_magician).gameObject.SetActive(false);
			break;
		}
	}

	public void HideHealthBarAll()
	{
		AdventurerHealthBar hero = _hero;
		if (hero != null)
		{
			((Component)hero).gameObject.SetActive(false);
		}
		AdventurerHealthBar archer = _archer;
		if (archer != null)
		{
			((Component)archer).gameObject.SetActive(false);
		}
		AdventurerHealthBar cleric = _cleric;
		if (cleric != null)
		{
			((Component)cleric).gameObject.SetActive(false);
		}
		AdventurerHealthBar warrior = _warrior;
		if (warrior != null)
		{
			((Component)warrior).gameObject.SetActive(false);
		}
		AdventurerHealthBar thief = _thief;
		if (thief != null)
		{
			((Component)thief).gameObject.SetActive(false);
		}
		AdventurerHealthBar magician = _magician;
		if (magician != null)
		{
			((Component)magician).gameObject.SetActive(false);
		}
	}

	public void ShowDeadUIOf(AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerClass.Hero:
			_hero.ShowDeadPortrait();
			break;
		case AdventurerClass.Archer:
			_archer.ShowDeadPortrait();
			break;
		case AdventurerClass.Cleric:
			_cleric.ShowDeadPortrait();
			break;
		case AdventurerClass.Warrior:
			_warrior.ShowDeadPortrait();
			break;
		case AdventurerClass.Thief:
			_thief.ShowDeadPortrait();
			break;
		case AdventurerClass.Magician:
			_magician.ShowDeadPortrait();
			break;
		}
	}

	public void HideDeadUIOf(AdventurerClass adventurerClass)
	{
		switch (adventurerClass)
		{
		case AdventurerClass.Hero:
			_hero.HideDeadPortrait();
			break;
		case AdventurerClass.Archer:
			_archer.HideDeadPortrait();
			break;
		case AdventurerClass.Cleric:
			_cleric.HideDeadPortrait();
			break;
		case AdventurerClass.Warrior:
			_warrior.HideDeadPortrait();
			break;
		case AdventurerClass.Thief:
			_thief.HideDeadPortrait();
			break;
		case AdventurerClass.Magician:
			_magician.HideDeadPortrait();
			break;
		}
	}

	public void HideDeadUIAll()
	{
		_hero?.HideDeadPortrait();
		_archer?.HideDeadPortrait();
		_cleric?.HideDeadPortrait();
		_warrior?.HideDeadPortrait();
		_thief?.HideDeadPortrait();
		_magician?.HideDeadPortrait();
	}
}
