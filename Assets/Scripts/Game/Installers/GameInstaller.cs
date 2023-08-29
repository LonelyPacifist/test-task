using Zenject;

namespace test_sber
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InGameTimeController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LabelsResolver>().AsSingle();
        }
    }
}