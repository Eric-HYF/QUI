using UnityEngine;
using System.Collections;
using com.yodo1.qui;
using UnityEditor;

public class UnitTestWindow : EditorWindow
{



    private static UnitTestWindow  _window;


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


        data.id = QUI.TextField(data.id, "ID");
        data.name = QUI.TextField(data.name, "Name");
        QUI.IntField(ref data.number, "Count");
        QUI.Toggle(ref data.isObject, "isObject");

        //data.number = (int)QUI.TextInput(data.number, "Count");
        //data.isObject = (bool)QUI.TextInput(data.name, "Object");


        GUILayout.Label("ID: "+ data.id);

    }



    #endregion









}


public class UnitTextWindow
{

}




public class TestData
{
    public string id = "123";
    public string name = "name";
    public int number;
    public bool isObject;
}
