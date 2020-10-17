using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Spores
{
    public class SporeInput : MonoBehaviour
    {
        private IInputBinder _inputBind;

        public void SetInputBinder(IInputBinder inputBind)
        {
            _inputBind = inputBind;
        }

        public IInputBinder GetInputBind()
        {
            if(!gameObject.activeSelf)
            {
                return null;
            }

            return _inputBind;
        }
    }
}
