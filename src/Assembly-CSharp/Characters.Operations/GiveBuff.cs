using System.Collections.Generic;
using Level;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations;

public class GiveBuff : CharacterOperation
{
	[UnityEditor.Subcomponent(typeof(AttachAbility))]
	[SerializeField]
	private AttachAbility _attachAbility;

	[SerializeField]
	private EnemyWaveContainer _enemyWaveContainer;

	private List<Character> _buffTargets;

	private static readonly NonAllocOverlapper _enemyOverlapper;

	private const int _targetCount = 1;

	static GiveBuff()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Start()
	{
		if ((Object)(object)_enemyWaveContainer == (Object)null)
		{
			_enemyWaveContainer = ((Component)this).GetComponentInParent<EnemyWaveContainer>();
		}
	}

	private List<Character> FindRandomEnemy(Character except)
	{
		List<Character> allSpawnedEnemies = _enemyWaveContainer.GetAllSpawnedEnemies();
		List<Character> list = new List<Character>();
		foreach (Character item in allSpawnedEnemies)
		{
			if ((Object)(object)item != (Object)(object)except)
			{
				list.Add(item);
			}
		}
		if (list.Count <= 0)
		{
			return list;
		}
		int count = Mathf.Min(list.Count, 1);
		list.PseudoShuffle();
		return list.GetRange(0, count);
	}

	public override void Run(Character owner)
	{
		_buffTargets = FindRandomEnemy(owner);
		foreach (Character buffTarget in _buffTargets)
		{
			_attachAbility.Run(buffTarget);
		}
	}

	public override void Stop()
	{
		if (!((Object)(object)_attachAbility == (Object)null))
		{
			base.Stop();
			_attachAbility.Stop();
		}
	}
}
