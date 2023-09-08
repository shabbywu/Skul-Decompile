using System.Collections.Generic;
using FX;
using UnityEngine;

namespace Characters.Operations;

public class BuffToTargets : CharacterOperation
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
	private List<Character> _targets;

	private void Awake()
	{
		foreach (Character target in _targets)
		{
			target.health.onDied += delegate
			{
				_targets.Remove(target);
			};
		}
	}

	public override void Run(Character owner)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
		Character character = SelectTarget();
		if ((Object)(object)_effect != (Object)null)
		{
			PoolObject spawnedEffect = _effect.Spawn(((Component)character).transform.position, true);
			VisualEffect.PostProcess(spawnedEffect, character, 1f, 0f, Vector3.zero, attachToTarget: true, relativeScaleToTargetSize: true, overwrite: true);
			SpriteRenderer component = ((Component)spawnedEffect).GetComponent<SpriteRenderer>();
			SpriteRenderer mainRenderer = character.spriteEffectStack.mainRenderer;
			((Renderer)component).sortingLayerID = ((Renderer)mainRenderer).sortingLayerID;
			((Renderer)component).sortingOrder = ((Renderer)mainRenderer).sortingOrder + _offset;
			onDetach = delegate
			{
				spawnedEffect.Despawn();
			};
		}
		character.stat.AttachOrUpdateTimedValues(_stat, _duration, onDetach);
	}

	private Character SelectTarget()
	{
		return _targets.Random();
	}
}
