using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public PlayerSettings player;

        [Serializable]
        public class PlayerSettings
        {
            public Player.Settings Settings;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(player.Settings);
        }
    }
}