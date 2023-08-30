using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace test_sber
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField]
        public Player.Settings playerSettings;
        [SerializeField]
        public Enemy.Settings enemySettings;
        [SerializeField]
        public Spotter.Settings spotterSettings;
        [SerializeField]
        private LabelsSettings labelsSettings;
        
        [Serializable]
        public class LabelsSettings
        {
            public List<LocalizationEntry> labels;
        }

        [Serializable]
        public class LocalizationEntry
        {
            public string Name;
            public string Value;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings);
            Container.BindInstance(enemySettings);
            Container.BindInstance(spotterSettings);
            Container.BindInstance(labelsSettings);
        }
    }
}