using UnityEngine;

public class GlobalTransformHolder : MonoBehaviour
{
	private Transform[] _children;

	private Vector3[] _originalPositions;

	private void Awake()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		int childCount = ((Component)this).transform.childCount;
		_children = (Transform[])(object)new Transform[childCount];
		_originalPositions = (Vector3[])(object)new Vector3[childCount];
		for (int i = 0; i < childCount; i++)
		{
			Transform child = ((Component)this).transform.GetChild(i);
			_children[i] = child;
			_originalPositions[i] = child.localPosition;
			child.parent = null;
		}
		((Component)this).transform.DetachChildren();
	}

	private void OnDestroy()
	{
		Transform[] children = _children;
		for (int i = 0; i < children.Length; i++)
		{
			Object.Destroy((Object)(object)((Component)children[i]).gameObject);
		}
	}

	public void ResetChildrenToLocal()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _originalPositions.Length; i++)
		{
			_children[i].position = ((Component)this).transform.TransformPoint(_originalPositions[i]);
		}
	}
}
