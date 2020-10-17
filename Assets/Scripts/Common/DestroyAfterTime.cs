using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float _time;

        private void OnEnable()
        {
            StartCoroutine(WaitDestroy());
        }

        private IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(_time);

            Destroy(gameObject);
        }
    }
}
