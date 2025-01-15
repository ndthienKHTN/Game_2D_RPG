using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Common.Scripts.UI.Model.ItemParameters
{
    [CreateAssetMenu]
    public class ItemParameterSO : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName { get; private set; }
    }
}
