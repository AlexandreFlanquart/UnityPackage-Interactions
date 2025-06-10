using UnityEngine;


namespace MyUnityPackage.Interactions
{
    public class ConditionLevel : ACondition
    {


        [SerializeField] private int level;
        public override bool CheckCondition()
        {
            Debug.Log("In Level " + level);
            return level >= 10;
        }
    }
}
