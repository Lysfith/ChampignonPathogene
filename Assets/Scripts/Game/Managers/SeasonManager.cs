using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Managers
{
    public class SeasonManager : MonoBehaviour
    {
        private const float YEAR_DURATION = 80;

        [SerializeField] private float _currentTime = 0;
        [SerializeField] private int _nbYears = 1;
        [Required] [SerializeField] private TextMeshProUGUI _text;
        private bool _isRunning;
        private bool _isPausing;
        private bool _needWinterPause;
        private float _winterDuration;

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
            _text.text = $"AN {_nbYears}";
            _needWinterPause = true;
            _winterDuration = 0;
        }

        public void StopLevel()
        {
            _isRunning = false;
        }

        public int GetNbYears()
        {
            return _nbYears;
        }

        private void Update()
        {
            if (!_isRunning || _isPausing)
            {
                return;
            }

            _currentTime += Time.deltaTime;
           
            if(GetYearPercent() >= 0.5f && _needWinterPause)
            {
                _currentTime = YEAR_DURATION / 2f;
                Debug.Log("Pause Winter");
                StartCoroutine(PauseWinter());
                _needWinterPause = false;
            }

            if(_currentTime > YEAR_DURATION)
            {
                _nbYears++;
                _text.text = $"AN {_nbYears}";
                _currentTime = 0;
                _needWinterPause = true;
                _winterDuration += 10;
            }
        }

        private IEnumerator PauseWinter()
        {
            _isPausing = true;

            yield return new WaitForSeconds(_winterDuration);

            _isPausing = false;

            Debug.Log("Fin Pause Winter");
        }

        public float GetYearPercent()
        {
            return _currentTime / YEAR_DURATION;
        }
    }
}
