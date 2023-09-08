using System.Collections;
using UnityEngine;

namespace Level.ReactiveProps;

public class FlyGroup : MonoBehaviour
{
	private enum StartPositionType
	{
		Consistent,
		RandomInBounds
	}

	[SerializeField]
	private StartPositionType _startPositionType;

	[SerializeField]
	private Transform _startPoint;

	[SerializeField]
	private Collider2D _startBounds;

	[SerializeField]
	private Transform _group;

	private ReactiveProp[] _reactiveProps;

	private void Start()
	{
		ReactiveProp[] componentsInChildren = ((Component)_group).GetComponentsInChildren<AlwaysFly>();
		_reactiveProps = componentsInChildren;
		componentsInChildren = _reactiveProps;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).gameObject.SetActive(false);
		}
	}

	public void Activate()
	{
		switch (_startPositionType)
		{
		case StartPositionType.Consistent:
			StartCosistentPosition();
			break;
		case StartPositionType.RandomInBounds:
			StartRandomPosition();
			break;
		}
		((MonoBehaviour)this).StartCoroutine(Spawn());
	}

	private IEnumerator Spawn()
	{
		ReactiveProp[] reactiveProps = _reactiveProps;
		foreach (ReactiveProp fly in reactiveProps)
		{
			int waitForRandomAnimationLength = Random.Range(1, 3);
			for (int i = 0; i < waitForRandomAnimationLength; i++)
			{
				yield return null;
			}
			fly.ResetPosition();
			((Component)fly).gameObject.SetActive(true);
		}
	}

	private void StartRandomPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((Component)_group).transform.position = Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_startBounds.bounds));
	}

	private void StartCosistentPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((Component)_group).transform.position = _startPoint.position;
	}
}
