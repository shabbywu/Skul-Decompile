using System.Collections;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class MultidimensionalPrism : CharacterOperation
{
	[Header("Laser 각도 설정")]
	[SerializeField]
	[MinMaxSlider(1f, 30f)]
	private Vector2Int _laserCountRange;

	[SerializeField]
	private Transform[] _laser;

	[SerializeField]
	private LayerMask _targetLayer = LayerMask.op_Implicit(1024);

	[SerializeField]
	private CustomFloat _detectRange;

	[SerializeField]
	private Transform _center;

	[SerializeField]
	private CompositeCollider2D _collider;

	[Range(0f, 360f)]
	[SerializeField]
	private int _maxAngle;

	[Header("Laser 이펙트 설정")]
	[SerializeField]
	private EffectInfo _effectInfo;

	public override void Run(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		Character character = ((LayerMask.op_Implicit(_targetLayer) != 512) ? TargetFinder.FindClosestTarget(Vector2.op_Implicit(_center.position), _detectRange.value, _targetLayer) : Singleton<Service>.Instance.levelManager.player);
		float num = Random.Range(0, _maxAngle);
		if ((Object)(object)character != (Object)null)
		{
			Vector2 val = Vector2.op_Implicit(((Component)character).transform.position - _center.position);
			num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		}
		int num2 = Random.Range(((Vector2Int)(ref _laserCountRange)).x, ((Vector2Int)(ref _laserCountRange)).y);
		int num3 = _maxAngle / num2;
		float num4 = num;
		Transform[] laser = _laser;
		for (int i = 0; i < laser.Length; i++)
		{
			((Component)laser[i]).gameObject.SetActive(false);
		}
		for (int j = 0; j < num2; j++)
		{
			_effectInfo.Spawn(_center.position, owner, num4);
			((Component)_laser[j]).gameObject.SetActive(true);
			_laser[j].rotation = Quaternion.Euler(0f, 0f, num4);
			num4 = num + (float)((j + 1) * num3) + (float)Random.Range(-15, 15);
		}
		((MonoBehaviour)this).StartCoroutine(CGenerateCollider());
	}

	private IEnumerator CGenerateCollider()
	{
		yield return null;
		_collider.GenerateGeometry();
	}
}
