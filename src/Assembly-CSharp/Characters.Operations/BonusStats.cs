using System;
using FX;
using UnityEngine;

namespace Characters.Operations;

[Obsolete("이거 대신 AttachAbility 사용하세요")]
public class BonusStats : CharacterOperation
{
	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	private PoolObject _effect;

	[SerializeField]
	private int _offset = 1;

	[SerializeField]
	private float _duration = 1f;

	private Character _character;

	public override void Run(Character owner)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_character = owner;
		Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
		if ((Object)(object)_effect != (Object)null)
		{
			PoolObject spawnedEffect = _effect.Spawn(((Component)owner).transform.position, true);
			VisualEffect.PostProcess(spawnedEffect, owner, 1f, 0f, Vector3.zero, attachToTarget: true, relativeScaleToTargetSize: true, overwrite: true);
			SpriteRenderer component = ((Component)spawnedEffect).GetComponent<SpriteRenderer>();
			SpriteRenderer mainRenderer = owner.spriteEffectStack.mainRenderer;
			((Renderer)component).sortingLayerID = ((Renderer)mainRenderer).sortingLayerID;
			((Renderer)component).sortingOrder = ((Renderer)mainRenderer).sortingOrder + _offset;
			onDetach = delegate
			{
				spawnedEffect.Despawn();
			};
		}
		if (_duration == 0f)
		{
			if (!owner.stat.Contains(_stat))
			{
				owner.stat.AttachValues(_stat, onDetach);
			}
		}
		else
		{
			owner.stat.AttachOrUpdateTimedValues(_stat, _duration, onDetach);
		}
	}

	public override void Stop()
	{
		_character?.stat.DetachValues(_stat);
	}
}
