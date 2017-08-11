using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;

public class Characteristics : MonoBehaviour
{
    public UnityEvent onStatsChanged = new UnityEvent();

    public int Strength { get { return strength; } set { strength = value; FireStatsChanged(); } }
    [SerializeField] private int strength = 0;

    public int Constitution { get{ return constitution; } set { constitution = value; FireStatsChanged(); } }
    [SerializeField] private int constitution = 0;

    public int Intelligence { get { return intelligence; } set { intelligence = value;  FireStatsChanged(); } }
    [SerializeField] private int intelligence = 0;

    public int Dexterity { get { return dexterity; } set { dexterity = value;  FireStatsChanged(); } }
    [SerializeField] private int dexterity = 0;

    public int RemainingPoints { get { return remainingPoints; } set { remainingPoints = value; FireStatsChanged(); } }
    [SerializeField]
    private int remainingPoints = 0;

    public int Experience { get { return experience; }
        set
        {
            experience = value;
            if (experience >= TargetExp)
                ApplyLevelUp();

            FireStatsChanged();
        }
    }
    private int experience = 0;

    public int TargetExp { get { return targetExp; } set { targetExp = value; FireStatsChanged(); } }
    private int targetExp = 100;

    public int Level { get { return level; } set { level = value; FireStatsChanged(); }  }
    private int level = 1;

    private void FireStatsChanged()
    {
        if (onStatsChanged != null)
            onStatsChanged.Invoke();
    }

    private void ApplyLevelUp()
    {
        while (experience >= targetExp)
        {
            experience = experience - targetExp;
            targetExp = level * 100;
            level++;
            remainingPoints += 5;
        }
    }

	public void ForceLevelUpToValue(int newLevel)
	{
		if (newLevel <= level)
			return;

		for (; level < newLevel; ++level)
		{
			experience = experience - targetExp;
			targetExp = level * 100;
			level++;
			remainingPoints += 5;
		}

		FireStatsChanged();
	}

	public void SetPointsOnRandomStat()
	{
		for (int idx = 0; idx < remainingPoints; ++idx)
		{
			int stat = UnityEngine.Random.Range(0, 3);
			switch (stat)
			{
				case 0:
					++strength; break;
				case 1:
					++constitution; break;
				case 2:
					++intelligence; break;
				case 3:
					++dexterity; break;
				default:
					break;
			}
		}
	}
}
