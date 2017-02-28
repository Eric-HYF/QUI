using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace  UnityEditor
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
        #region Property

        public const string CHECK_MARK = "✔";
        public const string CROSS_MARK = "✘";
        public const string PASTE_MARK = "P";


        public const int ACTION_NONE = 0;
        public const int ACTION_CLICK = 1;
        public const int ACTION_CANCEL = 2;
        public const int ACTION_PASTE = 3;

        public const int TITLE_WIDTH = 60;
        public const int TEXT_WIDTH = 160;
        public const int TEXT_HEIGHT = 18;
        public const int BUTTON_WIDTH_1 = 80;
        public const int BUTTON_WIDTH_2 = 25;
        public const int BUTTON_HEIGHT_1 = 25;
        public const int BUTTON_HEIGHT_2 = 17;

        private static string cur_clicked_guid = "";
        public static string CurClickedGUI
        {
            get { return cur_clicked_guid; }
        }

        private static Dictionary<string, object> tempDic = new Dictionary<string, object>();


        private static ValueBuffer<string> strBuff = new ValueBuffer<string>();
        private static ValueBuffer<int> intBuff = new ValueBuffer<int>();
        private static ValueBuffer<bool> boolBuff = new ValueBuffer<bool>();


        private static bool boolTmp;
        private static string strTemp;
        private static int intTemp;

        #endregion

        #region 公用接口

        /// <summary>
        /// 清除编辑标记
        /// </summary>
        public static void CleanEditMark()
        {
            cur_clicked_guid = "";
        }

        #endregion


        #region TextField

        /// <summary>
        /// Input Label for Text input
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label">the unique id for the ui </param>
        /// <returns></returns>
        public static void TextField(ref string value, string label, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            string guid = label;
            int id = ACTION_NONE;
            if (string.IsNullOrEmpty(cur_clicked_guid))
            {
                // Display

                // Text Body

                TextField_Display(value, label, out id, bodyWidth, titleWidth);
                if(id == ACTION_CLICK)
                {
                    cur_clicked_guid = guid;
                    strBuff[guid] = value;
                    strTemp = value;
                }


            }
            else
            {
                // Edit Text
                if (cur_clicked_guid == guid)
                {
                    value = TextField_Edit(value, label, out id, bodyWidth, titleWidth);
                    if(id == ACTION_CLICK)
                    {
                        // OK 
                        strBuff.Remove(cur_clicked_guid);
                        cur_clicked_guid = "";
                        id = ACTION_NONE;
                    }
                    else if(id == ACTION_CANCEL)
                    {
                        // Cancel
                        cur_clicked_guid = "";
                        if ( !string.IsNullOrEmpty(strBuff[guid]))
                        {
                            value = strBuff[guid];
                            strBuff.Remove(guid);
                        }
                        id = ACTION_NONE;
                    }
                    else if(id == 3)
                    {
                        EditorGUIUtility.systemCopyBuffer = value;
                    }
                }
                else
                {
                    // Lock
                    TextField_Lock(value, label, out id, bodyWidth, titleWidth);
                    if(id == 1)
                    {
                        if (cur_clicked_guid != label)
                        {
                            strBuff.Remove(cur_clicked_guid);
                            cur_clicked_guid = label; // Click and change target
                            strBuff[label] = value;
                            strTemp = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Display mode and touchable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        private static void TextField_Display(string value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = ACTION_NONE;
            GUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));

                if (GUILayout.Button(value, new GUIStyle(Skin.TextField.ToString()), GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT)))
                {
                    actionId = ACTION_CLICK;
                }

            }
            GUILayout.EndHorizontal();
        }




        /// <summary>
        /// Edit mode 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="onOK"></param>
        /// <param name="onCancel"></param>
        private static string TextField_Edit(string value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            int id = 0;
            GUILayout.BeginHorizontal();
            // Edit
            GUILayout.Label(label, GUILayout.Width(titleWidth));

            GUI.color = Color.cyan;
            GUI.backgroundColor = Color.gray;
            strTemp = GUILayout.TextField(strTemp, GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT));
            if(Button(CHECK_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.green, Color.green)) { id = ACTION_CLICK; };
            if(Button(CROSS_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.red, Color.red )) { id = ACTION_CANCEL; };
            if(Button(PASTE_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.yellow, Color.yellow)){ id = ACTION_PASTE; };
            GUILayout.EndHorizontal();

            actionId = id;
            if (id == ACTION_CLICK) return strTemp;
            else if (id == ACTION_CANCEL) return strBuff[label];
            return value;

        }



        /// <summary>
        /// Lock mode
        /// Clicked to change target
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        private static void TextField_Lock(string value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = ACTION_NONE;
            GUI.color = Color.gray;
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));
            if(GUILayout.Button(value, new GUIStyle("TextField"), GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT)))
            {
                actionId = ACTION_CLICK;
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
        }

        #endregion

        #region IntField


        /// <summary>
        /// IntInput with label
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        public static void IntField(ref int value, string label, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            int actionId = 0;
            if (string.IsNullOrEmpty(cur_clicked_guid))
            {
                // Display

                // Text Body
                IntField_Display(value, label, out actionId, bodyWidth, titleWidth);
                if (actionId == ACTION_CLICK)
                {
                    cur_clicked_guid = label;
                    intBuff[label] = value;
                    intTemp = value;
                }
            }
            else
            {
                // Edit Text
                if (cur_clicked_guid == label)
                {
                    actionId = ACTION_NONE;
                    value = IntField_Edit(value, label, out actionId, bodyWidth, titleWidth);

                    if (actionId == ACTION_CLICK)
                    {
                        // OK 
                        intBuff.Remove(label);
                        cur_clicked_guid = "";
                        actionId = 0;
                    }
                    else if (actionId == ACTION_CANCEL)
                    {
                        // Cancel
                        cur_clicked_guid = "";
                        value = intBuff[label];
                        strBuff.Remove(label);
                        actionId = ACTION_NONE;
                    }
                    else if (actionId == ACTION_PASTE)
                    {
                        EditorGUIUtility.systemCopyBuffer = value.ToString();
                    }
                }
                else
                {
                    // Lock
                    IntField_Lock(value, label, out actionId, bodyWidth, titleWidth);
                    if(actionId == ACTION_CLICK)
                    {
                        cur_clicked_guid = label; // Click and change target
                        intBuff[label] = value;
                        intTemp = value;
                    }
                }
            }


        }


        /// <summary>
        /// Display Mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        private static void IntField_Display(int value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            GUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));

                if (GUILayout.Button(value.ToString(), new GUIStyle(Skin.TextField.ToString()), GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT)))
                {
                    actionId = ACTION_CLICK;
                }

            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Edtior Mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        private static int IntField_Edit(int value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            int id = 0;
            GUILayout.BeginHorizontal();
            // Edit
            GUILayout.Label(label, GUILayout.Width(titleWidth));

            GUI.color = Color.cyan;
            GUI.backgroundColor = Color.gray;
            intTemp = EditorGUILayout.IntField(intTemp, GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT));
            if( Button(CHECK_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.green, Color.green)) { id = ACTION_CLICK; };
            if( Button(CROSS_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.red, Color.red)){ id = ACTION_CANCEL; };
            if( Button(PASTE_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.yellow, Color.yellow)) { id = ACTION_PASTE; };
            GUILayout.EndHorizontal();


            actionId = id;
            if (id == ACTION_CLICK) return intTemp;
            else if (id == ACTION_CANCEL) return intBuff[label];
            return value;

        }


        /// <summary>
        /// Lock Mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        private static void IntField_Lock(int value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            GUI.color = Color.gray;
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));
            if (GUILayout.Button(value.ToString(), new GUIStyle("TextField"), GUILayout.Width(bodyWidth), GUILayout.Height(TEXT_HEIGHT)))
            {
                if (cur_clicked_guid != label) actionId = ACTION_CLICK;
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
        }


        #endregion

        #region Toggle

        /// <summary>
        /// IntInput with label
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        public static void Toggle(ref bool value, string label, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            int actionId = 0;
            if (string.IsNullOrEmpty(cur_clicked_guid))
            {
                // Display

                // Text Body
                Toggle_Display(value, label, out actionId, bodyWidth, titleWidth);
                if (actionId == ACTION_CLICK)
                {
                    cur_clicked_guid = label;
                    boolBuff[label] = value;
                }
            }
            else
            {
                // Edit Text
                if (cur_clicked_guid == label)
                {
                    actionId = ACTION_NONE;
                    value = Toggle_Edit(value, label, out actionId, bodyWidth, titleWidth); ;

                    if (actionId == ACTION_CLICK)
                    {
                        // OK 
                        boolBuff.Remove(label);
                        cur_clicked_guid = "";
                        actionId = 0;
                    }
                    else if (actionId == ACTION_CANCEL)
                    {
                        // Cancel
                        cur_clicked_guid = "";
                        value = boolBuff[label];
                        boolBuff.Remove(label);
                        actionId = ACTION_NONE;
                    }
                    else if (actionId == ACTION_PASTE)
                    {
                        EditorGUIUtility.systemCopyBuffer = value.ToString();
                    }
                }
                else
                {
                    // Lock
                    Toggle_Lock(value, label, out actionId, bodyWidth, titleWidth);
                    if (actionId == ACTION_CLICK)
                    {
                        cur_clicked_guid = label; // Click and change target
                        boolBuff [label] = value;
                        boolTmp = value;
                    }
                }
            }


        }


        /// <summary>
        /// Display mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        private static void Toggle_Display(bool value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            if (!string.IsNullOrEmpty(label))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));

                bool f = GUILayout.Toggle(value, "");
                if( f != value)
                {
                    actionId = ACTION_CLICK;
                }
                GUILayout.EndHorizontal();
            }

        }



        /// <summary>
        /// Edtior Mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        private static bool Toggle_Edit(bool value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            int id = 0;
            GUILayout.BeginHorizontal();
            // Edit
            GUILayout.Label(label, GUILayout.Width(titleWidth));

            boolTmp = EditorGUILayout.Toggle(boolTmp, GUILayout.Width(bodyWidth));
            if( Button(CHECK_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.green, Color.green)) { id = ACTION_CLICK; };
            if( Button(CROSS_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.red, Color.red)) { id = ACTION_CANCEL; };
            if( Button(PASTE_MARK, BUTTON_WIDTH_2, BUTTON_HEIGHT_2, Color.yellow, Color.yellow)) { id = ACTION_PASTE; };
            GUILayout.EndHorizontal();



            actionId = id;
            if (id == ACTION_CLICK) return boolTmp;
            else if (id == ACTION_CANCEL) return boolBuff[label];
            return value;

        }

        /// <summary>
        /// Lock mode
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="actionId"></param>
        private static void Toggle_Lock(bool value, string label, out int actionId, float bodyWidth = TEXT_WIDTH, float titleWidth = TITLE_WIDTH)
        {
            actionId = 0;
            GUI.color = Color.gray;
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(titleWidth), GUILayout.Height(TEXT_HEIGHT));
            bool k = EditorGUILayout.Toggle(value, GUILayout.Width(bodyWidth));
            if(k != value)
            {
                if (cur_clicked_guid != label) actionId = ACTION_CLICK;
            }
            GUILayout.EndHorizontal();
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
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
        public static bool Button(string name, float w, float h, Color frontColor, Color backColor)
        {
            bool res = false;
            GUI.color = frontColor;
            GUI.backgroundColor = backColor;
            if (GUILayout.Button(name, GUILayout.Width(w), GUILayout.Height(h)))
            {
                res = true;
            }
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;

            return res;
        }



        #endregion

    }


    #region Value Buffer

    /// <summary>
    /// 值缓冲器
    /// </summary>
    public class ValueBuffer<T>
    {
        private Dictionary<string, T> buffer = new Dictionary<string, T>();


        /// <summary>
        /// Value exsit
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has(string key)
        {
            return buffer.ContainsKey(key);
        }


        /// <summary>
        /// Get the value 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]
        {
            get
            {
                if (Has(key)) return buffer[key];
                return default(T);
            }

            set
            {
                buffer[key] = value;
            }
        }


        public void Remove(string key)
        {
            if (Has(key)) buffer.Remove(key);
        }

    }

    #endregion


}