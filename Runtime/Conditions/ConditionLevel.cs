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
                OnConditionMet(_level >= 10);
            }
        }

        public override bool CheckCondition()
        {
            bool condition = _level >= 10;
            Debug.Log("condition : " + condition);
            return condition;
        }

        [Button]
        private void AddLevel()
        {
            Level = 100;
        }
        [Button]
        private void RemoveLevel()
        {
            Level = 0;
        }
    }
}
