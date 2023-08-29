using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    [RequireComponent(typeof(Collider))]
    public class Interactable : MonoBehaviour
    {
        public event Action Interacted;
        
        [Inject]
        private LazyInject<Player> _player;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player.Value.gameObject)
            {
                Interacted?.Invoke();
            }
        }
    }
}