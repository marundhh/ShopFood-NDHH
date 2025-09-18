using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    [Header("Points")]
    public Transform seatPoint;
    public Transform exitPoint;

    [Header("UI hiển thị món ăn")]
    public Image orderIconUI;

    [Header("Data & Events")]
    public FoodData orderFood;
    public event Action<CustomerController, FoodData> OnOrderFood;
    public event Action<CustomerController> OnLeave;

    [Header("Movement")]
    public float moveSpeed = 2f;

    private bool hasFood = false;
    private GameObject foodOnTable;

    // Biến theo dõi trạng thái flip
    private bool hasFlipped = false;

    public void StartOrder(FoodData food)
    {
        orderFood = food;

        // Hiển thị icon món ăn
        if (orderIconUI != null && food.foodIcon != null)
        {
            orderIconUI.sprite = food.foodIcon;
            orderIconUI.enabled = true;
        }

        // Bắt đầu đi tới bàn
        StartCoroutine(GoToSeat());
    }

    private IEnumerator GoToSeat()
    {
        // 👉 Flip đúng hướng 1 lần, chỉ lúc bắt đầu đi
        FlipTowards(seatPoint.position);

        // Di chuyển tới bàn
        yield return StartCoroutine(MoveTo(seatPoint.position));

        Debug.Log("Khách tới bàn");
        OnOrderFood?.Invoke(this, orderFood);
    }

    public void OnFoodDelivered(GameObject foodObj)
    {
        if (hasFood) return;
        hasFood = true;

        if (orderIconUI != null)
            orderIconUI.enabled = false;

        foodOnTable = foodObj;

        // 👉 Bắt đầu ăn và đi ra
        StartCoroutine(EatAndLeave());
    }

    private IEnumerator EatAndLeave()
    {
        yield return new WaitForSeconds(2f); // thời gian ăn

        if (foodOnTable != null)
        {
            Destroy(foodOnTable);
            foodOnTable = null;
        }

        MoneyUI.Instance.AddMoney(orderFood.price);

        OnLeave?.Invoke(this);

        // 👉 Ăn xong thì reset flip về hướng mặc định
        ResetFlip();

        // Đi ra
        yield return StartCoroutine(MoveTo(exitPoint.position));

        Destroy(gameObject);
    }

    /// <summary>
    /// Flip duy nhất 1 lần theo target
    /// </summary>
    private void FlipTowards(Vector3 targetPos)
    {
        if (hasFlipped) return; // đã flip rồi thì không làm gì

        Vector3 scale = transform.localScale;
        scale.x = (targetPos.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        hasFlipped = true;
    }

    /// <summary>
    /// Reset flip về hướng mặc định (luôn quay phải)
    /// </summary>
    private void ResetFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        hasFlipped = false; // chuẩn bị cho lần flip tiếp theo nếu cần
    }

    /// <summary>
    /// Di chuyển tới vị trí target
    /// </summary>
    private IEnumerator MoveTo(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
}
