using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreData : ScriptableObject
{
    [System.Serializable]
	public class Attribute
    {
        public int grade;
        public int weaponAtk;
        public int ArmorDef;
        public int WeaponPrice;
        public float ArmorPrice;

        //ArrayList X -> List
        //Hashtable X -> Dictionary
    }

    public List<Attribute> list = new List<Attribute>();
}
