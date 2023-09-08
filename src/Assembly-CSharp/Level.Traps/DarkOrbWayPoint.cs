using Characters;
using Characters.Abilities;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps;

public class DarkOrbWayPoint : MonoBehaviour
{
	private enum LoopType
	{
		Loop,
		PingPong
	}

	[SerializeField]
	private LoopType _rollbackType;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private SweepAttack _sweepAttack;

	[SerializeField]
	private SweepAttack _sweepAttackToEnemy;

	[SerializeField]
	private Transform _waypointParent;

	[SerializeField]
	private float minimumDistanceFromDestination = 0.1f;

	[SerializeField]
	[AbilityAttacher.Subcomponent]
	private AbilityAttacher _abilityAttacher;

	private float _distance;

	private int _currentCount;

	private int _wayPointDelta = 1;

	private void Start()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (_waypointParent.childCount <= 0)
		{
			Debug.LogError((object)"Waypoint Transform이 자식 오브젝트 가지고 있지 않습니다.");
			return;
		}
		_abilityAttacher.Initialize(_character);
		_abilityAttacher.StartAttach();
		_sweepAttack.Initialize();
		_sweepAttack.Run(_character);
		_sweepAttackToEnemy.Initialize();
		_sweepAttackToEnemy.Run(_character);
		((Component)_character).transform.position = _waypointParent.GetChild(0).position;
	}

	private void Update()
	{
		Move();
		UpdateWayPoint();
	}

	private void Move()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)_waypointParent.GetChild(_currentCount)).transform.position - ((Component)_character).transform.position;
		_distance = ((Vector3)(ref val)).sqrMagnitude;
		_character.movement.MoveHorizontal(Vector2.op_Implicit(((Vector3)(ref val)).normalized));
	}

	private void UpdateWayPoint()
	{
		if (_distance > minimumDistanceFromDestination)
		{
			return;
		}
		switch (_rollbackType)
		{
		case LoopType.Loop:
			_currentCount = ++_currentCount % _waypointParent.childCount;
			break;
		case LoopType.PingPong:
			if (_currentCount == _waypointParent.childCount - 1)
			{
				_wayPointDelta = -1;
			}
			else if (_currentCount == 0)
			{
				_wayPointDelta = 1;
			}
			_currentCount += _wayPointDelta;
			break;
		}
	}
}
