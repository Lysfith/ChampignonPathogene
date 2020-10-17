using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Common.Inputs
{
    [CreateAssetMenu(fileName = "GamepadBind", menuName = "ScriptableObjects/Common/GamepadBind", order = 1)]
    public class GamepadBind : BaseInputBind
    {
        private Gamepad _gamepad;

        public void SetGamepad(Gamepad gamepad)
        {
            _gamepad = gamepad;
        }

        public override float GetVerticalValue()
        {
            return _gamepad.leftStick.y.ReadValue();
        }

        public override float GetHorizontalValue()
        {
            return _gamepad.leftStick.x.ReadValue();
        }
    }
}
