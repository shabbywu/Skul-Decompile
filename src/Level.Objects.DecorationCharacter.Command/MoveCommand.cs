using System;
using System.Collections;
using Level.Objects.DecorationCharacter.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command;

[Serializable]
public class MoveCommand : ICommand
{
	private const float PI = (float)Math.PI;

	[SerializeField]
	private DecorationCharacter _owner;

	[SerializeField]
	private DecorationCharacterMovement _movement;

	[SerializeField]
	private CustomFloat _moveRange;

	private bool _isRight = true;

	private Vector2 _destination;

	public IEnumerator CRun()
	{
		_isRight = MMMaths.RandomBool();
		Collider2D lastStandingCollider = _movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider != (Object)null)
		{
			Bounds bounds = lastStandingCollider.bounds;
			float value = _moveRange.value;
			Bounds bounds2;
			if (_isRight)
			{
				MoveCommand moveCommand = this;
				float num = ((Component)_owner).transform.position.x + value;
				float x = ((Bounds)(ref bounds)).max.x;
				bounds2 = ((Collider2D)_owner.collider).bounds;
				moveCommand._destination = new Vector2(Mathf.Min(num, x - ((Bounds)(ref bounds2)).extents.x), 0f);
			}
			else
			{
				MoveCommand moveCommand2 = this;
				float num2 = ((Component)_owner).transform.position.x - value;
				float x2 = ((Bounds)(ref bounds)).min.x;
				bounds2 = ((Collider2D)_owner.collider).bounds;
				moveCommand2._destination = new Vector2(Mathf.Max(num2, x2 + ((Bounds)(ref bounds2)).extents.x), 0f);
			}
			while (Mathf.Abs(((Component)_owner).transform.position.x - _destination.x) > 1f)
			{
				_movement.Move(_isRight ? 0f : ((float)Math.PI));
				yield return null;
			}
		}
	}
}
