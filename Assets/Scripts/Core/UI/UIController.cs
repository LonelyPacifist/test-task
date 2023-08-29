using System;
using System.Collections.Generic;
using UnityEngine;

namespace test_sber
{
    public class UIController: MonoBehaviour
    {
        private const string PATH_TO_UI_PREFABS = "Prefabs/UI/{0}";
        
        [SerializeField]
        private Transform _dialogsParent;
        
        private Dictionary<string, DialogBase> _dialogs = new Dictionary<string, DialogBase>();

        //todo по хорошему должна быть валидация - для каждого класса диалога в нужной директории существует префаб по имени класса диалога
        public T CreateDialog<T>() where T: DialogBase
        {
            var dialogName = typeof(T).Name;
            
            var dialogPrefab = Resources.Load<GameObject>(string.Format(PATH_TO_UI_PREFABS, dialogName));
            if (dialogPrefab == null)
            {
                throw new ArgumentException($"no dialog prefab with name {dialogName}");
            }

            var dialog = Instantiate(dialogPrefab, _dialogsParent).GetComponent<T>();
            if (dialog == null)
            {
                throw new ArgumentException($"no corresponding dialog component on dialog {dialogName}");
            }
            
            _dialogs.Add(dialogName, dialog);
            return dialog;
        }

        public bool CloseDialog<T>(T dialogType) where T: DialogBase
        {
            var dialogName = typeof(T).Name;
            
            if (_dialogs.ContainsKey(dialogName))
            {
                Destroy(_dialogs[dialogName].gameObject);
                _dialogs.Remove(dialogName);
                return true;
            }

            return false;
        }
        
        public T CreateWidget<T>(Transform parent) where T: WidgetBase
        {
            var widgetName = typeof(T).Name;
            
            var prefab = Resources.Load<GameObject>(string.Format(PATH_TO_UI_PREFABS, widgetName));
            if (prefab == null)
            {
                throw new ArgumentException($"no widget prefab with name {widgetName}");
            }

            var widget = Instantiate(prefab, parent).GetComponent<T>();
            if (widget == null)
            {
                throw new ArgumentException($"no corresponding widget component on dialog {widgetName}");
            }
            
            return widget;
        }
    }
}