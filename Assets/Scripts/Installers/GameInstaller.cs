using Zenject;

namespace test_sber
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallEnemies();
            InstallPlayer();
        }
        
        void InstallPlayer()
        {
            
        }

        void InstallEnemies()
        {
            
        }
    }
}