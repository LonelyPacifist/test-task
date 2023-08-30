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
        
        [Serializable]
        public class Settings
        {
            public float speed;
            public float spottingTime;
            public float maxAttackAttemptDelay;
            public float attackDistance;
        }

        [Inject] 
        public Settings EnemySettings { get; }
        [Inject] 
        private InGameTimeController _inGameTimeController;
        [Inject] 
        private LazyInject<Player> _player;

        public event Action TargetReached;
        public bool Spotted => TotalSpottedTime >= EnemySettings.spottingTime;
        public bool CanBeSpotted => _state == State.Wait || _state == State.Attack;
        public float TotalSpottedTime { get; private set; }

        private NavMeshAgent _nma;
        private float _attackTimestamp;
        private State _state;
        private Vector3 _basePosition;

        private bool NmaTargetReached => _nma.hasPath && _nma.remainingDistance < _nma.stoppingDistance &&
                                         Vector3.Distance(_nma.destination, transform.position) <= EnemySettings.attackDistance;
        
        [Inject]
        public void Init()
        {
            _basePosition = transform.position;
            _attackTimestamp = Time.time + Random.Range(0, EnemySettings.maxAttackAttemptDelay);
            _state = State.Wait;
            _nma = GetComponent<NavMeshAgent>();
            _nma.stoppingDistance = EnemySettings.attackDistance;
            _nma.speed = EnemySettings.speed;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Wait:
                    if (_attackTimestamp < _inGameTimeController.InGameTime)
                    {
                        SetState(State.Attack);
                    }
                    break;
                case State.Attack:
                    _nma.SetDestination(_player.Value.transform.position);
                    if (NmaTargetReached)
                    {
                        TargetReached?.Invoke();
                        SetState(State.Idle);
                    }
                    break;
                case State.Return:
                    if (NmaTargetReached)
                    {
                        SetState(State.Idle);
                    }
                    break;
                case State.Idle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetState(State state)
        {
            Debug.Log($"{name} switched to {state}");
            _state = state;
            switch (state)
            {
                case State.Wait:
                    break;
                case State.Attack:
                    _nma.autoBraking = false;
                    _nma.stoppingDistance = EnemySettings.attackDistance;
                    break;
                case State.Return:
                    _nma.autoBraking = true;
                    _nma.stoppingDistance = 0;
                    _nma.SetDestination(_basePosition);
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
            TotalSpottedTime += duration;
            if (CanBeSpotted && TotalSpottedTime > EnemySettings.spottingTime)
            {
                Debug.Log($"{name} fully spotted!");
                SetState(State.Return);
            }
        }
    }
}