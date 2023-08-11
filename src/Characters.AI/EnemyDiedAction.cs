using System.Collections;
using Characters.Actions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.AI;

public class EnemyDiedAction : MonoBehaviour
{
	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	[FormerlySerializedAs("_deadAction")]
	private Action _dieAction;

	[SerializeField]
	private Action _diedAction;

	private bool _run;

	public Action diedAction => _dieAction;

	private void Start()
	{
		if (!((Object)(object)_aiController == (Object)null))
		{
			_aiController.character.health.onDiedTryCatch += OnDied;
		}
	}

	private void OnDied()
	{
		if (!_run)
		{
			_run = true;
			ActiveCharacterSprite();
			_aiController.StopAllCoroutinesWithBehaviour();
			if ((Object)(object)_dieAction != (Object)null)
			{
				((MonoBehaviour)this).StartCoroutine(PlayDieAction());
			}
			_aiController.character.health.onDiedTryCatch -= OnDied;
		}
	}

	private void ActiveCharacterSprite()
	{
		((Behaviour)_aiController.character.collider).enabled = false;
		((Component)_aiController.character).gameObject.SetActive(true);
		((Renderer)_spriteRenderer).enabled = true;
	}

	private IEnumerator PlayDieAction()
	{
		bool flag = _dieAction.TryStart();
		while (!flag)
		{
			yield return null;
			flag = _dieAction.TryStart();
		}
		if ((Object)(object)_diedAction != (Object)null)
		{
			((MonoBehaviour)this).StartCoroutine(PlayDiedAction());
		}
	}

	private IEnumerator PlayDiedAction()
	{
		while (_dieAction.running)
		{
			yield return null;
		}
		_diedAction.TryStart();
	}
}
