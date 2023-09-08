using System.Collections;
using System.Collections.Generic;
using Characters.Controllers;
using Level;
using UnityEngine;

namespace Characters;

public class CharacterInteraction : MonoBehaviour
{
	public enum InteractionType
	{
		Normal,
		Pressing
	}

	public const float pressingTimeForPressing = 1f;

	private const float _maxPressingTimeForRelease = 0.5f;

	private const float _interactInterval = 0.2f;

	[GetComponent]
	[SerializeField]
	private Character _character;

	[GetComponent]
	[SerializeField]
	private PlayerInput _input;

	private readonly List<InteractiveObject> _interactiveObjects = new List<InteractiveObject>();

	private float _lastInteractedTime;

	private float _pressingTime;

	private InteractiveObject _objectToInteract;

	public bool IsInteracting(InteractiveObject interactiveObject)
	{
		if ((Object)(object)_objectToInteract == (Object)null)
		{
			return false;
		}
		return ((object)_objectToInteract).Equals((object?)interactiveObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		InteractiveObject component = ((Component)collision).GetComponent<InteractiveObject>();
		if ((Object)(object)component != (Object)null)
		{
			_interactiveObjects.Add(component);
		}
		else
		{
			((Component)collision).GetComponent<IPickupable>()?.PickedUpBy(_character);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		InteractiveObject component = ((Component)collision).GetComponent<InteractiveObject>();
		if ((Object)(object)component != (Object)null)
		{
			_interactiveObjects.Remove(component);
		}
	}

	private void Update()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		for (int num = _interactiveObjects.Count - 1; num >= 0; num--)
		{
			InteractiveObject interactiveObject = _interactiveObjects[num];
			if ((Object)(object)interactiveObject == (Object)null || !((Behaviour)interactiveObject).isActiveAndEnabled)
			{
				_interactiveObjects.RemoveAt(num);
			}
		}
		if (_interactiveObjects.Count == 0 || PlayerInput.blocked.value)
		{
			if ((Object)(object)_objectToInteract != (Object)null)
			{
				_objectToInteract.ClosePopup();
			}
			_pressingTime = 0f;
			((MonoBehaviour)this).StopCoroutine("CPressing");
			_objectToInteract = null;
			return;
		}
		if ((Object)(object)_objectToInteract != (Object)null && !_objectToInteract.activated)
		{
			_objectToInteract = null;
		}
		float positionX = ((Component)this).transform.position.x;
		_interactiveObjects.Sort((InteractiveObject i1, InteractiveObject i2) => (i1 is Potion) ? (-1) : Mathf.Abs(positionX - ((Component)i1).transform.position.x).CompareTo(Mathf.Abs(positionX - ((Component)i2).transform.position.x)));
		for (int j = 0; j < _interactiveObjects.Count; j++)
		{
			InteractiveObject interactiveObject2 = _interactiveObjects[j];
			if (!interactiveObject2.activated || !interactiveObject2.interactable)
			{
				continue;
			}
			if (interactiveObject2.autoInteract)
			{
				interactiveObject2.InteractWith(_character);
			}
			if ((Object)(object)_objectToInteract != (Object)(object)interactiveObject2)
			{
				if ((Object)(object)_objectToInteract != (Object)null)
				{
					_pressingTime = 0f;
					((MonoBehaviour)this).StopCoroutine("CPressing");
					_objectToInteract.ClosePopup();
				}
				interactiveObject2.OpenPopupBy(_character);
				_objectToInteract = interactiveObject2;
			}
			break;
		}
	}

	public void Interact(InteractionType interactionType)
	{
		if (!((Object)(object)_objectToInteract == (Object)null) && _objectToInteract.activated && _objectToInteract.interactionType == interactionType && !(_lastInteractedTime + 0.2f > Time.realtimeSinceStartup))
		{
			_objectToInteract.InteractWith(_character);
			_lastInteractedTime = Time.realtimeSinceStartup;
		}
	}

	public void InteractionKeyWasPressed()
	{
		if (!((Object)(object)_objectToInteract == (Object)null) && _objectToInteract.activated && !(_lastInteractedTime + 0.2f > Time.realtimeSinceStartup))
		{
			if (_objectToInteract.interactionType == InteractionType.Normal)
			{
				_objectToInteract.InteractWith(_character);
				_lastInteractedTime = Time.realtimeSinceStartup;
			}
			else if (_objectToInteract.interactionType == InteractionType.Pressing)
			{
				((MonoBehaviour)this).StartCoroutine("CPressing");
			}
		}
	}

	private IEnumerator CPressing()
	{
		_pressingTime = Chronometer.global.deltaTime;
		while (_pressingTime < 1f)
		{
			yield return null;
			_pressingTime += Chronometer.global.deltaTime;
			_objectToInteract.pressingPercent = _pressingTime / 1f;
		}
		_objectToInteract.InteractWithByPressing(_character);
	}

	public void InteractionKeyWasReleased()
	{
		if (_pressingTime == 0f)
		{
			return;
		}
		((MonoBehaviour)this).StopCoroutine("CPressing");
		if (!((Object)(object)_objectToInteract == (Object)null) && _objectToInteract.activated && !(_lastInteractedTime + 0.2f > Time.realtimeSinceStartup))
		{
			_objectToInteract.pressingPercent = 0f;
			if (!(_pressingTime > 0.5f))
			{
				_objectToInteract.InteractWith(_character);
			}
		}
	}
}
