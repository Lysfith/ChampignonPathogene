using Assets.Scripts.Common;
using Assets.Scripts.Common.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.SciptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSlot", menuName = "ScriptableObjects/Common/PlayerSlot", order = 1)]
    public class PlayerSlot : ScriptableObject
    {
        public int Index;
        public bool Enable;
        public BaseInputBind Input;
        public Color Color;
    }
}
