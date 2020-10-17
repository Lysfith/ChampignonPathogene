using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.SciptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSlotList", menuName = "ScriptableObjects/Common/PlayerSlotList", order = 1)]
    public class PlayerSlotList : ScriptableObject
    {
        public List<PlayerSlot> Items;
    }
}
