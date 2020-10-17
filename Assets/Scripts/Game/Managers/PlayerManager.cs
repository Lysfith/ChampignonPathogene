using Assets.Scripts.Game.Leaves;
using Assets.Scripts.Game.SciptableObjects;
using Assets.Scripts.Game.Spores;
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
    public class PlayerManager : MonoBehaviour
    {
        [Required] [SerializeField] private PlayerSlotList _playerSlots;
        [Required] [SerializeField] private GameObject _playerPrefab;
        [Required] [SerializeField] private List<GameObject> _players;
        [Required] [SerializeField] private RectTransform _playerFolder;

        private static PlayerManager _instance;

        public static PlayerManager Instance => _instance;

        void Awake()
        {
            _instance = this;

            _playerPrefab.SetActive(false);
            _players = new List<GameObject>();
        }

        public void SetPlayerStart(List<Leave> leaves)
        {
            for(int i = 0; i < leaves.Count; i++)
            {
                var slot = _playerSlots.Items.ElementAt(i);
                if (!slot.Enable)
                {
                    continue;
                }

                var leave = leaves.ElementAt(i);
                var go = Instantiate(_playerPrefab);
                go.transform.SetParent(leave.Rect, false);
                var state = go.GetComponentInChildren<SporeState>();
                state.SetPlayerFolder(_playerFolder);
                state.SetPlayerColor(slot.Color);

                var input = go.GetComponentInChildren<SporeInput>();
                input.SetInputBinder(slot.Input);

                _players.Add(go);

                go.SetActive(true);
            }
        }
    }
}
