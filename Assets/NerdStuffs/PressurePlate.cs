using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Platform;
    public Collider2D Trigger;
    public Door door;
    
    Collider2D[] contacts = new Collider2D[1];
    ContactFilter2D contactFilter;

    private void Start()
    {
        var playerLayer = LayerMask.GetMask("Player");

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(playerLayer);

    }

    int pressure = 0;
    int direction = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        
        var count = Trigger.OverlapCollider(contactFilter, contacts);

        if (count > 0)
        {
            if(direction != 1)
            {
                var fat = contacts[0].GetComponent<FatScript>();
                if (pressure > 10 && fat.Fat > 3)
                    pressure += 7;
                else if (fat.Fat > 2)
                    pressure++;
                else
                pressure = 1;
                direction = -1;
            }
            else
            {
                direction = 0;
            }
        }
        else
        {
            if(direction > 0)
            {
                pressure-= direction;
            }
            if(direction < 5)
            {
                direction++;
            }
        }
        if (pressure > 30)
            pressure = 30;
        if (pressure < 0)
            pressure = 0;
        if(pressure == 0)
        {
            Platform.transform.localPosition = new Vector2(0f, 0f);
        }
        else if(pressure < 10)
        {
            Platform.transform.localPosition = new Vector2(0f, -0.0625f);
        }
        else
        {
            Platform.transform.localPosition = new Vector2(0f, -0.0625f * pressure / 10f);
        }
        if(door)
        {
            door.Pressure = pressure;
        }

    }
}
