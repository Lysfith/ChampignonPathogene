﻿using Assets.Scripts.Game.Effects;
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
        [Required] [SerializeField] private AudioSource _audioSourceDeath;
        [Required] [SerializeField] private AudioSource _audioSourceReproduction;
        [Required] [SerializeField] private AudioSource _audioSourceCompetition;
        [Required] [SerializeField] private PlayerSlotList _playerSlots;
        [Required] [SerializeField] private GameObject _playerPrefab;
        [Required] [SerializeField] private List<SporeState> _players;
        [Required] [SerializeField] private RectTransform _playerFolder;
        [Required] [SerializeField] private BoxCollider2D _borderCollider;
        [Required] [SerializeField] private GameObject _deathEffectPrefab;
        [Required] [SerializeField] private GameObject _heartEffectPrefab;

        private PlayerSlot _slot1;
        private PlayerSlot _slot2;
        private bool _checkCollider;

        private float _timeBetweenReproduction = 1f;
        private float _lastReproduction = 0f;

        private float _timeBetweenCompetition = 0.2f;
        private float _lastCompetition = 0f;

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
            _heartEffectPrefab.SetActive(false);
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

            CheckReproduction();
            CheckCompetition();
        }

        private void CheckReproduction()
        {
            _lastReproduction += Time.deltaTime;

            if (_lastReproduction < _timeBetweenReproduction)
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
                if (total % 2 == 1)
                {
                    total -= 1;
                }

                player1.SetNbSpores(total / 2);
                player2.SetNbSpores(total / 2);

                var middle = player1.transform.position + (player2.transform.position - player1.transform.position) / 2f;

                var go = Instantiate(_heartEffectPrefab);
                go.transform.SetParent(_playerFolder, false);
                go.transform.position = middle;
                go.SetActive(true);

                _audioSourceReproduction.Play();

                _lastReproduction = 0;
            }
        }

        private void CheckCompetition()
        {
            _lastCompetition += Time.deltaTime;

            if (_lastCompetition < _timeBetweenCompetition)
            {
                return;
            }

            var player1 = _players.ElementAt(0);
            var player2 = _players.ElementAt(1);

            if (!player1.IsOnLeave && !player2.IsOnLeave)
            {
                var distance = Vector3.Distance(player1.transform.position, player2.transform.position);

                if(distance > 100)
                {
                    return;
                }

                var deltaY = player2.transform.position.y - player1.transform.position.y;
                
                if(Mathf.Abs(deltaY) < 40f)
                {
                    return;
                }
                Debug.Log(deltaY);
                if (player2.transform.position.y > player1.transform.position.y)
                {
                    player1.RemoveNbSpores(1);
                }
                else
                {
                    player2.RemoveNbSpores(1);
                }

                //var middle = player1.transform.position + (player2.transform.position - player1.transform.position) / 2f;

                //var go = Instantiate(_heartEffectPrefab);
                //go.transform.SetParent(_playerFolder, false);
                //go.transform.position = middle;
                //go.SetActive(true);

                _audioSourceCompetition.Play();

                _lastCompetition = 0;
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

                    if(!_slot2.Enable && !_slot1.Alive)
                    {
                        LevelManager.Instance.StopLevel(null);
                    }

                    if (_slot2.Enable && (!_slot1.Alive || !_slot2.Alive))
                    {
                        LevelManager.Instance.StopLevel(_slot1.Alive ? "1" : "2");
                    }
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

            _audioSourceDeath.Play();
        }
    }
}
