using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorial;

public abstract class NPC : MonoBehaviour
{
	protected Character _player;

	protected abstract void Activate();

	protected abstract void Deactivate();

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && !((Object)(object)component != (Object)(object)_player))
		{
			Activate();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && !((Object)(object)component != (Object)(object)_player))
		{
			Deactivate();
		}
	}
}
