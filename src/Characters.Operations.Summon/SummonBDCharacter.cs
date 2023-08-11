using Level;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonBDCharacter : Operation
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Transform _position;

	[SerializeField]
	private bool _containInWave = true;

	[SerializeReference]
	[SubclassSelector]
	private IBDCharacterSetting[] _settings;

	public override void Run()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Character character = ((!((Object)(object)_position != (Object)null)) ? Object.Instantiate<Character>(_character) : Object.Instantiate<Character>(_character, _position.position, Quaternion.identity, ((Component)Map.Instance).transform));
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
