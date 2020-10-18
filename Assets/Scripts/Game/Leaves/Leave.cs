using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Leaves
{
    public class Leave : MonoBehaviour
    {
        [Required] [SerializeField] private RectTransform _rect;
        [Required] [SerializeField] private BoxCollider2D _collider;
        [Required] [SerializeField] private RectTransform _graphic;
        [Required] [SerializeField] private RectTransform _shadow;
        [Required] [SerializeField] private Image _imageLeave;
        [Required] [SerializeField] private Image _imageShadow;

        [Required] [SerializeField] private List<Sprite> _sprites;
        [Required] [SerializeField] private Gradient _gradientColor;
        [Required] [SerializeField] private Gradient _gradientShadow;

        [SerializeField] private bool _isMeleze;
        [SerializeField] private float _state;

        public bool IsMeleze => _isMeleze;
        public float State => _state;
        public RectTransform Rect => _rect;
        public BoxCollider2D Collider => _collider;

        private void OnEnable()
        {
            RandomRotation();
            RandomScale();
        }

        public void SetColorFromPercent(float percent, float percentYear)
        {
            if(percent > 1)
            {
                percent = 1;
            }
            else if (percent < 0)
            {
                percent = 0;
            }

            _state = percent;

            if (_sprites.Any())
            {
                var tranche = 0.5f / _sprites.Count;
                if (percent > 0.5f)
                {
                    var index = (int)((percent - 0.5f) / tranche);
                    var sprite = _sprites.ElementAt(index);

                    _imageLeave.sprite = sprite;
                    _imageShadow.sprite = sprite;
                }

                _imageLeave.color = _gradientColor.Evaluate(percent);
                _imageShadow.color = _gradientShadow.Evaluate(percent);
            }
        }

        public void RandomRotation()
        {
            _graphic.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            _shadow.position = _graphic.position + new Vector3(3, -3, 0);
        }

        public void RandomScale()
        {
            var scale = UnityEngine.Random.Range(3.25f, 3.75f);
            _graphic.localScale = new Vector3(scale, scale, scale);

            _collider.size = new Vector2(_collider.size.x * scale, _collider.size.y * scale);
        }
    }
}
