using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Managers
{
    public class FloatingTextManager : MonoBehaviour
    {
        [Required] [SerializeField] private GameObject _floatingTextPrefab;
        [Required] [SerializeField] private RectTransform _folder;

        private static FloatingTextManager _instance;

        public static FloatingTextManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        public void NewText(string text, Vector2 position, Color color)
        {
            var go = Instantiate(_floatingTextPrefab);
            go.transform.SetParent(_folder);
            go.transform.position = position;
            var textC = go.GetComponent<TextMeshProUGUI>();
            textC.text = text;
            textC.color = color;
        }
    }
}
