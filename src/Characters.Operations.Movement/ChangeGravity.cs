using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement;

public sealed class ChangeGravity : CharacterOperation
{
	[SerializeField]
	private float _gravirty;

	private Character character;

	private Characters.Movements.Movement.Config _originalConfig;

	private float _originalGravity;

	private CoroutineReference _coroutineReference;

	public override void Run(Character owner)
	{
		character = owner;
		Attach();
		_coroutineReference.Stop();
		_coroutineReference = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(CUpdate());
	}

	private IEnumerator CUpdate()
	{
		while (true)
		{
			yield return null;
			if (!_originalConfig.Equals(character.movement.config))
			{
				Attach();
			}
		}
	}

	private void Attach()
	{
		if (_originalConfig != null)
		{
			_originalConfig.gravity = _originalGravity;
		}
		_originalConfig = character.movement.config;
		_originalGravity = _originalConfig.gravity;
		_originalConfig.gravity = _gravirty;
	}

	public override void Stop()
	{
		_coroutineReference.Stop();
		if ((Object)(object)character != (Object)null && _originalConfig != null)
		{
			_originalConfig.gravity = _originalGravity;
		}
	}
}
