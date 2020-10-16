using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Spores
{
    public class SporeState : MonoBehaviour
    {
        [SerializeField] private int _nbSpores;
        public int NbSpores => _nbSpores;

        [SerializeField] private bool _isOnLeave = true;

        public UnityEvent OnChangeStateToFly;
        public UnityEvent OnChangeStateToLeave;

        public event EventHandler<int> OnNbSporeChange;

        public void AddNbSpores(int amount)
        {
            _nbSpores += amount;
            OnNbSporeChange?.Invoke(this, _nbSpores);
        }


        public void ChangeState()
        {
            if(_isOnLeave)
            {
                OnChangeStateToFly?.Invoke();
                _isOnLeave = false;
            }
            else
            {
                OnChangeStateToLeave?.Invoke();
                _isOnLeave = true;
            }
        }
    }
}
