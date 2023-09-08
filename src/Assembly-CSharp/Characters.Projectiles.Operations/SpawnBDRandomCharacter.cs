using Characters.Operations;
using Level;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class SpawnBDRandomCharacter : Operation
{
	[SerializeField]
	private Character[] _characterPrefabs;

	[SerializeField]
	[Range(0f, 10f)]
	private float _distribution;

	[SerializeField]
	[Range(1f, 10f)]
	private int _repeatCount;

	[SerializeField]
	private bool _containInWave = true;

	[SerializeReference]
	[SubclassSelector]
	private IBDCharacterSetting[] _settings;

	public override void Run(IProjectile projectile)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (_characterPrefabs.Length == 0)
		{
			return;
		}
		Vector3 position = projectile.transform.position;
		for (int i = 0; i < _repeatCount; i++)
		{
			float num = Random.Range(0f - _distribution, _distribution);
			Random.Range(0f - _distribution, _distribution);
			Vector3 val = position;
			val.x += Random.Range(0f - _distribution, _distribution);
			val.y += Random.Range(0f - _distribution, _distribution);
			Character character = Object.Instantiate<Character>(_characterPrefabs.Random(), val, Quaternion.identity);
			character.ForceToLookAt((num < 0f) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			if (_containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
			IBDCharacterSetting[] settings = _settings;
			for (int j = 0; j < settings.Length; j++)
			{
				settings[j].ApplyTo(character);
			}
		}
	}
}
