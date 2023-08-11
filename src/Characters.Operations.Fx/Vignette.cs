using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx;

public class Vignette : CharacterOperation
{
	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _curve;

	public override void Run(Character owner)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		Singleton<VignetteSpawner>.Instance.Spawn(_startColor, _endColor, _curve);
	}
}
