using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace test_sber
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, ISpottable
    {
        private enum State
        {
            Wait,
            Attack,
            Return,
            Idle
        }

        public event Action TargetReached;
        public event Action Spotted;
        
        public bool CanBeSpotted => _state == State.Wait || _state == State.Attack;

        [Inject] 
        private Settings _settings;
        [Inject] 
        private InGameTimeController _inGameTimeController;
        
        private LazyInject<Player> _player;

        private NavMeshAgent _nma;
        private float _attackTimestamp;
        private State _state;
        private Vector3 _basePosition;
        private float _totalSpottedTime;
        
        [Serializable]
        public class Settings
        {
            public float speed;
            public float spottingTime;
            public float maxAttackAttemptDelay;
            public float attackDistance;
        }

        [Inject]
        public void Init(LazyInject<Player> player)
        {
            _player = player;
            _basePosition = transform.position;
            _attackTimestamp = Time.time + Random.Range(0, _settings.maxAttackAttemptDelay);
            _state = State.Wait;
            _nma = GetComponent<NavMeshAgent>();
            _nma.stoppingDistance = _settings.attackDistance;
            _nma.speed = _settings.speed;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Wait:
                    if (_attackTimestamp < _inGameTimeController.InGameTime)
                    {
                        _state = State.Attack;
                    }
                    break;
                case State.Attack:
                    _nma.SetDestination(_player.Value.transform.position);
                    if (_nma.velocity.sqrMagnitude == 0 && _nma.hasPath && _nma.remainingDistance <= _nma.stoppingDistance)
                    {
                        TargetReached?.Invoke();
                        _state = State.Idle;
                    }
                    break;
                case State.Return:
                    _nma.SetDestination(_basePosition); //todo только при заходе в стейт
                    if (_nma.velocity.sqrMagnitude == 0 && _nma.hasPath && _nma.remainingDistance <= _nma.stoppingDistance)
                    {
                        _state = State.Idle;
                    }
                    break;
                case State.Idle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        
        public void TrySpot(float duration)
        {
            Debug.Log($"{name} spotted!");
            _totalSpottedTime += duration;
            if (CanBeSpotted && _totalSpottedTime > _settings.spottingTime)
            {
                Spotted?.Invoke();
                Debug.Log($"{name} fully spotted!");
                _state = State.Return;
            }
        }
    }
}