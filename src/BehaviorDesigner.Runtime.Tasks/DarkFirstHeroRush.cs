using Characters;
using Characters.Actions;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("커스텀 : 검은 초대용사 Rush 공격")]
public sealed class DarkFirstHeroRush : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private Action _action;

	[SerializeField]
	private float _lerpTime;

	[SerializeField]
	private float _signTime;

	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private Laser _laser;

	private Vector2 _source;

	private Vector2 _destination;

	private NonAllocCaster _caster;

	private Movement.Config _config;

	private float _elapsed;

	public override void OnAwake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_caster = new NonAllocCaster(1);
		_config = new Movement.Config(Movement.Config.Type.Flying);
		_config.ignoreGravity = true;
	}

	public override void OnStart()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_target).Value;
		Character value2 = ((SharedVariable<Character>)_character).Value;
		_elapsed = 0f;
		value2.movement.configs.Add(int.MaxValue, _config);
		Vector2 val = Vector2.op_Implicit(((Component)value).transform.position) - Vector2.op_Implicit(((Component)value2).transform.position);
		((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(Layers.terrainMask);
		_caster.RayCast(Vector2.op_Implicit(((Component)value2).transform.position) + Vector2.up * 1.5f, val, 30f);
		ReadonlyBoundedList<RaycastHit2D> results = _caster.results;
		if (results.Count != 0)
		{
			_source = Vector2.op_Implicit(((Component)value2).transform.position);
			RaycastHit2D val2 = results[0];
			_destination = ((RaycastHit2D)(ref val2)).point;
			_action.TryStart();
			_laser.Activate(value2, (Vector2)((value2.lookingDirection == Character.LookingDirection.Left) ? new Vector2(0f - val.x, val.y) : val));
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		_elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
		if (!(_elapsed < _signTime))
		{
			_laser.Deactivate();
			Character value = ((SharedVariable<Character>)_character).Value;
			if (_elapsed < _lerpTime + _signTime)
			{
				Vector2 val = Vector2.Lerp(_source, _destination, _elapsed / _lerpTime);
				value.movement.force = val - Vector2.op_Implicit(((Component)value).transform.position);
			}
			else
			{
				value.movement.force = _destination - Vector2.op_Implicit(((Component)value).transform.position);
			}
			if (_action.running)
			{
				return (TaskStatus)3;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}

	public override void OnEnd()
	{
		((Task)this).OnEnd();
		((SharedVariable<Character>)_character).Value.movement.configs.Remove(_config);
	}
}
