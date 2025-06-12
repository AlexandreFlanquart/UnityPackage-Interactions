using Sirenix.OdinInspector;
using UnityEngine;


namespace MyUnityPackage.Interactions
{
    public class ConditionLevel : ACondition
    {
        [SerializeField] private int _level;

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnConditionMet(true);
            }
        }

        public override bool CheckCondition()
        {
            bool condition = _level >= 10;
            Debug.Log("condition : " + condition);
            return condition;
        }

        [Button]
        private void ChangeLevel()
        {
            Level = 100000;
        }
    }
}
