using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class SpawnGold : Operation
{
	[SerializeField]
	private Transform _point;

	[SerializeField]
	private int _gold;

	[SerializeField]
	private int _count;

	public override void Run()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = (((Object)(object)_point == (Object)null) ? ((Component)this).transform.position : _point.position);
		Singleton<Service>.Instance.levelManager.DropGold(_gold, _count, position);
	}
}
