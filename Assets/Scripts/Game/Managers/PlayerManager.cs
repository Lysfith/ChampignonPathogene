using Assets.Scripts.Game.Effects;
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
        [Required] [SerializeField] private List<SporeState> _players;
        [Required] [SerializeField] private RectTransform _playerFolder;
        [Required] [SerializeField] private BoxCollider2D _borderCollider;
        [Required] [SerializeField] private GameObject _deathEffectPrefab;

        private PlayerSlot _slot1;
        private PlayerSlot _slot2;
        private bool _checkCollider;

        private static PlayerManager _instance;

        public static PlayerManager Instance => _instance;

        void Awake()
        {
            _instance = this;

            _playerPrefab.SetActive(false);
            _players = new List<SporeState>();

            _slot1 = _playerSlots.Items.ElementAt(0);
            _slot2 = _playerSlots.Items.ElementAt(1);

            _slot1.Alive = true;
            _slot2.Alive = _slot2.Enable;

            _deathEffectPrefab.SetActive(false);
        }

        private void Update()
        {
            if (!_players.Any())
            {
                return;
            }

            if (_checkCollider)
            {
                foreach (var player in _players.ToList())
                {
                    if (!_borderCollider.OverlapPoint(player.transform.position))
                    {
                        player.Death();
                    }
                }
            }

            if(!_slot2.Enable)
            {
                return;
            }

            if(!_slot1.Alive || !_slot2.Alive)
            {
                return;
            }

            var player1 = _players.ElementAt(0);
            var player2 = _players.ElementAt(1);

            if (player1.IsOnLeave && player2.IsOnLeave
                && player1.Leave == player2.Leave
                && player1.Leave.IsMeleze)
            {
                var total = player1.NbSpores + player2.NbSpores;
                if(total % 2 == 1)
                {
                    total -= 1;
                }

                player1.SetNbSpores(total / 2);
                player2.SetNbSpores(total / 2);
            }
        }

        public void SetPlayerStart(List<Leave> leaves)
        {
            leaves = leaves.OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < leaves.Count; i++)
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
                state.SetLeave(leave);

                state.OnDeath += (s, e) =>
                {
                    _players.Remove(state);
                    slot.Alive = false;
                    StartCoroutine(StartDeathAnimation(go.transform.position, slot.Color));
                    Destroy(go);
                };

                var input = go.GetComponentInChildren<SporeInput>();
                input.SetInputBinder(slot.Input);

                _players.Add(state);

                go.SetActive(true);
            }

            StartCoroutine(WaitStart());
        }

        private IEnumerator WaitStart()
        {
            yield return new WaitForSeconds(2);

            _checkCollider = true;
        }

        private IEnumerator StartDeathAnimation(Vector2 position, Color color)
        {
            yield return new WaitForEndOfFrame();

            var go = Instantiate(_deathEffectPrefab);
            go.transform.SetParent(_playerFolder, false);
            go.transform.position = position;
            var script = go.GetComponent<DeathEffect>();
            script.SetColor(color);
            go.SetActive(true);
        }
    }
}
