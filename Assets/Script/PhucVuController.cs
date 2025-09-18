using UnityEngine;
using System.Collections;

public class PhucVuController : MonoBehaviour
{
    public Transform bepPoint;
    public Transform banPoint;
    public PhucVuAnimation animationCtrl;

    [Header("Offset khi đặt món trên bàn")]
    public Vector3 foodOffset = new Vector3(0.3f, 0.1f, 0); // có thể gán trực tiếp từ Inspector

    private GameObject foodInHand;

    public void DeliverFood(GameObject cookedFood, CustomerController customer)
    {
        StartCoroutine(DeliverFlow(cookedFood, customer));
    }

    private IEnumerator DeliverFlow(GameObject cookedFood, CustomerController customer)
    {
        // cầm món ở bếp
        foodInHand = cookedFood;
        foodInHand.SetActive(true);
        foodInHand.transform.SetParent(transform);
        foodInHand.transform.localPosition = new Vector3(0.5f, 0, 0);

        // đi ra bàn
        animationCtrl.PlayCarry();
        transform.localScale = new Vector3(1, 1, 1);
        yield return StartCoroutine(MoveTo(banPoint.position));

        // đặt món lên bàn với offset có thể gán từ Inspector
        foodInHand.transform.SetParent(null);
        foodInHand.transform.position = banPoint.position + foodOffset;

        Debug.Log("+ " + customer.orderFood.price + " VND");

        // báo cho khách
        customer.OnFoodDelivered(foodInHand);

        // quay về bếp
        animationCtrl.PlayReturn();
        transform.localScale = new Vector3(-1, 1, 1);
        yield return StartCoroutine(MoveTo(bepPoint.position));

        animationCtrl.PlayIdle();
    }

    private IEnumerator MoveTo(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }
    }
}
