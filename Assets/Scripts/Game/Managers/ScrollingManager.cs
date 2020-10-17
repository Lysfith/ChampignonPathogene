using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class ScrollingManager : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _offset = 0;

        private static ScrollingManager _instance;

        public static ScrollingManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            _offset += Time.deltaTime * _speed;
        }

        public float GetOffset()
        {
            return _offset;
        }
    }
}
