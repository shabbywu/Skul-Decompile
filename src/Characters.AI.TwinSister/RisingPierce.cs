using System.Collections;
using Characters.Actions;
using Characters.Operations.Attack;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class RisingPierce : MonoBehaviour
{
	[SerializeField]
	private Action _ready;

	[SerializeField]
	private Action _motion;

	[SerializeField]
	private Collider2D _rangePartsOrigin;

	[Subcomponent(typeof(SpawnEffect))]
	[SerializeField]
	private SpawnEffect _spawnAttackSign;

	[Subcomponent(typeof(SweepAttack2))]
	[SerializeField]
	private SweepAttack2 _sweepAttack;

	[SerializeField]
	[Subcomponent(typeof(SpawnEffect))]
	private SpawnEffect _sweepAttackEffect;

	[SerializeField]
	private float _attackDelay = 1f;

	[SerializeField]
	private CompositeCollider2D _range;

	[SerializeField]
	[MinMaxSlider(0f, 5f)]
	private Vector2 _startNoise;

	[SerializeField]
	[MinMaxSlider(0f, 5f)]
	private Vector2 _distanceNoise;

	[MinMaxSlider(6f, 8f)]
	[SerializeField]
	private Vector2Int _countRange;

	private float[] _cachedDistance;

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		int y = ((Vector2Int)(ref _countRange)).y;
		_cachedDistance = new float[y];
		_sweepAttack.Initialize();
		for (int i = 0; i < y; i++)
		{
			GameObject val = new GameObject();
			BoxCollider2D obj = val.AddComponent<BoxCollider2D>();
			Bounds bounds = _rangePartsOrigin.bounds;
			float x = ((Bounds)(ref bounds)).size.x;
			bounds = _rangePartsOrigin.bounds;
			obj.size = new Vector2(x, ((Bounds)(ref bounds)).size.y);
			((Collider2D)obj).offset = new Vector2(_rangePartsOrigin.offset.x, _rangePartsOrigin.offset.y);
			((Collider2D)obj).usedByComposite = true;
			val.transform.SetParent(((Component)_range).transform);
			val.SetActive(false);
		}
	}

	private void MakeAttackRange(float startX, float y, int count)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _rangePartsOrigin.bounds;
		float x = ((Bounds)(ref bounds)).size.x;
		bounds = _rangePartsOrigin.bounds;
		float x2 = ((Bounds)(ref bounds)).extents.x;
		for (int i = 0; i < count; i++)
		{
			Transform child = ((Component)_range).transform.GetChild(i);
			float num = startX + (x * (float)i + x2) + (float)i * _cachedDistance[i];
			((Component)child).transform.position = Vector2.op_Implicit(new Vector2(num, y));
		}
		_range.GenerateGeometry();
	}

	private float GetStartPointX(Character character)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		float num = Random.Range(_startNoise.x, _startNoise.y);
		return x + num;
	}

	public IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Bounds platformBounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float startX = GetStartPointX(character);
		Bounds bounds = _rangePartsOrigin.bounds;
		float sizeX = ((Bounds)(ref bounds)).size.x;
		bounds = _rangePartsOrigin.bounds;
		float extentsX = ((Bounds)(ref bounds)).extents.x;
		int count = Random.Range(((Vector2Int)(ref _countRange)).x, ((Vector2Int)(ref _countRange)).y);
		_ready.TryStart();
		while (_ready.running)
		{
			yield return null;
		}
		_motion.TryStart();
		for (int i = 0; i < count; i++)
		{
			float num = Random.Range(_distanceNoise.x, _distanceNoise.y);
			_cachedDistance[i] = num;
			float num2 = startX + (sizeX * (float)i + extentsX) + (float)i * num;
			((Component)_rangePartsOrigin).transform.position = new Vector3(num2, ((Bounds)(ref platformBounds)).max.y);
			_spawnAttackSign.Run(character);
		}
		MakeAttackRange(startX, ((Bounds)(ref platformBounds)).max.y, count);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)character.chronometer.master, _attackDelay);
		_sweepAttack.Run(character);
		for (int j = 0; j < count; j++)
		{
			float num3 = _cachedDistance[j];
			float num4 = startX + (sizeX * (float)j + extentsX) + (float)j * num3;
			((Component)_rangePartsOrigin).transform.position = new Vector3(num4, ((Bounds)(ref platformBounds)).max.y);
			_sweepAttackEffect.Run(character);
		}
	}
}
