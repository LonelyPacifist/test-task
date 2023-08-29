using System;
using UnityEngine;
using Zenject;

namespace test_sber
{
    public class InGameTimeController : IInitializable, ITickable
    {
        private enum State
        {
            Running,
            Paused
        }

        private State _state;
        private float _inGameTime;

        public float InGameTime => _inGameTime;

        public void Initialize()
        {
            _state = State.Running;
        }
        
        public void Pause()
        {
            _state = State.Paused;
        }

        public void Resume()
        {
            _state = State.Running;
        }

        public void Tick()
        {
            switch (_state)
            {
                case State.Running:
                    _inGameTime += Time.deltaTime;
                    break;
                case State.Paused:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}