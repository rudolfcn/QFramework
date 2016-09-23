﻿using UnityEngine;
#if ToLua
using LuaInterface;
#endif
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace QFramework {
	public class QLuaBehaviour : QFramework.QMonoBehaviour {
        private string data = null;
		#if ToLua
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();
		Dictionary<string,LuaFunction> toggles = new Dictionary<string, LuaFunction>();
		#endif
        protected void Awake() {
            QUtil.CallMethod(name, "Awake", gameObject);
        }

        protected void Start() {
            QUtil.CallMethod(name, "Start");
        }

        protected void OnClick() {
            QUtil.CallMethod(name, "OnClick");
        }

        protected void OnClickEvent(GameObject go) {
            QUtil.CallMethod(name, "OnClick", go);
        }
		public override void ProcessMsg (QMsg msg)
		{
			throw new NotImplementedException ();
		}

		#if ToLua
        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            go.GetComponent<Button>().onClick.AddListener(
                delegate() {
                    luafunc.Call(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go) {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc)) {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }


		/// <summary>
		/// 添加ToggleValueChanged事件
		/// </summary>
		public void AddToggleValueChanged(GameObject go, LuaFunction luafunc) {
			if (go == null || luafunc == null) return;
			toggles.Add(go.name, luafunc);
			go.GetComponent<Toggle>().onValueChanged.AddListener(
				delegate(bool value) {
					luafunc.Call(value);
				}
			);
		}

		/// <summary>
		/// 删除ToggleValueChanged事件
		/// </summary>
		/// <param name="go"></param>
		public void RemoveToggleValueChanged(GameObject go) {
			if (go == null) return;
			LuaFunction luafunc = null;
			if (buttons.TryGetValue(go.name, out luafunc)) {
				luafunc.Dispose();
				luafunc = null;
				toggles.Remove(go.name);
			}
		}

		/// <summary>
		/// 清除ToggleValueChanged事件
		/// </summary>
		public void ClearValueChanged() {
			foreach (var de in toggles) {
				if (de.Value != null) {
					de.Value.Dispose();
				}
			}
			toggles.Clear();
		}




        //-----------------------------------------------------------------
        protected void OnDestroy() {
            ClearClick();
#if ASYNC_MODE
            string abName = name.ToLower().Replace("panel", "");
            ResManager.UnloadAssetBundle(abName + AppConst.ExtName);
#endif
            QUtil.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }
		#endif
    }
}