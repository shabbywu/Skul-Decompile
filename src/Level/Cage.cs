using System;
using Characters;
using UnityEngine;

namespace Level;

public class Cage : MonoBehaviour
{
	[SerializeField]
	private Target _target;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private SpriteRenderer _behind;

	[SerializeField]
	private Sprite _behindWreckage;

	[SerializeField]
	private Prop _prop;

	public Collider2D collider => _collider;

	public event Action onDestroyed;

	private void Awake()
	{
		_prop.onDestroy += Destroy;
	}

	public void OverrideProp(Prop newProp, SpriteRenderer newBehind, Sprite behindWreck)
	{
		_prop.onDestroy -= Destroy;
		((Component)_prop).gameObject.SetActive(false);
		((Component)_behind).gameObject.SetActive(false);
		_prop = newProp;
		_prop.onDestroy += Destroy;
		((Component)_prop).gameObject.SetActive(true);
		_behind = newBehind;
		_behindWreckage = behindWreck;
	}

	public void Activate()
	{
		((Behaviour)collider).enabled = true;
	}

	public void Deactivate()
	{
		((Behaviour)collider).enabled = false;
	}

	public void Destroy()
	{
		((Behaviour)_collider).enabled = false;
		_behind.sprite = _behindWreckage;
		this.onDestroyed?.Invoke();
		Deactivate();
	}
}
