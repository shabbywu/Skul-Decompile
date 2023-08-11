using UnityEngine;

namespace Characters;

public static class CharacterUtility
{
	public static bool TryFindCharacterComponent(this GameObject gameObject, out Character character)
	{
		if (gameObject.TryGetComponent<Character>(ref character))
		{
			return true;
		}
		Target target = default(Target);
		if (gameObject.TryGetComponent<Target>(ref target))
		{
			character = target.character;
			return true;
		}
		character = null;
		return false;
	}

	public static bool TryFindCharacterComponent(this Component component, out Character character)
	{
		return component.gameObject.TryFindCharacterComponent(out character);
	}
}
