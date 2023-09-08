using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI;

public class EliteSoundController : MonoBehaviour
{
	[SerializeField]
	private MusicInfo _musicInfo;

	[SerializeField]
	private AIController _elite;

	private void Awake()
	{
		if ((Object)(object)_elite == (Object)null)
		{
			_elite = ((Component)this).GetComponent<AIController>();
		}
	}

	private void Start()
	{
		_elite.onFind += delegate
		{
			PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_musicInfo);
		};
	}
}
