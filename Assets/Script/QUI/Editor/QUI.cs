using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace  com.yodo1.qui
{


    public enum Skin
    {
        TextField,
        Button,
    }





    /// <summary>
    /// QUI Main Class
    /// </summary>
    public class QUI
    {


        public const int TITLE_WIDTH = 60;
        public const int TEXT_WIDTH = 160;
        public const int TEXT_HEIGHT = 18;
        public const int BUTTON_WIDTH_1 = 80;
        public const int BUTTON_HEIGHT_1 = 17;


        private static string cur_clicked_guid = "";

        private static Dictionary<string, object> tempDic = new Dictionary<string, object>();


        private static string tmpStr;



        #region LabelText

        /// <summary>
        /// Input Label for Text input
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static void LabelText(ref string value, string label)
        {
            string guid = label;
            
            if (string.IsNullOrEmpty(cur_clicked_guid))
            {
                // Display

                // Text Body
                string val = value;
                LabelText_Display(val, label, () =>
                {
                    cur_clicked_guid = guid;
                    tempDic[guid] = val;
                    tmpStr = val;
                });

            }
            else
            {
                // Edit Text
                if (cur_clicked_guid == guid)
                {
                    int actionId = 0;
                    value = LabelText_Edit(value, label, out actionId);

                    if(actionId == 1)
                    {
                        // OK 
                        tmpStr = "";
                        cur_clicked_guid = "";
                        actionId = 0;
                    }
                    else if(actionId == 2)
                    {
                        // Cancel
                        cur_clicked_guid = "";
                        if (tempDic.ContainsKey(guid) && tempDic[guid] != null)
                        {
                            value = tempDic[guid].ToString();
                            tmpStr = "";
                        }
                        actionId = 0;
                    }
                }
                else
                {
                    // Lock
                    LabelText_Lock(value, label);
                }
            }


        }

        /// <summary>
        /// 可触发文本
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="onClick"></param>
        private static void LabelText_Display(string value, string label, Action onClick = null)
        {
            GUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                GUILayout.Label(label, GUILayout.Width(TITLE_WIDTH), GUILayout.Height(TEXT_HEIGHT));

                if (GUILayout.Button(value, new GUIStyle(Skin.TextField.ToString()), GUILayout.Width(TEXT_WIDTH), GUILayout.Height(TEXT_HEIGHT)))
                {
                    if (onClick != null) onClick();
                }

            }
            GUILayout.EndHorizontal();
        }




        /// <summary>
        /// 编辑模式的LabelText
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        private static string LabelText_Edit(string value, string label, out int actionId)
        {
            int id = 0;

            GUILayout.BeginHorizontal();
            // Edit
            GUILayout.Label(label, GUILayout.Width(TITLE_WIDTH));

            GUI.color = Color.cyan;
            GUI.backgroundColor = Color.gray;
            tmpStr = GUILayout.TextField(tmpStr, GUILayout.Width(TEXT_WIDTH), GUILayout.Height(TEXT_HEIGHT));

            Button("OK", BUTTON_WIDTH_1, BUTTON_HEIGHT_1, Color.green, Color.green, () => { id = 1; });
            Button("Cancel", BUTTON_WIDTH_1, BUTTON_HEIGHT_1, Color.red, Color.red, () => { id = 2; });

            actionId = id;


            GUILayout.EndHorizontal();

            if (id == 1) return tmpStr;
            return value;
            
        }



        /// <summary>
        /// 锁定模式的LabelText
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        private static void LabelText_Lock(string value, string label)
        {
            GUI.color = Color.gray;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(TITLE_WIDTH), GUILayout.Height(TEXT_HEIGHT));
            GUILayout.Label(value.ToString(), new GUIStyle("TextField"), GUILayout.Width(TEXT_WIDTH), GUILayout.Height(TEXT_HEIGHT));
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
        }

        #endregion



        #region 按钮显示

        /// <summary>
        /// 按钮显示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onClick"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="frontColor"></param>
        public static void Button(string name, float w, float h, Color frontColor, Color backColor, Action onClick)
        {

            GUI.color = frontColor;
            GUI.backgroundColor = backColor;
            if (GUILayout.Button(name, GUILayout.Width(w), GUILayout.Height(h)))
            {
                if (onClick != null) onClick();
            }
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
        }



        #endregion

    }


}