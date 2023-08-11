using Characters;
using Level;
using Services;
using Singletons;
using UnityEngine;

public class AdventurerReward : MonoBehaviour
{
	[SerializeField]
	private Character[] _characters;

	[SerializeField]
	private PotionPossibilities _potionPossibilities;

	private Character _lastDiedCharacter;

	private void Start()
	{
		Character[] characters = _characters;
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].health.onDiedTryCatch += CountAdventurerDead;
		}
	}

	private void CountAdventurerDead()
	{
		int num = 0;
		Character[] characters = _characters;
		foreach (Character character in characters)
		{
			if (character.health.dead)
			{
				num++;
			}
			else
			{
				_lastDiedCharacter = character;
			}
		}
		if (num >= _characters.Length)
		{
			DropReward();
			RemoveCountEvent();
		}
	}

	private void DropReward()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Potion potion = _potionPossibilities.Get();
		if ((Object)(object)potion != (Object)null)
		{
			Singleton<Service>.Instance.levelManager.DropPotion(potion, ((Component)_lastDiedCharacter).transform.position);
		}
	}

	private void RemoveCountEvent()
	{
		Character[] characters = _characters;
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].health.onDiedTryCatch -= CountAdventurerDead;
		}
	}

	private void OnDestroy()
	{
		RemoveCountEvent();
	}
}
