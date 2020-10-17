using Sirenix.OdinInspector;
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
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.8f;
        [Required][SerializeField] private RawImage _imageSpring;
        [Required][SerializeField] private RawImage _imageWinter;
        [Required][SerializeField] private RectTransform _rectWinter;

        private static BackgroundManager _instance;

        public static BackgroundManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            var offset = ScrollingManager.Instance.GetOffset();

            var amount = offset / 1300f;

            _imageSpring.material.SetTextureOffset("_BaseMap", new Vector2(0, -amount));
            _imageWinter.material.SetTextureOffset("_BaseMap", new Vector2(0, -amount));

            UpdateWinter();
        }

        private void UpdateWinter()
        {
            var percent = SeasonManager.Instance.GetYearPercent();

            var y = 1080 - 2160 * percent;
            _rectWinter.anchoredPosition = new Vector2(0, y);
        }
    }
}
