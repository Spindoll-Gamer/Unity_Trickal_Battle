using UnityEngine;
using System.Collections.Generic;

public class DamageTextPool : MonoBehaviour
{
    public DamageText prefab;
    public int initialCount = 20; // 처음에 미리 만들어둘 개수
    public Transform canvasTransform;

    public static DamageTextPool Instance { get; private set; }

    // 핵심 자료구조: 큐(Queue)
    // FIFO(먼저 들어온 놈이 먼저 나감) 성질이 재사용에 적합합니다.
    private Queue<DamageText> pool = new Queue<DamageText>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        // 1. 미리 생성해서 큐에 넣어두기 (Warm-up)
        for (int i = 0; i < initialCount; i++)
        {
            CreateNewInstance();
        }
    }

    private void CreateNewInstance()
    {
        DamageText obj = Instantiate(prefab, canvasTransform);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    // 풀에서 빌려오기
    public void GetFromPool(float damage, Vector3 position)
    {
        if (pool.Count == 0) // 모자라면 새로 하나 만듦
        {
            CreateNewInstance();
        }

        DamageText obj = pool.Dequeue();
        obj.transform.position = position;
        obj.gameObject.SetActive(true);
        obj.Setup(damage, this);
    }

    // 풀로 반납하기
    public void ReturnToPool(DamageText obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public void ReturnAllToPool()
    {
        foreach (var obj in pool)
        {
            if( obj.gameObject.activeSelf)
            { obj.gameObject.SetActive(false); }
        }
    }
}