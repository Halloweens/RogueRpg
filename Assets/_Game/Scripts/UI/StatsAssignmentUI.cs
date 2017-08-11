using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsAssignmentUI : MonoBehaviour
{
    public Entity Entity { get { return entity; } set { entity = value; } }
    [SerializeField]
    private Entity entity = null;

    public Characteristics Characteristics { get { return characteristics; } set { characteristics = value; } }
    [SerializeField]
    private Characteristics characteristics = null;

    public Text remainingPointsUI = null;

    public Text strengthUI = null;
    public Text constitutionUI = null;
    public Text intelligenceUI = null;
    public Text dexterityUI = null;

    private void Awake()
    {
        if (characteristics == null)
        {
            Debug.LogError("Please verify your configuration.");
            enabled = false;
            return;
        }

        characteristics.onStatsChanged.AddListener(RebuildUi);

        RebuildUi();
    }

    private void OnDestroy()
    {
        if (characteristics != null)
            characteristics.onStatsChanged.RemoveListener(RebuildUi);
    }

    public void AddStat(int type)
    {
        if (characteristics.RemainingPoints <= 0)
            return;

        StatType typee = (StatType)type;

        switch (typee)
        {
            case StatType.Strength:
                characteristics.Strength++;
                break;
            case StatType.Intelligence:
                characteristics.Intelligence++;
                break;
            case StatType.Constitution:
                characteristics.Constitution++;
                break;
            case StatType.Dexterity:
                characteristics.Dexterity++;
                break;
            default:
                Debug.LogError("Invalid Value");
                return;
        }

        characteristics.RemainingPoints--;
    }

    public void DecreaseStat(int type)
    {
        StatType typee = (StatType)type;

        switch (typee)
        {
            case StatType.Strength:
                if (characteristics.Strength > 0)
                    characteristics.Strength--;
                else
                    return;
                break;
            case StatType.Intelligence:
                if (characteristics.Intelligence > 0)
                    characteristics.Intelligence--;
                else
                    return;
                break;
            case StatType.Constitution:
                if (characteristics.Constitution > 0)
                    characteristics.Constitution--;
                else
                    return;
                break;
            case StatType.Dexterity:
                if (characteristics.Dexterity > 0)
                    characteristics.Dexterity--;
                else
                    return;
                break;
            default:
                Debug.LogError("Invalid Value");
                return;
        }

        characteristics.RemainingPoints++;
    }

    public void OnNameChanged(string text)
    {
        if (entity != null)
            entity.entityName = text;
    }

    private void RebuildUi()
    {
        strengthUI.text = characteristics.Strength.ToString();
        constitutionUI.text = characteristics.Constitution.ToString();
        intelligenceUI.text = characteristics.Intelligence.ToString();
        dexterityUI.text = characteristics.Dexterity.ToString();

        remainingPointsUI.text = characteristics.RemainingPoints.ToString();
    }
}

[System.Serializable]
public enum StatType : int
{
    Strength = 0,
    Intelligence = 1,
    Constitution = 2,
    Dexterity = 3
}