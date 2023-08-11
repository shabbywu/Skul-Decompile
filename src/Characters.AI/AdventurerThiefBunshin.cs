using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class AdventurerThiefBunshin : AIController
{
	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	[Space]
	[Header("Flashcut")]
	private ActionAttack _flashCut;

	[SerializeField]
	[Subcomponent(typeof(TeleportBehind))]
	private TeleportBehind _teleportBehind;

	[SerializeField]
	[Space]
	[Subcomponent(typeof(Jump))]
	[Header("Shuriken")]
	private Jump _surikenJump;

	[Header("Despawn Action")]
	[SerializeField]
	private Action _despawnAction;

	protected override void OnEnable()
	{
		Run();
	}

	protected override void OnDisable()
	{
		Hide();
	}

	public void Run()
	{
		Show();
		character.animationController.ForceUpdate();
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	private void Show()
	{
		((Component)character).gameObject.SetActive(true);
	}

	private void Hide()
	{
		((Component)character).gameObject.SetActive(false);
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		character.ForceToLookAt(((Component)base.target).transform.position.x);
		yield return Chronometer.global.WaitForSeconds(1f);
		if (MMMaths.RandomBool())
		{
			yield return CastFlashCut();
		}
		else
		{
			yield return CastSuriken();
		}
		character.animationController.ForceUpdate();
		_despawnAction.TryStart();
		while (_despawnAction.running)
		{
			yield return null;
		}
		((Component)this).gameObject.SetActive(false);
	}

	private IEnumerator CastFlashCut()
	{
		yield return _teleportBehind.CRun(this);
		if (((Component)character).transform.position.x > ((Component)base.target).transform.position.x)
		{
			character.lookingDirection = Character.LookingDirection.Left;
		}
		else
		{
			character.lookingDirection = Character.LookingDirection.Right;
		}
		yield return _flashCut.CRun(this);
	}

	private IEnumerator CastSuriken()
	{
		if (((Component)character).transform.position.x > ((Component)base.target).transform.position.x)
		{
			character.lookingDirection = Character.LookingDirection.Left;
		}
		else
		{
			character.lookingDirection = Character.LookingDirection.Right;
		}
		yield return _surikenJump.CRun(this);
	}
}
