using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightLogic : MonoBehaviour
{

    public static void StartFight()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject enemy = GameObject.FindWithTag("Enemy");

        GlobalState.canMove = false;
        FadeToBlack fade = FindFirstObjectByType<FadeToBlack>();
        if (fade != null)
        {
            fade.StartTransition("Fight", () => {
                player.transform.position = new Vector3(-6, 0, 0);
                enemy.transform.position = new Vector3(6, 0, 0);
            });
        }
        else
        {
            Debug.LogError("FadeToBlack not found in scene!");
        }
    }

    public void ButtonClickHandler(string button)
    {
        if (button == "Fight")
        {
            Debug.Log("Player attacks!");
            // Implement attack logic here
        }
        else if (button == "Ability")
        {
            Debug.Log("Player uses ability!");
            // Implement ability logic here
        }
        else if (button == "Heal")
        {
            Debug.Log("Player heals!");
            // Implement defend logic here
        }
    }
}
