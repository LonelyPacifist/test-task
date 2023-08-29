using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class GameController: IInitializable, IDisposable
    {
        [Inject] 
        private InGameTimeController _inGameTimeController;
        [Inject] 
        private UIController _uiController;
        
        [Inject(Id = "StartPoint")] 
        private Interactable _start;
        [Inject(Id = "FinishPoint")] 
        private Interactable _finish;
        [Inject(Id = "Killzone")] 
        private List<Interactable> _killzones;
        
        [Inject] 
        private List<Enemy> _enemies;
        private bool _playerKilled;
        
        public void Initialize()
        {
            _start.Interacted += HandleGameStarted;
            _finish.Interacted += HandleGameFinished;

            foreach (var killzone in _killzones)
            {
                killzone.Interacted += HandlePlayerKilled;
            }
            
            _start.gameObject.SetActive(true);
            _finish.gameObject.SetActive(false);
            
            foreach (var enemy in _enemies)
            {
                enemy.TargetReached += HandlePlayerKilled;
            }
        }

        public void Dispose()
        {
            _start.Interacted -= HandleGameStarted;
            _finish.Interacted -= HandleGameFinished;
            
            foreach (var killzone in _killzones)
            {
                killzone.Interacted -= HandlePlayerKilled;
            }
            
            foreach (var enemy in _enemies)
            {
                enemy.TargetReached -= HandlePlayerKilled;
            }
        }

        private void HandleGameStarted()
        {
            _start.gameObject.SetActive(false);
            _finish.gameObject.SetActive(true);
            _inGameTimeController.Resume();
        }

        private void HandleGameFinished()
        {
            EndGame();
        }

        private void HandlePlayerKilled()
        {
            _playerKilled = true;
            EndGame();
        }

        private void EndGame()
        {
            _inGameTimeController.Pause();
            Time.timeScale = 0;
            var dialogGameOver = _uiController.CreateDialog<DialogGameOver>();
            dialogGameOver.Build(_playerKilled, _enemies.Select(x => x.Spotted));
        }
    }
}