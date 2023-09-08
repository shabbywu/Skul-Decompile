using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords;

public class LightSword : MonoBehaviour
{
	[SerializeField]
	private LightSwordStuck _stuck;

	[SerializeField]
	private LightSwordProjectile _projectile;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onDraw;

	private Character _owner;

	private CoroutineReference _moveCoroutine;

	public bool active { get; private set; }

	private void Awake()
	{
		_onDraw.Initialize();
	}

	private void OnDestroy()
	{
		_stuck = null;
		_projectile = null;
	}

	public void Initialzie(Character owner)
	{
		_owner = owner;
		if (!((Object)(object)_owner == (Object)null))
		{
			_owner.health.onDiedTryCatch += Hide;
		}
	}

	public void Draw(Character owner)
	{
		((Component)_onDraw).gameObject.SetActive(true);
		_onDraw.Run(owner);
	}

	public void Fire(Character owner, Vector2 source, Vector2 destination)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		float degree = AngleBetween(source, destination);
		_moveCoroutine = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CMove(source, destination, degree));
		active = true;
	}

	private float AngleBetween(Vector2 from, Vector2 to)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = to - from;
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		if (!(num < 0f))
		{
			return num;
		}
		return num + 360f;
	}

	public Vector3 GetStuckPosition()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)_stuck).transform.position;
	}

	public void Sign()
	{
		_stuck.Sign();
	}

	public void Despawn()
	{
		active = false;
		_stuck.Despawn();
	}

	private void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}

	private IEnumerator CMove(Vector2 firePosition, Vector2 destination, float degree)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		yield return _projectile.CFire(firePosition, destination, degree);
		_stuck.OnStuck(_owner, destination, degree);
	}
}
