using Assets.Scripts.Game.Managers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Spores
{
    public class SporeMove : MonoBehaviour
    {
        [Required][SerializeField] private RectTransform _player;
        [Required][SerializeField] private SporeInput _input;
        [Required] [SerializeField] private SporeState _sporeState;
        [Required] [SerializeField] private SporeOnFly _sporeOnFly;
        [Required] [SerializeField] private SporeOnLeave _sporeOnLeave;

        private float _speed = 200;
        private float _lastOffset = 0;

        private void OnEnable()
        {
            _lastOffset = ScrollingManager.Instance.GetOffset();
        }

        // Update is called once per frame
        void Update()
        {
            float x = _input?.GetInputBind().GetHorizontalValue() ?? 0;
            float y = _input?.GetInputBind().GetVerticalValue() ?? 0;

            if (y < 0)
            {
                y *= 2;
            }

            var offset = ScrollingManager.Instance.GetOffset();
            if (_sporeState.IsOnLeave)
            {
                if ((Mathf.Abs(x) > 0.3f || Mathf.Abs(y) > 0.3f) && _sporeState.CanChangeState())
                {
                    _sporeOnLeave.Explode();
                }
            }
            else
            {
                var leave = LeavesManager.Instance.HasLeaveAtPosition(_player.position);
                if (leave != null && _sporeState.CanChangeState())
                {
                    _sporeOnFly.Landing();
                }
                else
                {
                    var deltaOffset = offset - _lastOffset;

                    _player.anchoredPosition += new Vector2(0, deltaOffset);
                    _player.anchoredPosition += new Vector2(x, y) * _speed * Time.deltaTime;
                }
            }

            _lastOffset = offset;
        }
    }
}
