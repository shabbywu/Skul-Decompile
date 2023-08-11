using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

public class Health : MonoBehaviour
{
	public enum HealthGiverType
	{
		None,
		Potion,
		AdventurerPotion,
		System
	}

	public struct HealInfo
	{
		public HealthGiverType healthGiver;

		public double amount;

		public bool notify;

		public HealInfo(HealthGiverType healthGiverType, double amount, bool notify = true)
		{
			healthGiver = healthGiverType;
			this.amount = amount;
			this.notify = notify;
		}
	}

	public delegate void HealedDelegate(double healed, double overHealed);

	public delegate void HealByGiver(ref HealInfo healInfo);

	private readonly TakeDamageEvent _onTakeDamage = new TakeDamageEvent();

	public bool immuneToCritical;

	private Collider2D _collider;

	public PriorityList<TakeDamageDelegate> onTakeDamage => _onTakeDamage;

	public Character owner { get; set; }

	public Shield shield { get; private set; } = new Shield();


	public GrayHealth grayHealth { get; private set; }

	public double currentHealth { get; private set; }

	public double maximumHealth { get; private set; }

	public double percent { get; private set; }

	public float displayPercent { get; private set; }

	public bool dead { get; private set; }

	public double lastTakenDamage { get; private set; }

	public event TookDamageDelegate onTookDamage;

	public event Action onDie;

	public event Action onDied;

	public event Action onDiedTryCatch;

	public event Action onChanged;

	public event Action onConsumeShield;

	public event HealedDelegate onHealed;

	public event HealByGiver onHealByGiver;

	private void Awake()
	{
		_collider = ((Component)this).GetComponentInChildren<Collider2D>();
		grayHealth = new GrayHealth(this);
	}

	public void SetHealth(double current, double maximum)
	{
		currentHealth = ((current < maximum) ? current : maximum);
		maximumHealth = maximum;
		UpdateHealth();
	}

	public void SetCurrentHealth(double health)
	{
		currentHealth = health;
		UpdateHealth();
	}

	public void SetMaximumHealth(double health)
	{
		maximumHealth = health;
		if (currentHealth > maximumHealth)
		{
			currentHealth = health;
		}
		UpdateHealth();
	}

	public bool TakeDamage(ref Damage damage)
	{
		double dealtDamage;
		return TakeDamage(ref damage, out dealtDamage);
	}

	public bool TakeDamage(ref Damage damage, out double dealtDamage)
	{
		damage.Evaluate(immuneToCritical);
		Damage originalDamage = damage;
		if (_onTakeDamage.Invoke(ref damage))
		{
			damage.@base = 0.0;
			dealtDamage = 0.0;
			return true;
		}
		if (damage.@null)
		{
			damage.@base = 0.0;
			dealtDamage = 0.0;
		}
		double amount = damage.amount;
		double num = shield.Consume(amount);
		if (amount != num)
		{
			this.onConsumeShield?.Invoke();
		}
		double num2 = amount - num;
		double num3 = TakeHealth(num);
		this.onTookDamage?.Invoke(in originalDamage, in damage, num3 + num2);
		dealtDamage = num3 + num2;
		return false;
	}

	public double TakeHealth(double amount)
	{
		if (dead)
		{
			return 0.0;
		}
		currentHealth -= amount;
		lastTakenDamage = amount;
		if (currentHealth <= 0.0)
		{
			double num = currentHealth;
			currentHealth = 0.0;
			try
			{
				this.onDie?.Invoke();
				if (currentHealth <= 0.0)
				{
					dead = true;
					this.onDied?.Invoke();
					this.onDiedTryCatch?.Invoke();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("Eror while running onDie or onDied of " + ((Object)this).name + " : " + ex.Message));
				currentHealth = 0.0;
				dead = true;
				this.onDiedTryCatch?.Invoke();
			}
			UpdateHealth();
			return amount + num;
		}
		UpdateHealth();
		return amount;
	}

