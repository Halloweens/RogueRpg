using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Consommable))]
public class ConsommableEditor : Editor
{
    override public void OnInspectorGUI()
    {
        Consommable item = target as Consommable;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name :");
        item.ItemName = EditorGUILayout.TextField(item.ItemName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weight :");
        item.Weight = EditorGUILayout.FloatField(item.Weight);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Value :");
        item.ItemValue = (uint)EditorGUILayout.IntField((int)item.ItemValue);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description :");
        item.Description = EditorGUILayout.TextField(item.Description);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon :");
        item.Icon = (Texture2D)EditorGUILayout.ObjectField(item.Icon, typeof(Texture2D), false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Can be add in inventory : ");
        item.CanBeAddInInventory = EditorGUILayout.Toggle(item.CanBeAddInInventory);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Affectation :");
        item.EnumAffectation = (Consommable.Affectation)EditorGUILayout.EnumPopup(item.EnumAffectation);
        GUILayout.EndHorizontal();

        if (item.EnumAffectation == Consommable.Affectation.Teleportation)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("TeleportationSceneName : ");
            item.TeleportationSceneName = EditorGUILayout.TextField(item.TeleportationSceneName);
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Bonus Value : ");
            item.BonusValue = EditorGUILayout.IntField(item.BonusValue);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
        }
    }
}
