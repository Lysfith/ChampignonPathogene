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

        private float _speed = 200;
        private float _lastOffset = 0;

        private void OnEnable()
        {
            _lastOffset = ScrollingManager.Instance.GetOffset();
        }

        // Update is called once per frame
        void Update()
        {
            float x = 0;
            float y = 0;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                x--;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                x++;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                y++;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                y-=2;
            }

            var offset = ScrollingManager.Instance.GetOffset();
            var deltaOffset = offset - _lastOffset;

            _player.anchoredPosition += new Vector2(0, deltaOffset);
            _lastOffset = offset;

            _player.anchoredPosition += new Vector2(x, y) * _speed * Time.deltaTime;
        }
    }
}
