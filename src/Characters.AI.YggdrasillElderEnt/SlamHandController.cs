using System.Collections;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class SlamHandController : MonoBehaviour
{
	[SerializeField]
	private float _targetingDelay = 0.64f;

	[SerializeField]
	private Health _owenrHealth;

	[SerializeField]
	private SlamHand _left;

	[SerializeField]
	private SlamHand _right;

	private SlamHand _current;

	private void Awake()
	{
		_owenrHealth.onDie += DisableHands;
	}

	public void Ready()
	{
		_left.ActiavteHand();
		_right.ActiavteHand();
		((MonoBehaviour)this).StartCoroutine(CsetDestination());
	}

	public void Slam()
	{
		((MonoBehaviour)this).StartCoroutine(_current.CSlam());
	}

	public void Recover()
	{
		((MonoBehaviour)this).StartCoroutine(_current.CRecover());
	}

	public void Vibrate()
	{
		((MonoBehaviour)this).StartCoroutine(_current.CVibrate());
	}

	public void DisableHands()
	{
		_left.DeactivateHand();
		_right.DeactivateHand();
	}

	private void SetHand()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		float x = ((Component)Singleton<Service>.Instance.levelManager.player).transform.position.x;
		bool flag = Mathf.Abs(x - ((Component)_left).transform.position.x) < Mathf.Abs(x - ((Component)_right).transform.position.x);
		_current = (flag ? _left : _right);
	}

	private IEnumerator CsetDestination()
	{
		yield return Chronometer.global.WaitForSeconds(_targetingDelay);
		SetHand();
		SetDestination();
	}

	private void SetDestination()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)Singleton<Service>.Instance.levelManager.player).transform;
		SlamHand current = _current;
		float x = transform.position.x;
		Bounds bounds = Map.Instance.bounds;
		current.destination = new Vector3(x, ((Bounds)(ref bounds)).max.y);
	}
}
