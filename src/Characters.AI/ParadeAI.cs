using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Movements;
using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class ParadeAI : AIController
{
	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	[Header("Appearance")]
	[Space]
	[Header("Behaviours")]
	private MoveToDestinationWithFly _appearance;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idleAfterAppearance;

	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[Header("Move and Attack")]
	[SerializeField]
	[Space]
	private MoveToDestinationWithFly _moveToTop;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _attackReady;

	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	private MoveToDestinationWithFly _moveToTargetGround;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _idle;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onSpawn;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onGround;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onAttack;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onJump;

	[Space]
	[SerializeField]
	[Header("Tools")]
	private float _attackHeight;

	[SerializeField]
	[MinMaxSlider(1f, 20f)]
	private Vector2 _moveAmountRange;

	private float _moveAmount;

	private float _originGravity;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _moveToTop, _moveToTargetGround };
		_onSpawn.Initialize();
		_onAttack.Initialize();
		_onGround.Initialize();
		_onJump.Initialize();
		_originGravity = character.movement.config.gravity;
		_moveAmount = Random.Range(_moveAmountRange.x, _moveAmountRange.y);
		LookToCenter();
	}

	private void LookToCenter()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Character obj = character;
		float x = ((Component)character).transform.position.x;
		Bounds bounds = Map.Instance.bounds;
		obj.lookingDirection = ((x > ((Bounds)(ref bounds)).center.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		((Component)_onSpawn).gameObject.SetActive(true);
		_onSpawn.Run(character);
		yield return CPlayStartOption();
		yield return CCombat();
	}

	private void SetJumpableState()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		character.movement.config.type = Movement.Config.Type.Flying;
		character.movement.config.gravity = 0f;
		character.movement.controller.terrainMask = LayerMask.op_Implicit(0);
		character.movement.controller.oneWayPlatformMask = LayerMask.op_Implicit(0);
	}

	private void SetAttackableState()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		character.movement.config.type = Movement.Config.Type.Walking;
		character.movement.config.gravity = _originGravity;
		character.movement.controller.terrainMask = Layers.terrainMask;
		character.movement.controller.oneWayPlatformMask = LayerMask.op_Implicit(131072);
	}

	private IEnumerator CCombat()
	{
		character.movement.onGrounded += delegate
		{
			SetAttackableState();
			((Component)_onGround).gameObject.SetActive(true);
			_onGround.Run(character);
		};
		SetJumpableState();
		SetDestination(0.5f);
		yield return CAttack();
		SetAttackableState();
		yield return _idle.CRun(this);
		while (!base.dead)
		{
			SetAttackableState();
			yield return null;
			if (CheckExitTimingAndSetDestination())
			{
				yield return CDisappear();
				continue;
			}
			yield return CAttack();
			SetAttackableState();
			yield return _idle.CRun(this);
		}
	}

	private bool CheckExitTimingAndSetDestination()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		int num = ((character.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1));
		float num2 = ((Component)character).transform.position.x + (float)num * _moveAmount;
		Bounds bounds = Map.Instance.bounds;
		float num3 = ((Component)character).transform.position.y + _attackHeight;
		base.destination = new Vector2(num2, num3);
		if (num2 >= ((Bounds)(ref bounds)).max.x || num2 <= ((Bounds)(ref bounds)).min.x)
		{
			return true;
		}
		return false;
	}

	private void SetDestination(float moveAmount)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		int num = ((character.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1));
		float num2 = ((Component)character).transform.position.x + (float)num * moveAmount;
		float num3 = ((Component)character).transform.position.y + _attackHeight;
		base.destination = new Vector2(num2, num3);
	}

	private IEnumerator CAttack()
	{
		SetJumpableState();
		((Component)_onJump).gameObject.SetActive(true);
		_onJump.Run(character);
		yield return _moveToTop.CRun(this);
		yield return _attackReady.CRun(this);
		RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(((Component)this).transform.position), Vector2.down, _attackHeight * 2f, LayerMask.op_Implicit(Layers.terrainMask));
		if (!RaycastHit2D.op_Implicit(val))
		{
			Debug.LogError((object)"Parade's y position was wrong");
			yield break;
		}
		float x = ((Component)this).transform.position.x;
		float y = ((RaycastHit2D)(ref val)).point.y;
		base.destination = new Vector2(x, y);
		((Component)_onAttack).gameObject.SetActive(true);
		_onAttack.Run(character);
	}

	private IEnumerator CAppear()
	{
		int num = ((character.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1));
		float num2 = ((Component)character).transform.position.x + (float)num;
		base.destination = new Vector2(num2, ((Component)character).transform.position.y);
		yield return _appearance.CRun(this);
		yield return _idleAfterAppearance.CRun(this);
	}

	private IEnumerator CDisappear()
	{
		character.health.Kill();
		yield break;
	}
}
