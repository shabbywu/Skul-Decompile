using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnLineText : CharacterOperation
{
	[SerializeField]
	private string _textKey;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private float _duration = 2f;

	[SerializeField]
	private float _coolTime = 8f;

	[SerializeField]
	private bool _force;

	private LineText _lineText;

	private Character _owner;

	public override void Run(Character owner)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_lineText == (Object)null)
		{
			_lineText = ((Component)owner).GetComponentInChildren<LineText>();
			if ((Object)(object)_lineText == (Object)null)
			{
				return;
			}
		}
		if (_force || _lineText.finished)
		{
			string[] localizedStringArray = Localization.GetLocalizedStringArray(_textKey);
			if (localizedStringArray.Length != 0)
			{
				string text = localizedStringArray.Random();
				Vector2 val = (((Object)(object)_spawnPosition == (Object)null) ? GetDefaultPosition(((Collider2D)owner.collider).bounds) : Vector2.op_Implicit(_spawnPosition.position));
				((Component)_lineText).transform.position = Vector2.op_Implicit(val);
				_lineText.Display(text, _duration);
			}
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
