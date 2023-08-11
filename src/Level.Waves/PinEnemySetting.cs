using Characters;
using UnityEngine;

namespace Level.Waves;

public class PinEnemySetting : MonoBehaviour
{
	[SerializeReference]
	[SubclassSelector]
	private IPinEnemyOption[] _settings = new IPinEnemyOption[1]
	{
		new SetEnemyBehaviorOption(setTargetToPlayer: false, idleUntilFindTarget: false, staticMovement: false)
	};

	public void ApplyTo(Character character)
	{
		IPinEnemyOption[] settings = _settings;
		for (int i = 0; i < settings.Length; i++)
		{
			settings[i].ApplyTo(character);
		}
	}

	public void CopySettingsFromPin(bool setTargetToPlayer, bool idleUntilFindTarget, bool staticMovement)
	{
		_settings = new IPinEnemyOption[1]
		{
			new SetEnemyBehaviorOption(setTargetToPlayer, idleUntilFindTarget, staticMovement)
		};
	}
}
