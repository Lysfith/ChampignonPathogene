using Assets.Scripts.Common;
using Assets.Scripts.Game.Leaves;
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
    public class LeavesManager : MonoBehaviour
    {
        private const int MIN_LEAVES_ON_LINE = 3;
        private const int MAX_LEAVES_ON_LINE = 4;
        private const int SCREEN_MARGE = 50;
        private const int SCREEN_LINE_OFFSET = 600;
        private const int SCREEN_LINE_START = -500;
        private const int SCREEN_LINE_MIDDLE = -500;
        private const int SCREEN_WIDTH = 1280;
        private const int SCREEN_HEIGHT = 720;

        [SerializeField] private List<Leave> _leaves;
        [Required][SerializeField] private GameObject _leafPrefab;
        [Required][SerializeField] private GameObject _leafMelezePrefab;
        [Required][SerializeField] private RectTransform _leavesFolder;
        [Required][SerializeField] private Canvas _canvas;

        private int _currentLineOffset = 0;
        private float _lastOffset = 0;
        private bool _isRunning;

        public UnityEventListLeave OnLeavesStartReady;

        private static LeavesManager _instance;

        public static LeavesManager Instance => _instance;

        void Awake()
        {
            _instance = this;

            _leaves = new List<Leave>();

            _leafPrefab.SetActive(false);
            _leafMelezePrefab.SetActive(false);
        }

        public void StartLevel()
        {
            StartCoroutine(GenerateLeavesForStart(SCREEN_LINE_START, 4));
            _isRunning = true;
        }

        public void StopLevel()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if(!_isRunning)
            {
                return;
            }

            var rect = _canvas.pixelRect;
            //var leavesToDestroy = new List<RectTransform>();

            var leavesToDestroy = _leaves.Where(l => l.Rect.anchoredPosition.y > 700).ToList();

            foreach (var leave in leavesToDestroy)
            {
                _leaves.Remove(leave);
            }

            if (leavesToDestroy.Any())
            {
                StartCoroutine(DestroyLeaves(leavesToDestroy));
            }

            var offset = ScrollingManager.Instance.GetOffset();

            if(offset >= _currentLineOffset + SCREEN_LINE_OFFSET)
            {
                _currentLineOffset += SCREEN_LINE_OFFSET;
                StartCoroutine(GenerateLeavesOnLine(SCREEN_LINE_MIDDLE));
            }

            var deltaOffset = offset - _lastOffset;

            if(deltaOffset > 0)
            {
                foreach (var leave in _leaves.ToList())
                {
                    leave.Rect.anchoredPosition += new Vector2(0, deltaOffset);
                }
                _lastOffset = offset;
            }

            var percentYear = SeasonManager.Instance.GetYearPercent();
            foreach (var leave in _leaves.ToList())
            {
                var percent = (leave.Rect.anchoredPosition.y + SCREEN_HEIGHT / 2f) / (float)SCREEN_HEIGHT;
                leave.SetColorFromPercent(percent, percentYear);
            }
        }

        public Leave HasLeaveAtPosition(Vector2 position)
        {
            foreach (var leave in _leaves.ToList())
            {
                if(leave.Collider.OverlapPoint(position))
                {
                    return leave;
                }
            }

            return null;
        }

        private IEnumerator GenerateLeavesOnLine(int y)
        {
            //var randLeaves = UnityEngine.Random.Range(MIN_LEAVES_ON_LINE, MAX_LEAVES_ON_LINE);

           
            var percentYear = SeasonManager.Instance.GetYearPercent();

            var prefabList = new List<GameObject>();

            if(percentYear < 0.25f || percentYear > 0.75f)
            {
                prefabList.Add(_leafPrefab);
                prefabList.Add(_leafPrefab);
                prefabList.Add(_leafPrefab);
            }
            else if(percentYear < 0.40f || percentYear > 0.60f)
            {
                prefabList.Add(_leafPrefab);
                prefabList.Add(_leafMelezePrefab);
                prefabList.Add(_leafPrefab);
            }
            else
            {
                prefabList.Add(_leafMelezePrefab);
            }

            prefabList = prefabList.OrderBy(a => Guid.NewGuid()).ToList();

            var width = SCREEN_WIDTH - SCREEN_MARGE * 4;
            var widthPart = width / prefabList.Count;

            for (int i = 0; i < prefabList.Count; i++)
            {
                var posX = -SCREEN_WIDTH / 2f + SCREEN_MARGE * 2 + widthPart * i;
                var randX = UnityEngine.Random.Range(100, widthPart - 100);
                var posY = y + UnityEngine.Random.Range(-200, 200);
                posX += randX;

                var go = Instantiate(prefabList.ElementAt(i), _leavesFolder);
                var rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(posX, posY);
                //rect.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                //var scale = UnityEngine.Random.Range(1.2f, 1.6f);
                //rect.localScale = new Vector3(scale, scale, scale);
                _leaves.Add(rect.GetComponent<Leave>());
                go.SetActive(true);

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator GenerateLeavesForStart(int y, int nbPlayers)
        {
            var width = SCREEN_WIDTH - SCREEN_MARGE * 2;
            var widthPart = width / nbPlayers;

            for (int i = 0; i < nbPlayers; i++)
            {
                var posX = -SCREEN_WIDTH / 2f + SCREEN_MARGE * 1 + widthPart * i + widthPart/2f;
                var posY = y;

                var go = Instantiate(_leafPrefab, _leavesFolder);
                var rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(posX, posY);
                //rect.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
               
                _leaves.Add(rect.GetComponent<Leave>());
                go.SetActive(true);

                yield return new WaitForEndOfFrame();
            }

            OnLeavesStartReady?.Invoke(_leaves);
        }

        private IEnumerator DestroyLeaves(List<Leave> leaves)
        {
            foreach (var leave in leaves)
            {
                yield return new WaitForEndOfFrame();
                Destroy(leave.gameObject);
            }
        }
    }
}
