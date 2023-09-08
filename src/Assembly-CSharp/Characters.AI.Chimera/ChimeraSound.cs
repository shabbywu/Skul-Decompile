using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.Chimera;

public class ChimeraSound : MonoBehaviour
{
	[Header("Intro")]
	[Space]
	[SerializeField]
	private SoundInfo _impactSound;

	[Header("Stomp")]
	[Space]
	[SerializeField]
	private SoundInfo _fistSlamSound;

	[SerializeField]
	private SoundInfo _fistSlamReadySound;

	[SerializeField]
	[Space]
	[Header("Bite")]
	private SoundInfo _sweepingReadySound;

	[SerializeField]
	private SoundInfo _sweepingSound;

	[Space]
	[SerializeField]
	[Header("VenomBall")]
	private SoundInfo _energyBombReadySound;

	[SerializeField]
	private SoundInfo _energyBombSound;

	[Space]
	[SerializeField]
	[Header("VenomCannon")]
	private SoundInfo _groggySound;

	[SerializeField]
	private SoundInfo _groundImpactSound;

	[Header("VenomBreath")]
	[Space]
	[SerializeField]
	private SoundInfo _venomBreathSound;

	[Space]
	[Header("Roar")]
	[SerializeField]
	private SoundInfo _roarSound;

	[Space]
	[SerializeField]
	[Header("In")]
	private SoundInfo _inSound;

	[SerializeField]
	[Header("Out")]
	[Space]
	private SoundInfo _outSound;

	[SerializeField]
	[Header("BigStomp")]
	[Space]
	private SoundInfo _bicStompSound;

	[Header("Outro")]
	[Space]
	[SerializeField]
	private SoundInfo _dieSound;

	[SerializeField]
	private SoundInfo _dieShoutSound;

	public void PlayRoarSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_roarSound, ((Component)this).transform.position);
	}

	public void PlayImpactSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_impactSound, ((Component)this).transform.position);
	}

	public void PlaySlamReadySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlamReadySound, ((Component)this).transform.position);
	}

	public void PlaySlamSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlamSound, ((Component)this).transform.position);
	}

	public void PlaySweepReadySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweepingReadySound, ((Component)this).transform.position);
	}

	public void PlaySweepAttackSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweepingSound, ((Component)this).transform.position);
	}

	public void PlayEnergyBombSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_energyBombSound, ((Component)this).transform.position);
	}

	public void PlayEnergyBombReadySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_energyBombReadySound, ((Component)this).transform.position);
	}

	public void PlayGroggyStartSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groggySound, ((Component)this).transform.position);
	}

	public void PlayGroggyOnGroundSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groundImpactSound, ((Component)this).transform.position);
	}

	public void PlayDieSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dieSound, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dieShoutSound, ((Component)this).transform.position);
	}
}
