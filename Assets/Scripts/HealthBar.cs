using UnityEngine;
public class HealthBar : MonoBehaviour
{
    public float health = 100f;

    void Start()
    {
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            health -= 10f;
            Debug.Log("Health: " + health);
        }
        if (health <= 0f)
        {
            Debug.Log("dead!");
        }
    }
}
