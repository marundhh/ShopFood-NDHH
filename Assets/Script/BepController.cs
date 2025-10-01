using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BepController : MonoBehaviour
{
    [Header("Cooking Settings")]
    public Transform cookPoint;
    public Image cookProgressUI; // ảnh UI kiểu Fill

    public void CookFood(FoodData data, Action<GameObject> onCookDone)
    {
        StartCoroutine(CookFlow(data, onCookDone));
    }

    private IEnumerator CookFlow(FoodData data, Action<GameObject> onCookDone)
    {
        Debug.Log("Bếp bắt đầu nấu: " + data.foodName);

        // Reset progress
        if (cookProgressUI != null)
            cookProgressUI.fillAmount = 0f;

        float timer = 0f;

        // Trong lúc nấu thì update progress bar
        while (timer < data.cookTime)
        {
            timer += Time.deltaTime;
            if (cookProgressUI != null)
                cookProgressUI.fillAmount = timer / data.cookTime;

            yield return null; // chờ frame sau
        }

        // Spawn đồ ăn tại bếp
        GameObject cookedFood = Instantiate(data.prefab, cookPoint.position, Quaternion.identity);
        cookedFood.SetActive(true);

        Debug.Log("Bếp nấu xong: " + data.foodName);

        // Giữ hiển thị progress = full
        if (cookProgressUI != null)
            cookProgressUI.fillAmount = 1f;

        // Hiện ở bếp 1 giây
        yield return new WaitForSeconds(1f);

        // Tắt món ăn đi (chuẩn bị đưa cho phục vụ cầm)
        cookedFood.SetActive(false);

        // Reset progress (ẩn đi hoặc để rỗng)
        if (cookProgressUI != null)
            cookProgressUI.fillAmount = 0f;

        // Gọi callback cho phục vụ
        onCookDone?.Invoke(cookedFood);
    }
}
