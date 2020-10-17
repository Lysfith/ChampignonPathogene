using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Spores
{
    public class SporeOnFly : MonoBehaviour
    {
        private const int MAX_SPORE_SHOW = 20;

        [Required] [SerializeField] private GameObject _sporePrefab;
        [Required] [SerializeField] private SporeState _sporeState;
        [Required] [SerializeField] private RectTransform _graphic;
        [Required] [SerializeField] private RectTransform _shadow;
        [Required] [SerializeField] private RectTransform _sporesFolder;

        [Required] [SerializeField] private List<GameObject> _scriptsRequired;

        private float _timeBetweenSporeDepletion = 0.5f;
        private float _lastSporeDepletion = 0;

        public UnityEvent OnSporeDepletion;

        private void OnEnable()
        {
            _sporePrefab.SetActive(false);

            UpdateSpores(_sporeState.NbSpores);

            _graphic.parent.gameObject.SetActive(true);
            _graphic.DOScale(1.5f, 0.5f).Play();
            _shadow.DOScale(0.5f, 0.5f).Play();
            _graphic.DOLocalMove(new Vector3(-65f, 65f, 0), 0.5f).OnComplete(new TweenCallback(() =>
            {
                foreach(var script in _scriptsRequired)
                {
                    script.SetActive(true);
                }
            })).Play();
        }

        private void OnDisable()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            _lastSporeDepletion += Time.deltaTime;

            if (_lastSporeDepletion >= _timeBetweenSporeDepletion)
            {
                OnSporeDepletion?.Invoke();
                UpdateSpores(_sporeState.NbSpores);
                _lastSporeDepletion = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Landing();
            }
        }

        private void UpdateSpores(int amount)
        {
            if (amount > MAX_SPORE_SHOW)
            {
                amount = MAX_SPORE_SHOW;
            }

            var childCount = _sporesFolder.childCount;

            if (childCount < amount)
            {
                var delta = amount - childCount;
                for (int i = 0; i < delta; i++)
                {
                    var go = Instantiate(_sporePrefab);
                    go.transform.SetParent(_sporesFolder, false);
                    var randPosition = Random.insideUnitCircle * 50;
                    var rect = go.GetComponent<RectTransform>();
                    rect.anchoredPosition = randPosition;
                    go.GetComponent<Image>().color = _sporeState.Color;
                    rect.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    var scale = UnityEngine.Random.Range(0.2f, 0.3f);
                    rect.localScale = new Vector3(scale, scale, scale);
                }
            }

            var childs = _sporesFolder.GetComponentsInChildren<RectTransform>();
            var nbVisibles = childs.Where(c => c.gameObject.activeSelf).Count();
            if (nbVisibles != amount)
            {
                for (var i = 0; i < _sporesFolder.childCount; i++)
                {
                    _sporesFolder.GetChild(i).gameObject.SetActive(i <= amount);
                }
            }

        }

        private void Landing()
        {
            if(!_sporeState.CanChangeState())
            {
                return;
            }

            _sporeState.ChangeState();

            foreach (var script in _scriptsRequired)
            {
                script.SetActive(false);
            }

            _graphic.DOLocalMove(new Vector3(0, 0, 0), 0.5f).Play();
            _shadow.DOScale(1f, 0.5f).Play();
            _graphic.DOScale(1f, 0.5f).OnComplete(new TweenCallback(() =>
            {
                _graphic.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            })).Play();
        }
    }
}
