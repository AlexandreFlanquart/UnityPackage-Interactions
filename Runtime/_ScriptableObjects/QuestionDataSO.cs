using UnityEngine;
using I2.Loc;

namespace MyUnityPackage.Toolkit{
    [CreateAssetMenu(fileName = "QuestionDataSO", menuName = "ScriptableObjects/QuestionDataSO", order = 1)]
    public class QuestionDataSO : ScriptableObject
    {
        public LocalizedString question;
        public LocalizedString goodAnswer;
        public LocalizedString badAnswer;
        public LocalizedString feedback;
    }
}
