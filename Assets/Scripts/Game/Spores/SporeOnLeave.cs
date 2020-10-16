using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Spores
{
    public class SporeOnLeave : MonoBehaviour
    {
        [Required][SerializeField] private SporeState _sporeState;
        [SerializeField] private int _spores;

        private float _timeBetweenSporeGeneration = 0.5f;
        private float _lastSporeGeneration = 0;

        public UnityEvent OnSporeGeneration;

        private void OnEnable()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _lastSporeGeneration += Time.deltaTime;

            if(_lastSporeGeneration >= _timeBetweenSporeGeneration)
            {
                _spores++;

                OnSporeGeneration?.Invoke();

                _lastSporeGeneration = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Explode();
            }
        }

        private void Explode()
        {
            _sporeState.ChangeState();
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


            _spores = 0;
            _lastSporeGeneration = 0;
        }
    }
}
