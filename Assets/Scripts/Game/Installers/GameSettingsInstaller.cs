using System;
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

        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings.settings);
            Container.BindInstance(enemySettings.settings);
            Container.BindInstance(baseSettings);
        }
    }
}