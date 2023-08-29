using System;
using System.Collections.Generic;
using Zenject;

namespace test_sber
{
    public class GameController: IInitializable, IDisposable
    {
        [Inject] 
        private InGameTimeController _inGameTimeController;
        [Inject] 
        private DialogsController _dialogsController;
        
        [Inject] 
        private List<Enemy> _enemies;
        
        public void Initialize()
        {
            foreach (var enemy in _enemies)
            {
                enemy.TargetReached += HandlePlayerKilled;
            }
        }

        public void Dispose()
        {
            foreach (var enemy in _enemies)
            {
                enemy.TargetReached -= HandlePlayerKilled;
            }
        }

        private void HandlePlayerKilled()
        {
            _inGameTimeController.Pause();
            var dialogGameOver = _dialogsController.CreateDialog<DialogGameOver>();
        }
    }
}