using System.Text;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx;

public class SpawnEnemyLineText : CharacterOperation
{
	private readonly string _textKeyPrefix = "enemy/line/";

	[SerializeField]
	private string _textKey;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private float _duration = 1.5f;

	[SerializeField]
	private float _coolTime = 8f;

	[SerializeField]
	private bool _force;

	[SerializeField]
	private LineText _lineText;

	private string[] _localizedStrings;

	public override void Run(Character owner)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (_force || _lineText.finished)
		{
			string[] localizedStringArray = GetLocalizedStringArray(owner);
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

	private string[] GetLocalizedStringArray(Character owner)
	{
		if (_localizedStrings == null)
		{
			StringBuilder stringBuilder = new StringBuilder(_textKeyPrefix);
			stringBuilder.Append(owner.key.ToString());
			stringBuilder.Append("/");
			stringBuilder.Append(_textKey);
			_localizedStrings = Localization.GetLocalizedStringArray(stringBuilder.ToString());
		}
		return _localizedStrings;
	}
}
