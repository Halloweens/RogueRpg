using UnityEngine;
using UnityEngine.Events;
using System;

public class Damageable : MonoBehaviour
{
    public OnDamageTaken onDamageTaken = new OnDamageTaken();
	public OnHealthChange onHealthChange = new OnHealthChange();
    public OnDeath onDeath = new OnDeath();

    public float Hp { get { return hp; } set { hp = Mathf.Clamp(value, 0.0f, maxHP); if (onHealthChange != null) onHealthChange.Invoke(new OnHealthChangeArgs(new Ref<float>(() => hp, x => { hp = x; }))); } }
    public float hp = 120;
    public float maxHP;

    public bool Dead { get { return dead; } }
	private bool dead = false;

	public float RegenarationValue { get { return regenarationValue; } set { regenarationValue = value; } }
	[SerializeField] private float regenarationValue = 3f;
	public float RegenarationTick { get { return regenarationTick; } set { regenarationTick = value; } }
	[SerializeField] private float regenarationTick = 1.5f;

	public float TimeSinceLastRegenaration { get { return timeSinceLastRegeneration; } set { timeSinceLastRegeneration = value; } }
	private float timeSinceLastRegeneration = 0f;

	public void TakeDamage(GameObject source, float amount, bool crit)
    {
        float realDamage = amount;

        if (onDamageTaken != null)
            onDamageTaken.Invoke(new OnDamageTakenArgs(new Ref<float>(() => realDamage, x => { realDamage = x; }), crit));

        Hp -= realDamage;
        if (hp <= 0 && !dead)
        {
            Die(source);
        }
    }

    public void Die(GameObject source)
    {
        if (onDeath != null)
            onDeath.Invoke(new OnDeathArgs(source));

		dead = true;
    }

	void Update()
	{
		timeSinceLastRegeneration += Time.deltaTime;
	}

	public void RefreshHealthStats(int constitution)
	{
		regenarationValue = 3f + constitution * 0.5f;
		regenarationTick = 1.5f - constitution / 30f > 0.3f ? 1.5f - constitution / 30f : 0.3f;
		float prevMaxHP = maxHP;
		maxHP = 120f + constitution * 1.5f;
		Hp += maxHP - prevMaxHP;
	}
}

[System.Serializable]
public class OnDamageTaken : UnityEvent<OnDamageTakenArgs> { }

[System.Serializable]
public class OnHealthChange : UnityEvent<OnHealthChangeArgs> { }

[System.Serializable]
public class OnDamageTakenArgs
{
    public Ref<float> damageAmount = null;
    public bool wasCrit;

    public OnDamageTakenArgs(Ref<float> da, bool wc)
    {
        damageAmount = da;
        wasCrit = wc;
    }
}

[System.Serializable]
public class OnHealthChangeArgs
{
	public Ref<float> value = null;

	public OnHealthChangeArgs(Ref<float> val)
	{
		value = val;
	}
}

[System.Serializable]
public class OnDeath : UnityEvent<OnDeathArgs> { }

[System.Serializable]
public class OnDeathArgs
{
    public GameObject source;

    public OnDeathArgs(GameObject src)
    {
        source = src;
    }
}

