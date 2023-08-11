using Characters;
using Characters.AI.Adventurer;
using Characters.Movements;
using UnityEngine;

namespace Level.Adventurer;

public class AdventurerCombatArea : MonoBehaviour
{
	[SerializeField]
	private Commander _commander;

	[SerializeField]
	private EnemyWave _enemyWave;

	[GetComponent]
	[SerializeField]
	private BoxCollider2D _startTrigger;

	[SerializeField]
	private GameObject _leftWall;

	[SerializeField]
	private GameObject _rightWall;

	private void Awake()
	{
		_enemyWave.onClear += DisableSideWall;
		_leftWall.SetActive(false);
		_rightWall.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && component.type == Character.Type.Player)
		{
			Bounds bounds = ((Collider2D)component.collider).bounds;
			float x = ((Bounds)(ref bounds)).min.x;
			bounds = ((Collider2D)_startTrigger).bounds;
			if (x < ((Bounds)(ref bounds)).min.x - 0.5f)
			{
				Movement movement = component.movement;
				movement.force += new Vector2(1f, 0f);
			}
			EnableSideWall();
			if ((Object)(object)_commander != (Object)null)
			{
				_commander.StartIntro();
			}
		}
	}

	private void EnableSideWall()
	{
		((Behaviour)_startTrigger).enabled = false;
		_leftWall.SetActive(true);
		_rightWall.SetActive(true);
	}

	public void DisableSideWall()
	{
		((Behaviour)_startTrigger).enabled = false;
		_leftWall.SetActive(false);
		_rightWall.SetActive(false);
	}
}
