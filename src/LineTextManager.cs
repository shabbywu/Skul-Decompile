using System.Collections.Generic;
using UnityEngine;

public class LineTextManager : MonoBehaviour
{
	[SerializeField]
	private LineText _floatingTextPrefab;

	private Queue<LineText> _lineTexts = new Queue<LineText>(20);

	private const int _maxFloats = 20;

	private HashSet<GameObject> _locked;

	public HashSet<GameObject> locked => _locked;

	private void Awake()
	{
		_locked = new HashSet<GameObject>();
	}

	public LineText Spawn(string text, Vector3 position, float duration)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		LineText lineText;
		if ((_lineTexts.Count > 0 && _lineTexts.Peek().finished) || _lineTexts.Count > 20)
		{
			lineText = _lineTexts.Dequeue();
			while ((Object)(object)lineText == (Object)null && _lineTexts.Count > 0)
			{
				lineText = _lineTexts.Dequeue();
			}
			if ((Object)(object)lineText == (Object)null)
			{
				lineText = Object.Instantiate<LineText>(_floatingTextPrefab);
			}
		}
		else
		{
			lineText = Object.Instantiate<LineText>(_floatingTextPrefab);
		}
		((Component)lineText).transform.position = position;
		lineText.Display(text, duration);
		_lineTexts.Enqueue(lineText);
		return lineText;
	}
}
