using Characters;
using UnityEngine;

namespace Level;

public class Portal : MonoBehaviour
{
	private enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	private const float _maxRemainTime = 1f;

	[SerializeField]
	private Transform _targetTransform;

	[SerializeField]
	private Direction _direction;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = ((Component)collision).gameObject.GetComponent<Character>();
		if (!((Object)(object)component == (Object)null))
		{
			Teleport(component);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Character component = ((Component)collision).gameObject.GetComponent<Character>();
		if (!((Object)(object)component == (Object)null))
		{
			Teleport(component);
		}
	}

	private void Teleport(Character character)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		switch (_direction)
		{
		case Direction.Up:
			if (character.movement.velocity.y >= 0f)
			{
				((Component)character).transform.position = new Vector3(((Component)character).transform.position.x, _targetTransform.position.y);
			}
			break;
		case Direction.Down:
			if (character.movement.velocity.y <= 0f)
			{
				((Component)character).transform.position = new Vector3(((Component)character).transform.position.x, _targetTransform.position.y);
			}
			break;
		case Direction.Left:
			if (character.movement.velocity.x <= 0f)
			{
				((Component)character).transform.position = new Vector3(_targetTransform.position.x, ((Component)character).transform.position.y);
			}
			break;
		case Direction.Right:
			if (character.movement.velocity.x >= 0f)
			{
				((Component)character).transform.position = new Vector3(_targetTransform.position.x, ((Component)character).transform.position.y);
			}
			break;
		}
	}
}
