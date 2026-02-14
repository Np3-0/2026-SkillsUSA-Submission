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

    public void Attack(PlayerState player, RegularEnemyState enemy, int type = 0)
    {
        int damageMultiplier = Random.Range(1, 4);
        if (type == 0)
        {
            enemy.SetHealth(enemy.curHealth - (10 * damageMultiplier));
        }
        else
        {
            player.SetHealth(player.curHealth - (10 * damageMultiplier));
        }
        
        Debug.Log("Player attacks for " + (10 * damageMultiplier) + " damage! Enemy health: " + enemy.curHealth);
    }

    public void Ability(PlayerState player, RegularEnemyState enemy, int type = 0)
    {
        float chance = Random.Range(1f, 2f);
        if (chance <= 1.5f)
        {
            Debug.Log("Ability failed! No damage dealt.");
            return;
        }
        int damageMultiplier = Random.Range(2, 5);
        if (type == 0)
        {
            enemy.SetHealth(enemy.curHealth - (15 * damageMultiplier));
            Debug.Log("Player uses ability for " + (15 * damageMultiplier) + " damage! Enemy health: " + enemy.curHealth);
        }
        else
        {
            player.SetHealth(player.curHealth - (15 * damageMultiplier));
            Debug.Log("Enemy uses ability for " + (15 * damageMultiplier) + " damage! Player health: " + player.curHealth);
        }
    }
    
    public void Heal(PlayerState player, RegularEnemyState enemy, int type = 0)
    {
        int healAmount = Random.Range(10, 21);
        if (type == 0)
        {
            player.SetHealth(player.curHealth + healAmount);
            Debug.Log("Player heals for " + healAmount + "! Player health: " + player.curHealth);
        }
        else
        {
            enemy.SetHealth(enemy.curHealth + healAmount);
            Debug.Log("Enemy heals for " + healAmount + "! Enemy health: " + enemy.curHealth);
        }
    }

    public void EnemyTurn()
    {
        PlayerState playerState = PlayerState.Instance;
        RegularEnemyState enemy = GameObject.FindWithTag("Enemy").GetComponent<RegularEnemyState>();
        if (enemy.curHealth <= 25)
        {
            Heal(playerState, enemy, 1);
            return;
        }
        else if (enemy.curHealth <= 60)
        {
            int distance = (int)(enemy.curHealth / 10);
            int healChance = Random.Range(1, 11);
            if (healChance == 2 + distance)
            {
                Heal(playerState, enemy, 1);
                return;
            }
        }
        int actionChoice = Random.Range(1, 11);
        if (actionChoice <= 8)        {
            Attack(playerState, enemy, 1);
        }
        else
        {
            Ability(playerState, enemy, 1);
        }
    }

    public void ButtonClickHandler(string button)
    {
        PlayerState player = PlayerState.Instance;
        RegularEnemyState enemy = GameObject.FindWithTag("Enemy").GetComponent<RegularEnemyState>();
        

        if (button == "Fight")
        {
            Attack(player, enemy, 0);
        }
        else if (button == "Ability")
        {
            Ability(player, enemy, 0);
        }
        else if (button == "Heal")
        {
            if (player.curHealth == player.maxHealth)
            {
                Debug.Log("Player is already at full health!");
                return;
            }
            Heal(player, enemy, 0);
        }
        if (enemy.curHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            GlobalState.canMove = true;
            return;
        }
        Invoke("EnemyTurn", 0.75f);
        if (player.curHealth <= 0)
        {
            Debug.Log("Player defeated! Game Over.");
            return;
        }
    }
}
