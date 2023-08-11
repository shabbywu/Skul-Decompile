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
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		character = owner;
		Attach();
		((CoroutineReference)(ref _coroutineReference)).Stop();
		_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, CUpdate());
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
		((CoroutineReference)(ref _coroutineReference)).Stop();
		if ((Object)(object)character != (Object)null && _originalConfig != null)
		{
			_originalConfig.gravity = _originalGravity;
		}
	}
}
