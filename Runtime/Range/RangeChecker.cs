using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class RangeChecker : MonoBehaviour
    {
        static RangeChecker instance;
        [SerializeField] private Transform player;
        [SerializeField] private float checkFrequency = 0.1f;

        private Vector3 currentPlayerPos;
        private List<RangeHandler> rangeHandlers = new List<RangeHandler>();

        public bool forceCheck = false;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCalculating();
        }

        public static RangeChecker GetInstance()
        {
            return instance;
        }

        public void StartCalculating()
        {
            StartCoroutine(Calculating());
        }

        public void StopCalculating()
        {
            StopAllCoroutines();
        }

        private IEnumerator Calculating()
        {
            while (true)
            {
                CalculateRange();
                yield return new WaitForSeconds(checkFrequency);
            }
        }

        public void AddRangeElement(RangeHandler rangeHandler)
        {
            if (!rangeHandlers.Contains(rangeHandler))
            {
                rangeHandlers.Add(rangeHandler);
                CalculateRange(rangeHandler);
            }
        }

        public void RemoveRangeElement(RangeHandler rangeHandler)
        {
            if (rangeHandlers.Contains(rangeHandler))
            {
                rangeHandlers.Remove(rangeHandler);
            }
        }

        private void CalculateRange()
        {
            if (!player.gameObject.activeInHierarchy) return;
            if ((player != null && currentPlayerPos != player.position) || forceCheck)
            {
                forceCheck = false;
                for (int i = 0; i < rangeHandlers.Count; i++)
                {
                    CalculateRange(rangeHandlers[i]);
                }
            }
        }

        public void CalculateRange(RangeHandler pRangeHandler)
        {
            currentPlayerPos = player.position;
            float dist = Vector3.Distance(player.position, pRangeHandler.Center.position);
            pRangeHandler.CheckRange(dist);
        }
    }
}