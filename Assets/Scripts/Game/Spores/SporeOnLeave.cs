using Assets.Scripts.Game.Leaves;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Spores
{
    public class SporeOnLeave : MonoBehaviour
    {
        [Required][SerializeField] private SporeState _sporeState;
        [Required] [SerializeField] private Image _graphic;

        [Required] [SerializeField] private List<GameObject> _scriptsRequired;


        private float _timeBetweenSporeGeneration = 0.5f;
        private float _lastSporeGeneration = 0;

        public UnityEvent OnSporeGeneration;

        private void OnEnable()
        {
            _graphic.color = _sporeState.Color;
            foreach (var script in _scriptsRequired)
            {
                script.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_sporeState.Leave.IsMeleze || _sporeState.Leave.State > 0.6f)
            {
                return;
            }

            _lastSporeGeneration += Time.deltaTime;

            if(_lastSporeGeneration >= _timeBetweenSporeGeneration)
            {
                OnSporeGeneration?.Invoke();

                _lastSporeGeneration = 0;
            }

            
        }

       
        public void Explode()
        {
            _sporeState.ChangeState();

            foreach (var script in _scriptsRequired)
            {
                script.SetActive(false);
            }

            //foreach (var spore in spores)
            //{
            //    var pos3D = Random.onUnitSphere;
            //    var randPosition = new Vector2(pos3D.x, pos3D.y);
            //    randPosition = randPosition.normalized;
            //    spore.transform.SetParent(null);
            //    spore.SetActive(true);
            //    var body = spore.GetComponent<Rigidbody2D>();
            //    body.AddForce(randPosition * 10, ForceMode2D.Impulse);
            //}

            _lastSporeGeneration = 0;

            gameObject.SetActive(false);
        }
    }
}
