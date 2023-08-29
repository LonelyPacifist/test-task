using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class LabelsResolver
    {
        [Inject] 
        private GameSettingsInstaller.LabelsSettings _settings;

        public LabelsResolver()
        {
            Debug.Log("created");
        }
        
        public string Resolve(string id)
        {
            var index = _settings.labels.FindIndex(x => x.Name == id);
            if (index == -1)
            {
                throw new ArgumentException($"no such label id {id}");
            }

            return _settings.labels[index].Value;
        }
    }
}