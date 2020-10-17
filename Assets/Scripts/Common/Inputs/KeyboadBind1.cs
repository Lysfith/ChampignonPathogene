using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common.Inputs
{
    [CreateAssetMenu(fileName = "KeyboadBind1", menuName = "ScriptableObjects/Common/KeyboadBind1", order = 1)]
    public class KeyboadBind1 : BaseInputBind
    {
        public override float GetVerticalValue()
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                return -1;
            }
            else if(Input.GetKey(KeyCode.UpArrow))
            {
                return 1;
            }
            return 0;
        }

        public override float GetHorizontalValue()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                return 1;
            }
            return 0;
        }
    }
}
