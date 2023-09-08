using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class YggdrasillSounds : MonoBehaviour
{
	[Space]
	[SerializeField]
	[Header("Intro")]
	private SoundInfo _appearance_Sign;

	[SerializeField]
	private SoundInfo _appearance_Impact;

	[SerializeField]
	private SoundInfo _appearance_Appearance;

	[SerializeField]
	private SoundInfo _appearance_Roar;

	[SerializeField]
	[Space]
	[Header("Fist Slam")]
	private SoundInfo _fistSlam_Intro1;

	[SerializeField]
	private SoundInfo _fistSlam_Intro2;

	[SerializeField]
	private SoundInfo _fistSlam_Sign;

	[SerializeField]
	private SoundInfo _fistSlam_Impact1;

	[SerializeField]
	private SoundInfo _fistSlam_Impact2;

	[SerializeField]
	private SoundInfo _fistSlam_Recovery_Sign;

	[SerializeField]
	private SoundInfo _fistSlam_Recovery;

	[SerializeField]
	private SoundInfo _fistSlam_Outro;

	[Header("Sweep")]
	[Space]
	[SerializeField]
	private SoundInfo _sweeping_Intro1;

	[SerializeField]
	private SoundInfo _sweeping_Intro2;

	[SerializeField]
	private SoundInfo _sweeping_Ready;

	[SerializeField]
	private SoundInfo _sweeping_Sweeping1;

	[SerializeField]
	private SoundInfo _sweeping_Sweeping2;

	[SerializeField]
	private SoundInfo _sweeping_Outro;

	[Header("EnergyBomb")]
	[Space]
	[SerializeField]
	private SoundInfo _laser_Intro_Impact;

	[SerializeField]
	private SoundInfo _laser_Sign;

	[SerializeField]
	private SoundInfo _energyBomb_Fire;

	[Header("Groggy")]
	[Space]
	[SerializeField]
	private SoundInfo _groggy_Intro;

	[SerializeField]
	private SoundInfo _groggy_groggy;

	[SerializeField]
	private SoundInfo _groggy_impact;

	[SerializeField]
	private SoundInfo _groggy_recovery;

	[SerializeField]
	[Space]
	[Header("Dead")]
	private SoundInfo _dead_Intro;

	[SerializeField]
	private SoundInfo _dead_DarkQuartz_Intro;

	[SerializeField]
	private SoundInfo _dead_DarkQuartz_Explosion;

	[SerializeField]
	private SoundInfo _dead_Normalize;

	public void PlaySignSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_appearance_Sign, ((Component)this).transform.position);
	}

	public void PlayImpactSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_appearance_Impact, ((Component)this).transform.position);
	}

	public void PlayAppearanceSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_appearance_Appearance, ((Component)this).transform.position);
	}

	public void PlayRoarSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_appearance_Roar, ((Component)this).transform.position);
	}

	public void PlaySlamIntroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Intro1, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Intro2, ((Component)this).transform.position);
	}

	public void PlaySlamSignSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Sign, ((Component)this).transform.position);
	}

	public void PlaySlamImpactSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Impact1, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Impact2, ((Component)this).transform.position);
	}

	public void PlaySlamRecoverySignSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Recovery_Sign, ((Component)this).transform.position);
	}

	public void PlaySlamRecoverySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Recovery, ((Component)this).transform.position);
	}

	public void PlaySlamOutroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_fistSlam_Outro, ((Component)this).transform.position);
	}

	public void PlaySweepingIntroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Intro1, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Intro2, ((Component)this).transform.position);
	}

	public void PlaySweepingReadySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Ready, ((Component)this).transform.position);
	}

	public void PlaySweepingSweepingSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Sweeping1, ((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Sweeping2, ((Component)this).transform.position);
	}

	public void PlaySweepingOutroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sweeping_Outro, ((Component)this).transform.position);
	}

	public void PlayEnergyBombImpactSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_laser_Intro_Impact, ((Component)this).transform.position);
	}

	public void PlayEnergyBombSignSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_laser_Sign, ((Component)this).transform.position);
	}

	public void PlayEnergyBombFireSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_energyBomb_Fire, ((Component)this).transform.position);
	}

	public void PlayGroggyIntroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groggy_Intro, ((Component)this).transform.position);
	}

	public void PlayGroggyGroggySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groggy_groggy, ((Component)this).transform.position);
	}

	public void PlayGroggyImpctSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groggy_impact, ((Component)this).transform.position);
	}

	public void PlayGroggyRecoverySound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_groggy_recovery, ((Component)this).transform.position);
	}

	public void PlayDeadIntroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dead_Intro, ((Component)this).transform.position);
	}

	public void PlayDeadDarkQuartzIntroSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dead_DarkQuartz_Intro, ((Component)this).transform.position);
	}

	public void PlayDeadDarkQuartzExplosionSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dead_DarkQuartz_Explosion, ((Component)this).transform.position);
	}

	public void PlayDeadNormalizeSound()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_dead_Normalize, ((Component)this).transform.position);
	}
}
