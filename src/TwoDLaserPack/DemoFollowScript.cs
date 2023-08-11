using System.Collections.Generic;
using UnityEngine;

namespace TwoDLaserPack;

public class DemoFollowScript : MonoBehaviour
{
	public Transform target;

	public float speed;

	public bool shouldFollow;

	public bool isHomeAndShouldDeactivate;

	public bool movingToDeactivationTarget;

	private Vector3 newPosition;

	public List<Transform> acquiredTargets;

	private void Start()
	{
		isHomeAndShouldDeactivate = false;
		movingToDeactivationTarget = false;
		acquiredTargets = new List<Transform>();
		if ((Object)(object)target == (Object)null)
		{
			Debug.Log((object)("No target found for the FollowScript on: " + ((Object)((Component)this).gameObject).name));
		}
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (shouldFollow && (Object)(object)target != (Object)null)
		{
			newPosition = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(((Component)this).transform.position), Vector2.op_Implicit(target.position), Time.deltaTime * speed));
			((Component)this).transform.position = new Vector3(newPosition.x, newPosition.y, ((Component)this).transform.position.z);
		}
	}

	private void OnDisable()
	{
	}
}
