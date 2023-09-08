using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class ThunderAttack : MonoBehaviour
{
	[SerializeField]
	private Collider2D _attackRange;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	[Subcomponent(typeof(SpawnEffect))]
	private SpawnEffect _spawnAttackSign;

	[SerializeField]
	[Subcomponent(typeof(SweepAttack2))]
	private SweepAttack2 _sweepAttack;

	[Subcomponent(typeof(SpawnEffect))]
	[SerializeField]
	private SpawnEffect _sweepAttackEffect;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _playSignSound;

	[SerializeField]
	[Subcomponent(typeof(PlaySound))]
	private PlaySound _playAttackSound;

	[SerializeField]
	private float _signDelay;

	[SerializeField]
	private float _term = 0.15f;

	[SerializeField]
	private float _distance;

	private int _count;

	private bool _initialized;

	private void Initialize(Character character)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		float x2 = ((Bounds)(ref bounds)).max.x;
		Bounds bounds2 = _attackRange.bounds;
		float x3 = ((Bounds)(ref bounds2)).size.x;
		bounds2 = _attackRange.bounds;
		float x4 = ((Bounds)(ref bounds2)).extents.x;
		for (float num = x + x4; num <= x2; num += x3 + _distance)
		{
			_count++;
		}
		_sweepAttack.Initialize();
		_initialized = true;
	}

	public IEnumerator CRun(AIController controller)
	{
		if (!_initialized)
		{
			Initialize(controller.character);
		}
		Character character = controller.character;
		yield return Chronometer.global.WaitForSeconds(_signDelay);
		Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float startX = ((character.lookingDirection == Character.LookingDirection.Left) ? ((Bounds)(ref platformBounds)).max.x : ((Bounds)(ref platformBounds)).min.x);
		Bounds bounds = _attackRange.bounds;
		float sizeX = ((Bounds)(ref bounds)).size.x;
		bounds = _attackRange.bounds;
		float extentsX = ((Bounds)(ref bounds)).extents.x;
		int sign = ((character.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1));
		for (int j = 0; j < _count; j++)
		{
			float num = startX + (sizeX * (float)j + extentsX) * (float)sign + (float)j * _distance * (float)sign;
			((Component)_attackRange).transform.position = new Vector3(num, ((Bounds)(ref platformBounds)).max.y);
			_spawnAttackSign.Run(character);
			_playSignSound.Run(character);
		}
		yield return Chronometer.global.WaitForSeconds(1f);
		for (int i = 0; i < _count; i++)
		{
			float num2 = startX + (sizeX * (float)i + extentsX) * (float)sign + (float)i * _distance * (float)sign;
			((Component)_attackRange).transform.position = new Vector3(num2, ((Bounds)(ref platformBounds)).max.y);
			Physics2D.SyncTransforms();
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(character));
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				yield return Chronometer.global.WaitForSeconds(_term);
			}
		}
	}
}
