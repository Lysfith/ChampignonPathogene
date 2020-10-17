using Assets.Scripts.Common;
using Assets.Scripts.Game.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Spores
{
    public class SporeState : MonoBehaviour
    {
        [Required] [SerializeField] private RectTransform _player;
        [Required] [SerializeField] private Color _color;

        [SerializeField] private float _lastStateChange;
        [SerializeField] private int _nbSpores;
        public int NbSpores => _nbSpores;
        public Color Color => _color;

        [SerializeField] private bool _isOnLeave = true;

        public bool IsOnLeave => _isOnLeave;

        private RectTransform _playerFolder;

        public UnityEvent OnChangeStateToFly;
        public UnityEvent OnChangeStateToLeave;

        public event EventHandler<int> OnNbSporeChange;

        private void Update()
        {
            _lastStateChange += Time.deltaTime;
        }

        public void AddNbSpores(int amount)
        {
            _nbSpores += amount;
            OnNbSporeChange?.Invoke(this, _nbSpores);
        }

        public void RemoveNbSpores(int amount)
        {
            _nbSpores -= amount;
            OnNbSporeChange?.Invoke(this, _nbSpores);
        }

        public void SetPlayerFolder(RectTransform folder)
        {
            _playerFolder = folder;
        }

        public void SetPlayerColor(Color color)
        {
            _color = color;
        }

        public bool CanChangeState()
        {
            if(_lastStateChange < 2)
            {
                return false;
            }

            if (_isOnLeave)
            {
                return true;
            }
            else
            {
                var leave = LeavesManager.Instance.HasLeaveAtPosition(_player.position);
                if (leave == null)
                {
                    return false;
                }

                return true;
            }
        }

        public void ChangeState()
        {
            if(_isOnLeave)
            {
                OnChangeStateToFly?.Invoke();
                _player.SetParent(_playerFolder, true);
                _isOnLeave = false;
            }
            else
            {
                var leave = LeavesManager.Instance.HasLeaveAtPosition(_player.position);
                if (leave == null)
                {
                    return;
                }

                OnChangeStateToLeave?.Invoke();
                _player.SetParent(leave.Rect, true);
                _isOnLeave = true;
            }

            _lastStateChange = 0;
        }
    }
}
