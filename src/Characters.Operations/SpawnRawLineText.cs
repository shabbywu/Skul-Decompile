using UnityEngine;

namespace Characters.Operations;

public class SpawnRawLineText : CharacterOperation
{
	[SerializeField]
	private string _text;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private float _duration = 1.5f;

	[SerializeField]
	private float _coolTime = 8f;

	[SerializeField]
	private bool _force;

	private LineText _lineText;

	public override void Run(Character owner)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_lineText == (Object)null)
		{
			_lineText = ((Component)owner).GetComponentInChildren<LineText>();
			if ((Object)(object)_lineText == (Object)null)
			{
				return;
			}
		}
		if ((_force || _lineText.finished) && _text.Length > 0)
		{
			Vector2 val = (((Object)(object)_spawnPosition == (Object)null) ? GetDefaultPosition(((Collider2D)owner.collider).bounds) : Vector2.op_Implicit(_spawnPosition.position));
			((Component)_lineText).transform.position = Vector2.op_Implicit(val);
			_lineText.Display(_text, _duration);
		}
	}

	private Vector2 GetDefaultPosition(Bounds bounds)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y + 0.5f);
	}
}
