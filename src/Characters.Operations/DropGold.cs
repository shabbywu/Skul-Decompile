using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class DropGold : CharacterOperation
{
	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private int _amount;

	[SerializeField]
	private int _count;

	public override void Run(Character owner)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Singleton<Service>.Instance.levelManager.DropGold(_amount, _count, _dropPosition.position);
	}
}
