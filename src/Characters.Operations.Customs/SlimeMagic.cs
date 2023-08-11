using System.Collections.Generic;
using FX;
using Level;
using Level.Objects.DecorationCharacter;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class SlimeMagic : TargetedCharacterOperation
{
	private static int _sortingOrder = int.MinValue;

	[SerializeField]
	[Range(0f, 100f)]
	private int _chance;

	[SerializeField]
	private EffectInfo _transformEffectInfo;

	[SerializeField]
	private SoundInfo _transformSoundInfo2;

	[SerializeField]
	private SoundInfo _transformSoundInfo3;

	[SerializeField]
	private DecorationCharacter[] _transformTargetPrefabs;

	private Vector2 _offeset;

	private SoundInfo[] _soundInfos;

	private void Awake()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_offeset = new Vector2(0f, 1f);
		_soundInfos = new SoundInfo[2] { _transformSoundInfo2, _transformSoundInfo3 };
	}

	public override void Run(Character owner, Character target)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (MMMaths.PercentChance(_chance))
		{
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector2 val = Vector2.op_Implicit(((Bounds)(ref bounds)).center);
			Object.Instantiate<DecorationCharacter>(ExtensionMethods.Random<DecorationCharacter>((IEnumerable<DecorationCharacter>)_transformTargetPrefabs), Vector2.op_Implicit(val), Quaternion.identity, ((Component)Map.Instance).transform).SetRenderSortingOrder(_sortingOrder++);
			_transformEffectInfo.Spawn(((Component)target).transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(ExtensionMethods.Random<SoundInfo>((IEnumerable<SoundInfo>)_soundInfos), ((Component)target).transform.position);
			target.health.Kill();
		}
	}
}
