using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnEffectOnScreen : Operation
{
	[SerializeField]
	private Vector3 _positionOffset;

	[SerializeField]
	private EffectInfo _info;

	public override void Run()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Singleton<ScreenEffectSpawner>.Instance.Spawn(_info, Vector2.op_Implicit(_positionOffset));
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
