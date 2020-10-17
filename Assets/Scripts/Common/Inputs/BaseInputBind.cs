using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Common.Inputs
{

    public abstract class BaseInputBind : ScriptableObject, IInputBinder
    {

        public virtual float GetVerticalValue()
        {
            return 0;
        }

        public virtual float GetHorizontalValue()
        {
            return 0;
        }
    }
}