	public double PercentHeal(float percent)
	{
		return Heal(maximumHealth * (double)percent);
	}

	public double FixedAmountHeal(double amount, bool notify = true)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		currentHealth += amount;
		if (currentHealth > maximumHealth)
		{
			num = currentHealth - maximumHealth;
			amount -= num;
			currentHealth = maximumHealth;
		}
		UpdateHealth();
		if (notify)
		{
			if ((Object)(object)_collider == (Object)null)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, ((Component)this).transform.position);
			}
			else
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_collider.bounds)));
			}
			this.onHealed?.Invoke(amount, num);
		}
		return num;
	}

	public double Heal(HealInfo info)
	{
		this.onHealByGiver?.Invoke(ref info);
		return Heal(ref info.amount, info.notify);
	}

	public double Heal(double amount, bool notify = true)
	{
		return Heal(ref amount, notify);
	}

	public double Heal(ref double amount, bool notify = true)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		amount *= owner.stat.GetFinal(Stat.Kind.TakingHealAmount);
		double num = 0.0;
		currentHealth += amount;
		if (currentHealth > maximumHealth)
		{
			num = currentHealth - maximumHealth;
			amount -= num;
			currentHealth = maximumHealth;
		}
		UpdateHealth();
		if (notify)
		{
			if ((Object)(object)_collider == (Object)null)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, ((Component)this).transform.position);
			}
			else
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_collider.bounds)));
			}
			this.onHealed?.Invoke(amount, num);
		}
		return num;
	}

	public void GrayHeal()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		double num = grayHealth.canHeal;
		double num2 = 0.0;
		currentHealth += num;
		if (currentHealth > maximumHealth)
		{
			num2 = currentHealth - maximumHealth;
			num -= num2;
			currentHealth = maximumHealth;
		}
		grayHealth.maximum = 0.0;
		UpdateHealth();
		if ((Object)(object)_collider == (Object)null)
		{
			Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(num, ((Component)this).transform.position);
		}
		else
		{
			Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(num, Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(_collider.bounds)));
		}
		this.onHealed?.Invoke(num, num2);
	}

	public void ResetToMaximumHealth()
	{
		SetCurrentHealth(maximumHealth);
	}

	public void Revive()
	{
		Revive(maximumHealth);
	}

	public void Revive(double health)
	{
		dead = false;
		SetCurrentHealth(health);
	}

	public void TryKill()
	{
		currentHealth = 0.0;
		try
		{
			this.onDie?.Invoke();
			if (currentHealth <= 0.0)
			{
				dead = true;
				this.onDied?.Invoke();
				this.onDiedTryCatch?.Invoke();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError((object)("Eror while running onDie or onDied of " + ((Object)this).name + " : " + ex.Message));
			currentHealth = 0.0;
			dead = true;
			this.onDiedTryCatch?.Invoke();
		}
		UpdateHealth();
	}

	public void Kill()
	{
		if (dead)
		{
			return;
		}
		currentHealth = 0.0;
		dead = true;
		UpdateHealth();
		try
		{
			this.onDie?.Invoke();
			this.onDied?.Invoke();
			this.onDiedTryCatch?.Invoke();
		}
		catch (Exception ex)
		{
			Debug.LogError((object)("Eror while running onDie or onDied of " + ((Object)this).name + " : " + ex.Message));
			this.onDiedTryCatch?.Invoke();
		}
	}

	private void UpdateHealth()
	{
		percent = currentHealth / maximumHealth;
		displayPercent = Mathf.Round((float)currentHealth) / (float)Mathf.RoundToInt((float)maximumHealth);
		this.onChanged?.Invoke();
	}

	private void OnDestroy()
	{
		this.onTookDamage = null;
		this.onDie = null;
		this.onDied = null;
		this.onDiedTryCatch = null;
		this.onChanged = null;
		this.onConsumeShield = null;
		this.onHealed = null;
		((PriorityList<TakeDamageDelegate>)_onTakeDamage).Clear();
	}
}
