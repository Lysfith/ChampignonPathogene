﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Managers
{
    public class SeasonManager : MonoBehaviour
    {
        private const float YEAR_DURATION = 120;

        [SerializeField] private float _currentTime = 0;
        private bool _isRunning;

        private static SeasonManager _instance;

        public static SeasonManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        public void StartLevel()
        {
            _currentTime = 0;
            _isRunning = true;
        }

        public void StopLevel()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }

            _currentTime += Time.deltaTime;

            if(_currentTime > YEAR_DURATION)
            {
                _currentTime = 0;
            }
        }

        public float GetYearPercent()
        {
            return _currentTime / YEAR_DURATION;
        }
    }
}