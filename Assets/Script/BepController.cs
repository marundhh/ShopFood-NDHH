using System;
using System.Collections;
using UnityEngine;

public class BepController : MonoBehaviour
{
    public Transform cookPoint;

    public void CookFood(FoodData data, Action<GameObject> onCookDone)
    {
        StartCoroutine(CookFlow(data, onCookDone));
    }

    private IEnumerator CookFlow(FoodData data, Action<GameObject> onCookDone)
    {
        Debug.Log("Bếp bắt đầu nấu: " + data.foodName);
        yield return new WaitForSeconds(data.cookTime);

        // Spawn đồ ăn tại bếp
        GameObject cookedFood = Instantiate(data.prefab, cookPoint.position, Quaternion.identity);
        cookedFood.SetActive(true);

        Debug.Log("Bếp nấu xong: " + data.foodName);

        // Hiện ở bếp 1 giây
        yield return new WaitForSeconds(1f);

        // Tắt món ăn đi (chuẩn bị đưa cho phục vụ cầm)
        cookedFood.SetActive(false);

        // Gọi callback cho phục vụ
        onCookDone?.Invoke(cookedFood);
    }
}
