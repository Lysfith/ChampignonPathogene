using Assets.Scripts.Common;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Spores
{
    public class SporeUI : MonoBehaviour
    {
        [Required] [SerializeField] private Text _sporeAmountTextOnLeave;
        [Required] [SerializeField] private Text _sporeAmountTextOnFly;

        [Required] [SerializeField] private SporeState _sporeState;

        private void OnEnable()
        {
            _sporeState.OnNbSporeChange += Event_OnNbSporeChange;
        }

        private void OnDisable()
        {
            _sporeState.OnNbSporeChange -= Event_OnNbSporeChange;
        }

        public void SetSporeAmount(int amount)
        {
            _sporeAmountTextOnLeave.text = amount.ToString();
            _sporeAmountTextOnFly.text = amount.ToString();
        }

        private void Event_OnNbSporeChange(object sender, int e)
        {
            SetSporeAmount(e);
        }


    }
}
