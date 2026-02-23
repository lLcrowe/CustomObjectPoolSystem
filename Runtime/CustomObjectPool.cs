using lLCroweTool.DestroyManger;
using System.Collections.Generic;
using UnityEngine;

namespace lLCroweTool.ObjectPool
{
    /// <summary>
    /// 커스텀오브젝트폴
    /// </summary>
    /// <typeparam name="T">모노비헤이비어 상속한 타입</typeparam>
    //[System.Serializable]
    public abstract class CustomObjectPool<T> where T : Component
    {
        [Header("타겟팅될 프리팹")]
        [SerializeField] protected T objectPrefab = null;//타겟팅될 프리팹
        //[SerializeField] private List<T> objectPoolList = new List<T>();
        [SerializeField] private Queue<T> objectPoolList = new Queue<T>();
        [SerializeField] protected int size = 50;

        /// <summary>
        /// 커스텀오브젝트폴 생성자. 프리팹을 세팅할것
        /// </summary>
        /// <param name="targetPrefab">세팅할 프리팹</param>
        //public CustomObjectPool(T targetPrefab)
        //{
        //    SetPrefab(targetPrefab);
        //}

        /// <summary>
        /// 프리팹세팅해주는 함수//생성자 만들때 해줘야됨
        /// </summary>
        /// <param name="targetPrefab">타겟팅할 프리팹</param>
        public void SetPrefab(T targetPrefab)
        {
            objectPrefab = targetPrefab;
        }

        /// <summary>
        /// 적용한 프리팹을 가져오는 함수
        /// </summary>
        /// <returns>적용한 프리팹</returns>
        public T GetPrefab()
        {
            return objectPrefab;
        }

        /// <summary>
        /// 프리팹을 요청하는 함수
        /// </summary>
        /// <returns>프리팹</returns>
        public T RequestPrefab(int instanceID)
        {
            return RequestPrefab(this, instanceID);
        }

        /// <summary>
        /// 프리팹을 요청하는 함수
        /// </summary>
        /// <returns>프리팹</returns>
        private static T RequestPrefab(CustomObjectPool<T> customObjectPool, int instanceID)
        {
            //초기화
            //bool isFind = false;
            T targetObject = null;

            //로직작동
            var objectPoolList = customObjectPool.objectPoolList;

            //큐형
            if (objectPoolList.Count > 0)
            {
                //isFind = true;
                targetObject = objectPoolList.Dequeue();
            }
            else
            {
                //찾은게 없다면 오브젝트 하나를 만들어준다.
                targetObject = Object.Instantiate(customObjectPool.objectPrefab);
                targetObject.name = customObjectPool.objectPrefab.name;//아이디지정
                targetObject.GetAddComponent<CustomPoolTarget>().SetPoolTargetComponent(targetObject, instanceID);//컴포넌트지정
            }
            targetObject.SetActive(true);//무조건 키는걸로 변경
            return targetObject;
        }

        /// <summary>
        /// 커스텀오브젝트폴을 클리어해주는 함수
        /// </summary>
        public void ClearCustomObjectPool()
        {
            ClearCustomObjectPool(this);
        }

        /// <summary>
        /// 커스텀오브젝트폴을 클리어해주는 함수
        /// </summary>
        /// <param name="customObjectPool">대상이 될 커스텀폴</param>
        private static void ClearCustomObjectPool(CustomObjectPool<T> customObjectPool)
        {
            var objectPoolList = customObjectPool.objectPoolList;

            //큐
            int count = objectPoolList.Count;
            for (int i = 0; i < count; i++)
            {
                var temp = objectPoolList.Dequeue();
                DestroyManager.Instance.AddDestoryGameObject(temp.gameObject);
            }
            objectPoolList.Clear();
        }

        /// <summary>
        /// 반납함수
        /// </summary>
        /// <param name="targetObject">폴에 배정된 오브젝트를 반납하는 함수</param>
        public void ReturnPrefab(T targetObject)
        {
            objectPoolList.Enqueue(targetObject);
        }

        /// <summary>
        /// 로딩창에 들어갔을시 사이즈에 맞게 프리팹을 처리하는 함수
        /// </summary>
        /// <param name="customObjectPool">타겟팅될 커스텀오브젝트폴</param>
        public static void LoadToDecreasePrefab(CustomObjectPool<T> customObjectPool)
        {
            //사이즈처리를 로딩창에서만 처리하게 제작
            int curSize = customObjectPool.objectPoolList.Count;//현재 사이즈
            int targetSize = customObjectPool.size;//해당 사이즈까지 내려가야됨

            //큐
            while (customObjectPool.objectPoolList.Count > targetSize)
            {
                var target = customObjectPool.objectPoolList.Dequeue();
                DestroyManager.Instance.AddDestoryGameObject(target.gameObject);
            }
        }
    }
}
