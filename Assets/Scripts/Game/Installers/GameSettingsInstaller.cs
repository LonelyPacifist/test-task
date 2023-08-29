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
        private PlayerSettings playerSettings;
        [SerializeField]
        private EnemySettings enemySettings;
        [SerializeField]
        private BaseSettings baseSettings;
        [SerializeField]
        private LabelsSettings labelsSettings;

        [Serializable]
        public class PlayerSettings
        {
            public Player.Settings settings;
        }
        
        [Serializable]
        public class EnemySettings
        {
            public Enemy.Settings settings;
        }
        
        [Serializable]
        public class BaseSettings
        {
            public Vector2 viewportAimCoords;
        }
        
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
            Container.BindInstance(playerSettings.settings);
            Container.BindInstance(enemySettings.settings);
            Container.BindInstance(baseSettings);
            Container.BindInstance(labelsSettings);
        }
    }
}