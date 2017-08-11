using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ManaManager : MonoBehaviour
{
    public float Mana { get { return mana; } set
        {
            mana = value;
            if (onManaChanged != null)
                onManaChanged.Invoke(new OnManaChangedArgs(mana));
        }
    }
    private float mana = 100.0f;

    public float MaxMana { get { return maxMana; } set
        {
            maxMana = value;
            if (mana > maxMana)
                Mana = maxMana;
        }
    }
    private float maxMana = 100.0f;

    public float refillRate = 10.0f;

    public OnManaChanged onManaChanged = new OnManaChanged();

    private Characteristics characteristics = null;

    private void Start()
    {
        characteristics = GetComponent<Characteristics>();
        if (characteristics != null)
            characteristics.onStatsChanged.AddListener(RecomputeStats);

        RecomputeStats();
    }

    private void Update()
    {
        if (mana < maxMana)
            Mana += refillRate * Time.deltaTime;
    }

	public void GainMana(float value)
	{
		Mana = mana + value >= maxMana ? maxMana : mana + value;
	}

    private void RecomputeStats()
    {
        if (characteristics != null)
        {
            MaxMana = 100.0f + characteristics.Intelligence * 20.0f;
            refillRate = 10.0f + characteristics.Intelligence * 10.0f;
        }
    }
}

[System.Serializable]
public class OnManaChanged : UnityEvent<OnManaChangedArgs> { }

[System.Serializable]
public class OnManaChangedArgs
{
    public float newManaValue = 0.0f;

    public OnManaChangedArgs(float v)
    {
        newManaValue = v;
    }
}