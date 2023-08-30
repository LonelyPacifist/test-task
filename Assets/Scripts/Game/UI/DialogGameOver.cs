using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class DialogGameOver : DialogBase
    {
        [Inject] 
        private LabelsResolver _resolver;
        [Inject] 
        private UIController _uiController;
        
        [SerializeField]
        private Transform _containerEnemiesStatus; 
        [SerializeField]
        private TMP_Text _labelResult; 
        
        public void Build(bool playerStatus, IEnumerable<bool> enemiesSpottedStatus)
        {
            _labelResult.SetText(playerStatus 
                ? _resolver.Resolve(Labels.PlayerStatusWin) 
                : _resolver.Resolve(Labels.PlayerStatusLose));
            foreach (var enemyStatus in enemiesSpottedStatus)
            {
                var widget = _uiController.CreateWidget<WidgetEnemyFinalStatus>(_containerEnemiesStatus);
                widget.Build(enemyStatus);
            }
        }
    }
}