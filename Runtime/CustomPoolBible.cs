using lLCroweTool.Dictionary;
using lLCroweTool.ObjectPool;
using UnityEngine;

namespace lLCroweTool.PoolBible
{
    /// <summary>
    /// 컴포넌트 폴
    /// </summary>
    [System.Serializable]
    public class CustomPool : CustomObjectPool<Component> { }

    /// <summary>
    /// 컴포넌트 폴 바이블
    /// </summary>
    //public class CustomPoolBible<T> : CustomDictionary<string, CustomPool> where T : Component
    public class CustomPoolBible<T> : CustomDictionary<int, CustomPool> where T : Component
    {
        public T RequestPrefab(T component)
        {
            T target = null;
            //string id = component.name;
            int id = component.GetInstanceID();


            if (!TryGetValue(id, out var pool))
            {
                pool = new CustomPool();
                pool.SetPrefab(component);
                Add(id, pool);
            }
            target = pool.RequestPrefab(id) as T;

            return target;
        }

        //public void ReturnPrefab(T component)
        public void ReturnPrefab(T component, int instaceID)
        {
            string id = component.name;


            if (!TryGetValue(instaceID, out var pool))
            {
                //등록된 폴이 없으면 새로생성해서 등록후 추가
                pool = new CustomPool();
                T originPrefab = Object.Instantiate(component);
                originPrefab.gameObject.SetActive(false);
                originPrefab.name = id;
                pool.SetPrefab(originPrefab);

                component.GetAddComponent<CustomPoolTarget>().SetPoolTargetComponent(component, instaceID);
                Add(instaceID, pool);
            }

            //폴이 있으면 해당 폴에 반납
            pool.ReturnPrefab(component);
            component.gameObject.SetActive(false);
        }

        /// <summary>
        /// 등록된 폴을 리셋해주는 함수
        /// </summary>
        public void ResetCustomPoolBible()
        {
            foreach (var item in this)
            {
                var customPool = item.Value;
                customPool.ClearCustomObjectPool();
            }
        }
    }
}
