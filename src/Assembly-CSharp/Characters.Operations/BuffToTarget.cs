using FX;
using UnityEngine;

namespace Characters.Operations;

public class BuffToTarget : CharacterOperation
{
	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	private PoolObject _effect;

	[SerializeField]
	private int _offset = 1;

	[SerializeField]
	private float _duration = 1f;

	[SerializeField]
	private Character _target;

	public override void Run(Character owner)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
		if ((Object)(object)_effect != (Object)null)
		{
			PoolObject spawnedEffect = _effect.Spawn(((Component)_target).transform.position, true);
			VisualEffect.PostProcess(spawnedEffect, _target, 1f, 0f, Vector3.zero, attachToTarget: true, relativeScaleToTargetSize: true, overwrite: true);
			SpriteRenderer component = ((Component)spawnedEffect).GetComponent<SpriteRenderer>();
			SpriteRenderer mainRenderer = _target.spriteEffectStack.mainRenderer;
			((Renderer)component).sortingLayerID = ((Renderer)mainRenderer).sortingLayerID;
			((Renderer)component).sortingOrder = ((Renderer)mainRenderer).sortingOrder + _offset;
			onDetach = delegate
			{
				spawnedEffect.Despawn();
			};
		}
		_target.stat.AttachOrUpdateTimedValues(_stat, _duration, onDetach);
	}
}
