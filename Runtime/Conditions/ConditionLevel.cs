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

        private GameManager gameManager;

        public void Start()
        {
            gameManager = GameManager.GetInstance();//ServiceLocator.GetService<GameManager>();
            gameManager.OnLevelChange += OnLevelChange;
        }


        public override bool CheckCondition()
        {
            bool condition = _level >= 10;
            Debug.Log("condition : " + condition);
            return condition;
        }

        private void OnLevelChange(int lvl)
        {
            Level = lvl;
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
