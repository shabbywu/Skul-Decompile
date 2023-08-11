using System.Collections.Generic;
using UnityEngine;

public class ShufflePositions : MonoBehaviour
{
	[SerializeField]
	private bool _shuffleOnAwake = true;

	private List<Transform> _childs = new List<Transform>();

	private List<Vector3> _positions = new List<Vector3>();

	private void Awake()
	{
		Initialize();
		if (_shuffleOnAwake)
		{
			Shuffle();
		}
	}

	private void Initialize()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		_childs.Clear();
		_positions.Clear();
		for (int i = 0; i < ((Component)this).transform.childCount; i++)
		{
			Transform child = ((Component)this).transform.GetChild(i);
			_childs.Add(child);
			_positions.Add(child.position);
		}
	}

	public void Shuffle()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		ExtensionMethods.Shuffle<Vector3>((IList<Vector3>)_positions);
		for (int i = 0; i < _childs.Count; i++)
		{
			((Component)_childs[i]).transform.position = _positions[i];
		}
	}
}
