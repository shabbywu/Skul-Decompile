using Level;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonBDRandomCharacter : Operation
{
	[SerializeField]
	private Character[] _characterPrefabs;

	[SerializeField]
	private Transform _position;

	[SerializeField]
	private bool _containInWave = true;

	[SubclassSelector]
	[SerializeReference]
	private IBDCharacterSetting[] _settings;

	public override void Run()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (_characterPrefabs.Length != 0)
		{
			Character character = ((!((Object)(object)_position != (Object)null)) ? Object.Instantiate<Character>(_characterPrefabs.Random()) : Object.Instantiate<Character>(_characterPrefabs.Random(), _position.position, Quaternion.identity, ((Component)Map.Instance).transform));
			if (_containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
			IBDCharacterSetting[] settings = _settings;
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].ApplyTo(character);
			}
		}
	}
}
