using System.Collections.Generic;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx;

public sealed class PlaySound : CharacterOperation
{
	[SerializeField]
	private SoundInfo _audioClipInfo;

	[SerializeField]
	private Transform _position;

	[SerializeField]
	private bool _trackChildren;

	private readonly List<ReusableAudioSource> _children = new List<ReusableAudioSource>();

	public override void Run(Character owner)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = (((Object)(object)_position == (Object)null) ? ((Component)this).transform.position : _position.position);
		ReusableAudioSource reusableAudioSource = PersistentSingleton<SoundManager>.Instance.PlaySound(_audioClipInfo, position);
		if (!((Object)(object)reusableAudioSource == (Object)null) && _trackChildren)
		{
			_children.Add(reusableAudioSource);
			reusableAudioSource.reusable.onDespawn += RemoveFromList;
		}
		void RemoveFromList()
		{
			reusableAudioSource.reusable.onDespawn -= RemoveFromList;
			_children.Remove(reusableAudioSource);
		}
	}

	public override void Stop()
	{
		if (_trackChildren)
		{
			for (int num = _children.Count - 1; num >= 0; num--)
			{
				_children[num].reusable.Despawn();
			}
		}
	}
}
