using System;
using System.Collections.Generic;
using UnityEngine;

namespace test_sber
{
    public class DialogsController: MonoBehaviour
    {
        private const string PATH_TO_DIALOGS = "Prefabs/Dialogs/{0}";
        
        [SerializeField]
        private Transform _dialogsParent;
        
        private Dictionary<string, DialogBase> _dialogs = new Dictionary<string, DialogBase>();

        //todo по хорошему должна быть валидация - для каждого класса диалога в нужной директории существует префаб по имени класса диалога
        public T CreateDialog<T>() where T: DialogBase
        {
            var name = typeof(T).Name;
            
            var dialogPrefab = Resources.Load<GameObject>(string.Format(PATH_TO_DIALOGS, name));
            if (dialogPrefab == null)
            {
                throw new ArgumentException($"no dialog prefab with name {name}");
            }

            var dialog = Instantiate(dialogPrefab, _dialogsParent).GetComponent<T>();
            if (dialog == null)
            {
                throw new ArgumentException($"no corresponding dialog component on dialog {name}");
            }
            
            _dialogs.Add(name, dialog);
            return dialog;
        }

        public bool CloseDialog<T>(T dialogType) where T: DialogBase
        {
            var name = typeof(T).Name;
            
            if (_dialogs.ContainsKey(name))
            {
                Destroy(_dialogs[name].gameObject);
                _dialogs.Remove(name);
                return true;
            }

            return false;
        }
    }
}