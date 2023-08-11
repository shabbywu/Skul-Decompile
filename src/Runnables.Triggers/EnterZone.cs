using System.Collections;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace Runnables.Triggers;

public class EnterZone : Trigger
{
	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private LayerMask _layer;

	[Space]
	[Header("Character Type")]
	[SerializeField]
	private bool _checkCharacterType;

	[SerializeField]
	private Character.Type _characterType;

	private bool _result;

	private static readonly NonAllocOverlapper _lapper;

	static EnterZone()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_lapper = new NonAllocOverlapper(15);
	}

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CTriggerEnter());
	}

	public IEnumerator CTriggerEnter()
	{
		while (true)
		{
			yield return null;
			((ContactFilter2D)(ref _lapper.contactFilter)).SetLayerMask(_layer);
			ReadonlyBoundedList<Collider2D> results = _lapper.OverlapCollider(_collider).results;
			_result = false;
			if (results.Count == 0)
			{
				continue;
			}
			if (!_checkCharacterType)
			{
				_result = true;
				continue;
			}
			foreach (Collider2D item in results)
			{
				Characters.Target component = ((Component)item).GetComponent<Characters.Target>();
				if (!((Object)(object)component == (Object)null) && component.character.type == _characterType)
				{
					_result = true;
					break;
				}
			}
		}
	}

	protected override bool Check()
	{
		return _result;
	}
}
