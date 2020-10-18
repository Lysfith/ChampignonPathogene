using Assets.Scripts.Game.SciptableObjects;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Required] [SerializeField] private AudioSource _audioSource;
        [Required] [SerializeField] private CanvasGroup _mainMenu;
        [Required] [SerializeField] private CanvasGroup _reglesMenu;
        [Required] [SerializeField] private CanvasGroup _creditsMenu;
        [Required] [SerializeField] private PlayerSlotList _playerSlots;
        [Required] [SerializeField] private Button _1pButton;
        [Required] [SerializeField] private Button _2pButton;

        [Required] [SerializeField] private CanvasGroup[] _pages;

        private int _currentPage = 0;

        private static MenuManager _instance;

        public static MenuManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            HideAllFrames();
            OnePlayer();
            ReglesToMainMenu();
        }

        public void Play()
        {
            _mainMenu.blocksRaycasts = false;
            _mainMenu.interactable = false;
            _audioSource.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                SceneManager.LoadScene("GameScene");
            })).Play();
        }

        private void HideAllFrames()
        {
            _reglesMenu.blocksRaycasts = false;
            _reglesMenu.interactable = false;
            _creditsMenu.blocksRaycasts = false;
            _creditsMenu.interactable = false;
            _mainMenu.blocksRaycasts = true;
            _mainMenu.interactable = true;
        }

        public void Regles()
        {
            _mainMenu.blocksRaycasts = false;
            _mainMenu.interactable = false;
            _mainMenu.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                _reglesMenu.blocksRaycasts = true;
                _reglesMenu.interactable = true;
                _currentPage = 0;
                _reglesMenu.DOFade(1, 1f).OnComplete(new TweenCallback(() =>
                {
                    ShowCurrentPage();
                })).Play();
            })).Play();
        }

        public void Credits()
        {
            _mainMenu.blocksRaycasts = false;
            _mainMenu.interactable = false;
            _mainMenu.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                _creditsMenu.blocksRaycasts = true;
                _creditsMenu.interactable = true;
                _creditsMenu.DOFade(1, 1f).OnComplete(new TweenCallback(() =>
                {
                    ShowCurrentPage();
                })).Play();
            })).Play();
        }

        public void ReglesToMainMenu()
        {
            _reglesMenu.blocksRaycasts = false;
            _reglesMenu.interactable = false;
            _reglesMenu.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                _mainMenu.blocksRaycasts = true;
                _mainMenu.interactable = true;
                _mainMenu.DOFade(1, 1f).Play();
            })).Play();
        }

        public void CreditsToMainMenu()
        {
            _creditsMenu.blocksRaycasts = false;
            _creditsMenu.interactable = false;
            _creditsMenu.DOFade(0, 1f).OnComplete(new TweenCallback(() =>
            {
                _mainMenu.blocksRaycasts = true;
                _mainMenu.interactable = true;
                _mainMenu.DOFade(1, 1f).Play();
            })).Play();
        }

        public void OnePlayer()
        {
            _playerSlots.Items.ElementAt(0).Enable = true;
            _playerSlots.Items.ElementAt(1).Enable = false;
            _1pButton.interactable = false;
            _2pButton.interactable = true;
        }

        public void TwoPlayers()
        {
            _playerSlots.Items.ElementAt(0).Enable = true;
            _playerSlots.Items.ElementAt(1).Enable = true;
            _1pButton.interactable = true;
            _2pButton.interactable = false;
        }

        public void PreviousPage()
        {
            HideCurrentPage();

            _currentPage--;
            if(_currentPage < 0)
            {
                _currentPage = 3;
            }

            ShowCurrentPage();
        }

        public void NextPage()
        {
            HideCurrentPage();

            _currentPage++;
            if (_currentPage > 3)
            {
                _currentPage = 0;
            }

            ShowCurrentPage();
        }

        private void HideCurrentPage()
        {
            var page = _pages[_currentPage];
            page.DOFade(0, 0.5f).Play();
        }

        private void ShowCurrentPage()
        {
            var page = _pages[_currentPage];
            page.DOFade(1, 0.5f).Play();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
