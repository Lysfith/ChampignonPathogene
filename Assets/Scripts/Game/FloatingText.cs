using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class FloatingText : MonoBehaviour
    {
        [Required] [SerializeField] private TextMeshProUGUI _text;
        [Required] [SerializeField] private Rigidbody2D _body;

        private void OnEnable()
        {
            _body.AddForce((UnityEngine.Random.insideUnitCircle.normalized + new Vector2(0, 1)) * 100, ForceMode2D.Impulse);

            var sequence = DOTween.Sequence();
            sequence = sequence.AppendInterval(1);
            sequence = sequence.Append(_text.DOFade(0, 1f));
            sequence = sequence.OnComplete(new TweenCallback(() =>
            {
                Destroy(gameObject);
            }));

            sequence.Play();
        }
    }
}
