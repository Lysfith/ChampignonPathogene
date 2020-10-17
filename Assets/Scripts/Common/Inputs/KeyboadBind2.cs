using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Inputs
{
    [CreateAssetMenu(fileName = "KeyboadBind2", menuName = "ScriptableObjects/Common/KeyboadBind2", order = 1)]
    public class KeyboadBind2 : BaseInputBind
    {
        public override float GetVerticalValue()
        {
            if (Input.GetKey(KeyCode.S))
            {
                return -1;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                return 1;
            }
            return 0;
        }

        public override float GetHorizontalValue()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                return -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                return 1;
            }
            return 0;
        }
    }
}
