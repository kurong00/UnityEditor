using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class AlignEditor : EditorWindow
{
    public float alignX = 0f, alignY = 0f, alignZ = 0f;
    enum AlignType
    {
        leftAlign,
        rightAlign,
        topAlign,
        bottomAlign
    }
    [MenuItem("ToolBox/AlignTool")]
    static void Init()
    {
        var alignEditor = EditorWindow.GetWindow(typeof(AlignEditor));
        alignEditor.Show();
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择对齐方式");
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("左对齐")) AlignSelection(AlignType.leftAlign);
        if (GUILayout.Button("右对齐")) AlignSelection(AlignType.rightAlign);
        if (GUILayout.Button("顶对齐")) AlignSelection(AlignType.topAlign);
        if (GUILayout.Button("底对齐")) AlignSelection(AlignType.bottomAlign);
        GUILayout.EndHorizontal();
        alignX = EditorGUILayout.FloatField("X", alignX);
        alignY = EditorGUILayout.FloatField("Y", alignY);
        alignZ = EditorGUILayout.FloatField("Z", alignZ);
        if (GUILayout.Button("设置间距")) SetSpan();
    }

    List<GameObject> GetSelectedObject()
    {
        List<GameObject> gameObjects = new List<GameObject>(Selection.gameObjects);
        return gameObjects;
    }

    void AlignSelection(AlignType alignType)
    {
        List<GameObject> gameObjects = GetSelectedObject();
        if (gameObjects.Count <= 0) { Debug.LogWarning("没有选中物体");return; };
       /* gameObjects.Sort((x, y) =>
        { return x.transform.localPosition.x.CompareTo(y.transform.localPosition.x); });*/
        float align = 0f;
        if (alignType == AlignType.leftAlign)
        {
            align = gameObjects.FirstOrDefault().transform.localPosition.x;
            foreach (GameObject obj in gameObjects)
            {
                float selfY = obj.transform.localPosition.y;
                float selfZ = obj.transform.localPosition.z;
                obj.transform.localPosition = new Vector3(align, selfY, selfZ);
                Undo.RecordObject(obj, "SetX");
            }
        }
        if (alignType == AlignType.rightAlign)
        {
            align = gameObjects.LastOrDefault().transform.localPosition.x;
            foreach (GameObject obj in gameObjects)
            {
                float selfY = obj.transform.localPosition.y;
                float selfZ = obj.transform.localPosition.z;
                obj.transform.localPosition = new Vector3(align, selfY, selfZ);
            }
        }
        if (alignType == AlignType.topAlign)
        {
            align = gameObjects.FirstOrDefault().transform.localPosition.y;
            foreach (GameObject obj in gameObjects)
            {
                float selfX = obj.transform.localPosition.x;
                float selfZ = obj.transform.localPosition.z;
                Undo.RecordObject(obj, "SetX");
                obj.transform.localPosition = new Vector3(selfX, align, selfZ);
            }
        }
        if (alignType == AlignType.bottomAlign)
        {
            align = gameObjects.LastOrDefault().transform.localPosition.y;
            foreach (GameObject obj in gameObjects)
            {
                float selfX = obj.transform.localPosition.x;
                float selfZ = obj.transform.localPosition.z;
                obj.transform.localPosition = new Vector3(selfX, align, selfZ);
                Undo.RecordObject(obj, "SetX");
            }
        }
    }

    void SetSpan()
    {
        List<GameObject> gameObjects = GetSelectedObject();
        Vector3 headpos = gameObjects[0].transform.localPosition;
        Undo.RecordObjects(gameObjects.ToArray(), "change transform");
        for (int i = 1; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.localPosition = new Vector3(
                headpos.x + alignX * i,
                headpos.y + alignY * i,
                headpos.z + alignZ * i);
        }
    }
}
