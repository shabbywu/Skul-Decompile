using Characters.Operations.Summon;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

public class SpriteLaser : MonoBehaviour
{
	[Header("Length")]
	[SerializeField]
	private float _minWidth = 2f;

	[SerializeField]
	private float _maxWidth = 40f;

	[SerializeField]
	private float _minHeight = 25f / 32f;

	[SerializeField]
	private float _maxDistance = 30f;

	[SerializeField]
	[Header("Point")]
	private Transform _firePosition;

	[SerializeField]
	private Transform _pivot;

	[SerializeField]
	private Transform _body;

	[SerializeField]
	[Header("Hit")]
	private LayerMask _terrainMask;

	[SerializeField]
	private GameObject _hitEffect;

	[SerializeField]
	private float _fireTerm = 0.5f;

	[Subcomponent(typeof(SummonOperationRunner))]
	[SerializeField]
	private SummonOperationRunner _summonOperationRunner;

	private static NonAllocCaster _laycaster;

	private float _elapsed;

	static SpriteLaser()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_laycaster = new NonAllocCaster(15);
	}

	private void Update()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		_pivot.Rotate(new Vector3(0f, 0f, 30f * Time.deltaTime));
		((ContactFilter2D)(ref _laycaster.contactFilter)).SetLayerMask(_terrainMask);
		Quaternion rotation = _pivot.rotation;
		Vector3 val = Quaternion.Euler(0f, 0f, ((Quaternion)(ref rotation)).eulerAngles.z) * Vector2.op_Implicit(Vector2.down);
		_laycaster.RayCast(Vector2.op_Implicit(_firePosition.position), Vector2.op_Implicit(val), _maxDistance);
		ReadonlyBoundedList<RaycastHit2D> results = _laycaster.results;
		if (results.Count <= 0)
		{
			_elapsed = _fireTerm;
			_body.localScale = Vector2.op_Implicit(new Vector2(1f, _maxDistance));
			_hitEffect.SetActive(false);
			return;
		}
		int num = 0;
		RaycastHit2D val2 = results[0];
		float num2 = ((RaycastHit2D)(ref val2)).distance;
		for (int i = 1; i < results.Count; i++)
		{
			val2 = results[i];
			float distance = ((RaycastHit2D)(ref val2)).distance;
			if (distance < num2)
			{
				num2 = distance;
				num = i;
			}
		}
		RaycastHit2D val3 = results[num];
		_body.localScale = Vector2.op_Implicit(new Vector2(1f, Vector2.Distance(Vector2.op_Implicit(((Component)_firePosition).transform.position), ((RaycastHit2D)(ref val3)).point)));
		_hitEffect.transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref val3)).point);
		_hitEffect.SetActive(true);
		_elapsed += Time.deltaTime;
		if (_elapsed >= _fireTerm)
		{
			_summonOperationRunner.Run(Singleton<Service>.Instance.levelManager.player);
			_elapsed -= _fireTerm;
		}
	}
}
