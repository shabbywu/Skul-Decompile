using System.Collections.Generic;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations;

public class FingerOfThunderbolt : CharacterOperation
{
	[FormerlySerializedAs("_serachRange")]
	[SerializeField]
	private Collider2D _searchRange;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper;

	private RayCaster _groundFinder;

	[SerializeField]
	private Transform _thunderboltPosition;

	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(5);
		_groundFinder = new RayCaster
		{
			direction = Vector2.down,
			distance = 5f
		};
		((ContactFilter2D)(ref ((Caster)_groundFinder).contactFilter)).SetLayerMask(Layers.groundMask);
		((Behaviour)_searchRange).enabled = false;
	}

	private void OnEnable()
	{
		((Component)_thunderboltPosition).transform.parent = null;
	}

	protected override void OnDestroy()
	{
		Object.Destroy((Object)(object)((Component)_thunderboltPosition).gameObject);
	}

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_searchRange).enabled = true;
		_overlapper.OverlapCollider(_searchRange);
		List<Target> components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
		if (components.Count == 0)
		{
			((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(2048));
			_overlapper.OverlapCollider(_searchRange);
			components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
			if (components.Count == 0)
			{
				((Behaviour)_searchRange).enabled = false;
				return;
			}
		}
		((Behaviour)_searchRange).enabled = false;
		Target target = components.Random();
		((Caster)_groundFinder).origin = Vector2.op_Implicit(((Component)target).transform.position);
		RaycastHit2D val = ((Caster)_groundFinder).SingleCast();
		if (RaycastHit2D.op_Implicit(val))
		{
			_thunderboltPosition.position = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point);
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(owner));
		}
	}
}
