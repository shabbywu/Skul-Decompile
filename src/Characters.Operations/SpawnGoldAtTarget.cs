using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class SpawnGoldAtTarget : TargetedCharacterOperation
{
	[SerializeField]
	[Range(0f, 100f)]
	private int _possibility;

	[SerializeField]
	private int _gold;

	[SerializeField]
	private int _count;

	[SerializeField]
	private bool _spawnAtOwner;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter;

	public override void Run(Character owner, Character target)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (((EnumArray<Character.Type, bool>)_characterTypeFilter)[target.type] && MMMaths.PercentChance(_possibility))
		{
			Vector3 position = (_spawnAtOwner ? ((Component)owner).transform.position : ((Component)target).transform.position);
			Singleton<Service>.Instance.levelManager.DropGold(_gold, _count, position);
		}
	}
}
