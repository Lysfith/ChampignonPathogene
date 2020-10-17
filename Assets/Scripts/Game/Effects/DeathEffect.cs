using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Effects
{
    public class DeathEffect : MonoBehaviour
    {
        [Required] [SerializeField] private GameObject _particlePrefab;

        private Color _color;

        private void OnEnable()
        {
            _particlePrefab.SetActive(false);

            var rbs = new List<Rigidbody2D>();
            for(int i = 0; i < 10; i++)
            {
                var go = Instantiate(_particlePrefab);
                go.transform.SetParent(transform.parent, false);
                go.transform.position = transform.position;
                var sprite = go.GetComponent<Image>();
                sprite.color = _color;
                rbs.Add(go.GetComponent<Rigidbody2D>());
            }

            foreach(var rb in rbs)
            {
                rb.gameObject.SetActive(true);
                rb.AddForce(UnityEngine.Random.insideUnitCircle.normalized * 100, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }

        public void SetColor(Color color)
        {
            _color = color;
        }
    }
}
