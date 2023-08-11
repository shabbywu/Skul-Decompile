using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class BehindGoldenAide : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[Space]
	[SerializeField]
	[Header("Intro")]
	private Action _introOut;

	[SerializeField]
	private Transform _introSource;

	[SerializeField]
	private Transform _introDest;

	[Header("InGame")]
	[SerializeField]
	[Space]
	private Action _in;

	[SerializeField]
	private Transform _inStart;

	[SerializeField]
	private Transform _inDest;

	[SerializeField]
	private float _inDuration;

	[SerializeField]
	private Action _out;

	private void Start()
	{
	}

	public IEnumerator CIntroOut()
	{
		Show(_introSource.position);
		_introOut.TryStart();
		while (_introOut.running)
		{
			yield return null;
		}
		Hide();
	}

	public IEnumerator CIn()
	{
		Show(_inStart.position);
		_in.TryStart();
		Vector2 val = Vector2.op_Implicit(((Component)_character).transform.position);
		Vector2 val2 = Vector2.op_Implicit(_inDest.position);
		yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _in, _inDuration);
	}

	public IEnumerator COut()
	{
		_out.TryStart();
		Vector2 val = Vector2.op_Implicit(((Component)_character).transform.position);
		Vector2 val2 = Vector2.op_Implicit(_inStart.position);
		yield return MoveToDestination(Vector2.op_Implicit(val), Vector2.op_Implicit(val2), _out, _inDuration);
		Hide();
	}

	public void Hide()
	{
		((Component)_character.@base).gameObject.SetActive(false);
	}

	private void Show(Vector3 startPoint)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Component)_character).transform.position = startPoint;
		((Component)_character.@base).gameObject.SetActive(true);
	}

	private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Action action, float duration)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float elapsed = 0f;
		Character.LookingDirection direction = _character.lookingDirection;
		while (action.running)
		{
			yield return null;
			Vector2 val = Vector2.Lerp(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), elapsed / duration);
			((Component)_character).transform.position = Vector2.op_Implicit(val);
			elapsed += ((ChronometerBase)_character.chronometer.master).deltaTime;
			if (elapsed > duration)
			{
				_character.CancelAction();
				break;
			}
			Vector3 val2 = source - dest;
			if (((Vector3)(ref val2)).magnitude < 0.1f)
			{
				_character.CancelAction();
				break;
			}
		}
		((Component)_character).transform.position = dest;
		_character.lookingDirection = direction;
	}
}
