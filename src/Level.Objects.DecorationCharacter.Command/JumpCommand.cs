using System;
using System.Collections;
using Level.Objects.DecorationCharacter.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command;

[Serializable]
public class JumpCommand : ICommand
{
	private const float PI = (float)Math.PI;

	[SerializeField]
	private DecorationCharacterMovement _movement;

	[SerializeField]
	private CustomFloat _jumpPower;

	private bool _isRight = true;

	public IEnumerator CRun()
	{
		_isRight = MMMaths.RandomBool();
		if (!_movement.isGrounded)
		{
			yield return null;
		}
		_movement.Jump(_jumpPower.value);
		yield return null;
		while (!_movement.isGrounded)
		{
			_movement.Move(_isRight ? 0f : ((float)Math.PI));
			yield return null;
		}
	}
}
