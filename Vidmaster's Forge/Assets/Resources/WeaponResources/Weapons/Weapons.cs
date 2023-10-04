using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;
using System.Collections.Generic;

namespace Resources.WeaponResources
{
    [CreateAssetMenu(menuName = "ScriptableObject Collection/Collections/Create Weapons", fileName = "Weapons", order = 0)]
    public class Weapons : ScriptableObjectCollection<Weapon>
    {
    }
}
