using Characters.Minions;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Summon;

public class SummonMinion : CharacterOperation
{
	[SerializeField]
	private Minion _minion;

	[SerializeField]
	private bool _lookOwnerDirection;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private MinionSetting _overrideSetting;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private Transform[] _spawnPositions;

	[SerializeField]
	private PushInfo _pushInfo;

	[SerializeField]
	[Space]
	private bool _snapToGround;

	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	[SerializeField]
	private float _groundFindingDistance = 7f;

	public override void Run(Character owner)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (owner.playerComponents == null)
		{
			return;
		}
		if (_spawnPositions.Length == 0)
		{
			Spawn(owner, ((Component)owner).transform.position);
			return;
		}
		Transform[] spawnPositions = _spawnPositions;
		foreach (Transform val in spawnPositions)
		{
			Spawn(owner, val.position);
		}
	}

	private void Spawn(Character owner, Vector3 position)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (_snapToGround)
		{
			RaycastHit2D val = Physics2D.Raycast(Vector2.op_Implicit(position), Vector2.down, _groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val))
			{
				position = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point);
			}
		}
		Minion minion = owner.playerComponents.minionLeader.Summon(_minion, position, _overrideSetting);
		if (_lookOwnerDirection)
		{
			minion.character.ForceToLookAt(owner.lookingDirection);
		}
		if (_pushInfo != null)
		{
			minion.character.movement.push.ApplyKnockback(minion.character, _pushInfo);
		}
	}
}
