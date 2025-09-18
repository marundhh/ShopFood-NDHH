using UnityEngine;

public class ServeSystem : MonoBehaviour
{
    public BepController bep;
    public PhucVuController phucVu;

    public void RegisterCustomer(CustomerController customer)
    {
        customer.OnOrderFood += (c, food) =>
        {
            bep.CookFood(food, (cookedFood) =>
            {
                phucVu.DeliverFood(cookedFood, c);
            });
        };
    }
}
