using Assets.Scripts.Game.SciptableObjects;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        

        private static LevelManager _instance;

        public static LevelManager Instance => _instance;

        void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            LeavesManager.Instance.StartLevel();
            ScrollingManager.Instance.StartLevel();
            SeasonManager.Instance.StartLevel();
        }

        private void Update()
        {
            
        }
    }
}
