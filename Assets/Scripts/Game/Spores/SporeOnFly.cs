using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.Spores
{
    public class SporeOnFly : MonoBehaviour
    {
        [Required] [SerializeField] private SporeState _sporeState;
        [Required] [SerializeField] private RectTransform _graphic;
        [Required] [SerializeField] private RectTransform _shadow;

        private void OnEnable()
        {
            _graphic.DOScale(1.5f, 0.5f).Play();
            _shadow.DOScale(0.5f, 0.5f).Play();
            _graphic.DOLocalMove(new Vector3(-65f, 65f, 0), 0.5f).Play();
        }

        private void OnDisable()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Landing();
            }
        }

        private void Landing()
        {
            _graphic.DOLocalMove(new Vector3(0, 0, 0), 0.5f).Play();
            _shadow.DOScale(1f, 0.5f).Play();
            _graphic.DOScale(1f, 0.5f).OnComplete(new TweenCallback(() =>
            {
                _sporeState.ChangeState();
            })).Play();
        }
    }
}
