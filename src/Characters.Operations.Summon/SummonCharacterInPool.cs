using Services;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonCharacterInPool : CharacterOperation
{
	[SerializeField]
	private Character _characterToSummon;

	[SerializeField]
	private Transform _summonTransform;

	[Range(1f, 10f)]
	[SerializeField]
	private int _cacheCount = 1;

	[Range(0f, 100f)]
	[SerializeField]
	private int _spawnChance = 50;

	private Character[] _pool;

	private void Awake()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		_pool = new Character[_cacheCount];
		for (int i = 0; i < _cacheCount; i++)
		{
			Character character = Object.Instantiate<Character>(_characterToSummon, _summonTransform.position, Quaternion.identity);
			((Component)character).gameObject.SetActive(false);
			_pool[i] = character;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!Service.quitting)
		{
			for (int i = 0; i < _cacheCount; i++)
			{
				Object.Destroy((Object)(object)((Component)_pool[i]).gameObject);
			}
		}
	}

	public override void Run(Character owner)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (!MMMaths.PercentChance(_spawnChance))
		{
			return;
		}
		for (int i = 0; i < _cacheCount; i++)
		{
			Character character = _pool[i];
			if (!((Component)character).gameObject.activeSelf)
			{
				((Component)character).transform.position = _summonTransform.position;
				((Component)character).gameObject.SetActive(true);
				break;
			}
		}
	}
}
