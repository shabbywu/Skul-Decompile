using Characters.Operations;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Items.Customs;

public sealed class ChosenMagicianBadgeOrb : MonoBehaviour
{
	[SerializeField]
	private Item _item;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onFast;

	[Subcomponent(typeof(SweepAttack))]
	[SerializeField]
	private SweepAttack _normalAttack;

	[Subcomponent(typeof(SweepAttack))]
	[SerializeField]
	private SweepAttack _fastAttack;

	[SerializeField]
	private Transform _pivot;

	[SerializeField]
	private float _normalSpeed;

	[SerializeField]
	private float _fastSpeed;

	private Character _character;

	private float _elapsed;

	private float _speed;

	private SweepAttack _attack;

	private void OnEnable()
	{
		_normalAttack.Initialize();
		_fastAttack.Initialize();
		_onFast.Initialize();
		_character = _item.owner;
		_normalAttack.Run(_character);
	}

	public void Initialize(float startRadian)
	{
		_elapsed = startRadian;
		_attack = _normalAttack;
		_speed = _normalSpeed;
	}

	public void Move(float radius)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)_pivot).transform.position - ((Component)this).transform.position;
		_elapsed += _speed * ((ChronometerBase)_character.chronometer.master).deltaTime;
		Vector2 val2 = Vector2.op_Implicit(val) + new Vector2(Mathf.Cos(_elapsed), Mathf.Sin(_elapsed)) * radius;
		((Component)this).transform.position = Vector2.op_Implicit(Vector2.op_Implicit(((Component)this).transform.position) + val2);
	}

	public void ChangeToFast()
	{
		_speed = _fastSpeed;
		_attack.Stop();
		_attack = _fastAttack;
		_attack.Run(_character);
		_onFast.StopAll();
		((MonoBehaviour)this).StartCoroutine(_onFast.CRun(_character));
	}

	public void ChangeToNormal()
	{
		_speed = _normalSpeed;
		_attack.Stop();
		_attack = _normalAttack;
		_attack.Run(_character);
	}
}
