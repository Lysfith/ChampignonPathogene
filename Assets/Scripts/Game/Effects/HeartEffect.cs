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
    public class HeartEffect : MonoBehaviour
    {
        [Required] [SerializeField] private GameObject _particlePrefab;

        private void OnEnable()
        {
            _particlePrefab.SetActive(false);

            var rbs = new List<Rigidbody2D>();
            for(int i = 0; i < 10; i++)
            {
                var go = Instantiate(_particlePrefab);
                go.transform.SetParent(transform.parent, false);
                go.transform.position = transform.position;
                rbs.Add(go.GetComponent<Rigidbody2D>());
            }

            foreach(var rb in rbs)
            {
                var randX = UnityEngine.Random.Range(-0.5f, 0.5f);
                var randY = UnityEngine.Random.Range(0.2f, 1f);
                rb.gameObject.SetActive(true);
                rb.AddForce(new Vector2(randX, randY * 2) * 100, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
