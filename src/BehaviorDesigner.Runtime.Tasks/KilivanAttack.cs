using Characters;
using Characters.AI.Hero;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("커스텀 : 검은 초대용사 킬리반 공격")]
public sealed class KilivanAttack : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private KilivanFinish _kilivanFinish;

	[SerializeField]
	private Action _throw;

	private Vector2 _destination;

	public override void OnStart()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_target).Value;
		Character value2 = ((SharedVariable<Character>)_character).Value;
		value.movement.TryBelowRayCast(LayerMask.op_Implicit(262144), out var point, 100f);
		Vector2 direction = ((RaycastHit2D)(ref point)).point - Vector2.op_Implicit(((Component)value2).transform.position);
		_destination = _kilivanFinish.Fire(direction);
	}

	public override TaskStatus OnUpdate()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (!_throw.running)
		{
			_throw.TryStart();
		}
		if (_kilivanFinish.UpdateMove(((ChronometerBase)Chronometer.global).deltaTime, _destination))
		{
			return (TaskStatus)3;
		}
		return (TaskStatus)2;
	}
}
