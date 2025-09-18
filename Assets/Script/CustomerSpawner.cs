using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public ServeSystem serveSystem;
    public CustomerController customerPrefab;
    public Transform spawnPoint;
    public Transform seatPoint;
    public Transform exitPoint;

    public FoodData[] foodList;

    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                yield return SpawnCustomer();
                isSpawning = false;
            }
            yield return null;
        }
    }

    private IEnumerator SpawnCustomer()
    {
        // tạo khách
        CustomerController customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        customer.seatPoint = seatPoint;
        customer.exitPoint = exitPoint;

        // đăng ký với ServeSystem
        serveSystem.RegisterCustomer(customer);

        // chọn món random
        FoodData randomFood = foodList[Random.Range(0, foodList.Length)];

        // bắt đầu order (CustomerController tự đi tới bàn)
        customer.StartOrder(randomFood);

        // đợi khách rời đi
        bool done = false;
        customer.OnLeave += (c) => { done = true; };

        while (!done) yield return null;
    }


    private IEnumerator MoveTo(Transform obj, Vector3 targetPos)
    {
        while (Vector3.Distance(obj.position, targetPos) > 0.05f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }
    }
}
