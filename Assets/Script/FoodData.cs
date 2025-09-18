using UnityEngine;

[CreateAssetMenu(menuName = "Food/FoodData")]
public class FoodData : ScriptableObject
{
    public string foodName;
    public GameObject prefab;   // prefab món
    public Sprite foodIcon; // hình ảnh món ăn   // ảnh hiển thị món ăn
    public float cookTime;      // thời gian nấu
    public int price;           // giá tiền
}
