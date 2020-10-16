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
                y--;
            }

            _player.anchoredPosition += new Vector2(x, y) * _speed * Time.deltaTime;
        }
    }
}
