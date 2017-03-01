﻿//#define TEST_QUI
using UnityEngine;
using System.Collections;
using UnityEditor;

public class UnitTestWindow : EditorWindow
{

#if TEST_QUI


    private static UnitTestWindow _window;


    [MenuItem("QUI/Test")]
    public static void TestQUI()
    {
        _window = EditorWindow.GetWindow<UnitTestWindow>();
        _window.Init();
    }


    private TestData data;

    #region 初始化



    public void Init()
    {
        data = new TestData();



        data.id = "222";
        data.name = "AAA";
        data.number = 5;
        data.isObject = false;
    }


    #endregion

    #region GUI Draw

    void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("------------------------------");
        GUILayout.Space(10);



        data.id = QUI.TextField(data.id, "ID", 200);
        data.name = QUI.TextField(data.name, "Name", 200);
        data.number = QUI.IntField(data.number, "Count", 200);
        data.isObject = QUI.Toggle(data.isObject, "isObject", 200);

        GUILayout.Space(10);
        GUILayout.Label("------------------------------");
        GUILayout.Space(10);


        if(QUI.Button("Test", 80, 40, Color.green, Color.green))
        {
            Debug.Log("<color=#00ff00>Hello</color>");
        }


        if (QUI.CurClickedGUI == "ID") GUI.color = Color.green;
        GUILayout.Label("ID: " + data.id);
        if (QUI.CurClickedGUI == "ID") GUI.color = Color.white;
        if (QUI.CurClickedGUI == "Name") GUI.color = Color.green;
        GUILayout.Label("Name: " + data.name);
        if (QUI.CurClickedGUI == "Name") GUI.color = Color.white;
        if (QUI.CurClickedGUI == "Count") GUI.color = Color.green;
        GUILayout.Label("Count: " + data.number);
        if (QUI.CurClickedGUI == "Count") GUI.color = Color.white;
        if (QUI.CurClickedGUI == "isObject") GUI.color = Color.green;
        GUILayout.Label("isObject: " + data.isObject);
        if (QUI.CurClickedGUI == "isObject") GUI.color = Color.white;
    }



    #endregion

#endif

}


public class TestData
{
    public string id = "123";
    public string name = "name";
    public int number;
    public bool isObject;
}
