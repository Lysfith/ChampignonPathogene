using Assets.Scripts.Game.SciptableObjects;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Required] [SerializeField] private AnimationCurve _audioCurve;
        [Required] [SerializeField] private AudioSource _audioSourceSummer;
        [Required] [SerializeField] private AudioSource _audioSourceWinter;
        [Required] [SerializeField] private AudioSource _audioSourceEnd;
        [Required] [SerializeField] private CanvasGroup _yearText;
        [Required] [SerializeField] private CanvasGroup _endScreen;
        [Required] [SerializeField] private TextMeshProUGUI _endText;

        private bool _isRunning;

        private static LevelManager _instance;

        public static LevelManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            LeavesManager.Instance.StartLevel();
            ScrollingManager.Instance.StartLevel();
            SeasonManager.Instance.StartLevel();

            _audioSourceSummer.volume = 0;
            _audioSourceWinter.volume = 0;

            _audioSourceSummer.Play();
            _audioSourceWinter.Play();

            _audioSourceSummer.DOFade(1f, 1f).OnComplete(new TweenCallback(() =>
            {
                _isRunning = true;
            })).Play();
        }

        private void Update()
        {
            if(!_isRunning)
            {
                return;
            }

            var percent = SeasonManager.Instance.GetYearPercent();

            var curveValue = _audioCurve.Evaluate(percent);

            _audioSourceSummer.volume = 1- curveValue;
            _audioSourceWinter.volume = curveValue;
        }

        public void StopLevel(string player)
        {
            _audioSourceSummer.DOFade(0f, 1f).OnComplete(new TweenCallback(() =>
            {
                _audioSourceSummer.Stop();
            })).Play();
            _audioSourceWinter.DOFade(0f, 1f).OnComplete(new TweenCallback(() =>
            {
                _audioSourceWinter.Stop();
            })).Play();

            LeavesManager.Instance.StopLevel();
            ScrollingManager.Instance.StopLevel();
            SeasonManager.Instance.StopLevel();

            var str = $"Vous avez survecu a {SeasonManager.Instance.GetNbYears()-1} hiver(s)";

            if(player != null)
            {
                str += $"\nLe joueur {player} gagne !";
            }

            _endText.text = str;

            _yearText.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                _endScreen.DOFade(1, 1f).OnComplete(new TweenCallback(() =>
                {
                    _audioSourceEnd.volume = 0;
                    _audioSourceEnd.Play();
                    _audioSourceEnd.DOFade(1f, 1f).Play();
                })).Play();
            })).Play();
        }

        public void MainMenu()
        {
            _endScreen.blocksRaycasts = false;
            _endScreen.interactable = false;
            _audioSourceEnd.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                SceneManager.LoadScene("MenuScene");
            })).Play();
        }

    }
}
