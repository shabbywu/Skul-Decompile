using CutScenes.SpecialMap;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Gear.Quintessences.Effects;

[RequireComponent(typeof(FollowMovement))]
public sealed class AbyssShadow : MonoBehaviour
{
	[SerializeField]
	private UnityEvent _onBomb;

	[GetComponent]
	[SerializeField]
	private FollowMovement _followMovement;

	private AbyssShadowPool _pool;

	public void Initialize(AbyssShadowPool pool)
	{
		_pool = pool;
		_followMovement = ((Component)this).GetComponent<FollowMovement>();
		Singleton<Service>.Instance.levelManager.onMapLoaded += Despawn;
	}

	public void Spawn(Vector2 position)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.SetParent((Transform)null);
		((Component)this).transform.position = Vector2.op_Implicit(position);
		((Component)this).gameObject.SetActive(true);
		_followMovement.Run();
	}

	public void Despawn()
	{
		((Component)this).gameObject.SetActive(false);
		_pool.Push(this);
	}

	public void Bomb()
	{
		UnityEvent onBomb = _onBomb;
		if (onBomb != null)
		{
			onBomb.Invoke();
		}
	}

	private void OnDestroy()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Despawn;
	}
}
