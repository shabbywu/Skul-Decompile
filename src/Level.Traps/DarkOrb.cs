using Characters;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps;

public class DarkOrb : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private SweepAttack _sweepAttack;

	[SerializeField]
	private Transform _pivot;

	[SerializeField]
	private float _rotateSpeed = 1f;

	[SerializeField]
	private float _radius;

	private float _rotationTime;

	private void Start()
	{
		_sweepAttack.Initialize();
		_sweepAttack.Run(_character);
	}

	private void Update()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)_pivot).transform.position - ((Component)_character).transform.position;
		_rotationTime += _rotateSpeed * ((ChronometerBase)_character.chronometer.master).deltaTime;
		_character.movement.MoveHorizontal(Vector2.op_Implicit(val) + new Vector2(Mathf.Cos(_rotationTime), Mathf.Sin(_rotationTime)) * _radius);
	}
}
