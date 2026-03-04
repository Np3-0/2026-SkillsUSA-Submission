using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightLogic : MonoBehaviour
{
    private const string FightSceneName = "Fight";
    private const string MainSceneName = "Main";
    private const string DefaultEnemyTag = "Enemy";
    private static readonly Vector3 FightPlayerPosition = new(-6f, 0f, 0f);
    private static readonly Vector3 FightEnemyPosition = new(6f, 0f, 0f);

    public GameObject buttonUI;
    public string winReturnScene = MainSceneName;
    private const string loseScene = "Game Over";

    private static GameObject pendingEnemy;
    private static string pendingEnemyTag = DefaultEnemyTag;
    private static Vector3 preFightPlayerPosition;
    private static Vector3 preFightEnemyPosition;
    private static string preFightSceneName = MainSceneName;
    private static bool hasPreFightSnapshot;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == FightSceneName)
        {
            PositionCombatantsInFightScene();
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == FightSceneName)
        {
            PositionCombatantsInFightScene();
        }

        if (scene.name != FightSceneName)
        {
            SetPlayerCanMove(true);
        }
    }

    public static void StartFight(string enemyName, GameObject enemyObject = null)
    {
        CapturePreFightState(enemyName, enemyObject);
        SetPlayerCanMove(false);

        FadeToBlack fade = FindFirstObjectByType<FadeToBlack>();
        if (fade != null)
        {
            fade.StartTransition(FightSceneName, PositionCombatantsInFightScene);
        }
        else
        {
            Debug.LogError("FadeToBlack not found in scene!");
            SceneManager.LoadScene(FightSceneName);
            PositionCombatantsInFightScene();
        }
    }

    private static void CapturePreFightState(string enemyName, GameObject enemyObject)
    {
        pendingEnemyTag = string.IsNullOrWhiteSpace(enemyName) ? DefaultEnemyTag : enemyName;
        pendingEnemy = enemyObject;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            preFightPlayerPosition = player.transform.position;
        }

        GameObject enemy = ResolveEnemyObject();
        if (enemy != null)
        {
            preFightEnemyPosition = enemy.transform.position;
            pendingEnemy = enemy;
        }

        preFightSceneName = SceneManager.GetActiveScene().name;
        hasPreFightSnapshot = true;
    }

    private static GameObject ResolveEnemyObject()
    {
        if (pendingEnemy != null)
        {
            return pendingEnemy;
        }

        if (!string.IsNullOrWhiteSpace(pendingEnemyTag))
        {
            GameObject taggedEnemy = GameObject.FindWithTag(pendingEnemyTag);
            if (taggedEnemy != null)
            {
                return taggedEnemy;
            }
        }

        GameObject defaultEnemy = GameObject.FindWithTag(DefaultEnemyTag);
        if (defaultEnemy != null)
        {
            return defaultEnemy;
        }

        GameObject beastEnemy = GameObject.FindWithTag("Beast");
        if (beastEnemy != null)
        {
            return beastEnemy;
        }

        GameObject fallbackEnemy = GameObject.FindWithTag("Enemy");
        if (fallbackEnemy != null)
        {
            return fallbackEnemy;
        }

        RegularEnemyState regularEnemyState = FindFirstObjectByType<RegularEnemyState>();
        if (regularEnemyState != null)
        {
            return regularEnemyState.gameObject;
        }

        BeastState beastState = FindFirstObjectByType<BeastState>();
        return beastState != null ? beastState.gameObject : null;
    }

    private static void PositionCombatantsInFightScene()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = FightPlayerPosition;
        }

        GameObject enemy = ResolveEnemyObject();
        if (enemy != null)
        {
            enemy.transform.position = FightEnemyPosition;
        }
    }

    private static bool TryResolveEnemyObject(out GameObject enemyObject)
    {
        enemyObject = ResolveEnemyObject();
        return enemyObject != null;
    }

    private static bool TryGetEnemyHealth(GameObject enemyObject, out float currentHealth, out float maximumHealth)
    {
        currentHealth = 0f;
        maximumHealth = 0f;

        if (enemyObject == null)
        {
            return false;
        }

        RegularEnemyState regularEnemyState = enemyObject.GetComponent<RegularEnemyState>();
        if (regularEnemyState != null)
        {
            currentHealth = regularEnemyState.curHealth;
            maximumHealth = regularEnemyState.maxHealth;
            return true;
        }

        BeastState beastState = enemyObject.GetComponent<BeastState>();
        if (beastState != null)
        {
            currentHealth = beastState.curHealth;
            maximumHealth = beastState.maxHealth;
            return true;
        }

        return false;
    }

    private static bool TrySetEnemyHealth(GameObject enemyObject, float value)
    {
        if (enemyObject == null)
        {
            return false;
        }

        RegularEnemyState regularEnemyState = enemyObject.GetComponent<RegularEnemyState>();
        if (regularEnemyState != null)
        {
            regularEnemyState.SetHealth(value);
            return true;
        }

        BeastState beastState = enemyObject.GetComponent<BeastState>();
        if (beastState != null)
        {
            beastState.SetHealth(value);
            return true;
        }

        return false;
    }

    private static void MarkEnemyDefeated(GameObject enemyObject)
    {
        if (enemyObject == null)
        {
            return;
        }

        RegularEnemyState regularEnemyState = enemyObject.GetComponent<RegularEnemyState>();
        if (regularEnemyState != null)
        {
            regularEnemyState.defeated = true;
        }

        BeastState beastState = enemyObject.GetComponent<BeastState>();
        if (beastState != null)
        {
            beastState.defeated = true;
        }
    }

    private static void SetPlayerCanMove(bool canMove)
    {
        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.canMove = canMove;
        }
    }

    public void Attack(PlayerState player, GameObject enemyObject, int type = 0)
    {
        if (!TryGetEnemyHealth(enemyObject, out float enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            return;
        }

        int damageMultiplier = Random.Range(1, 4);
        float damage = 10f * damageMultiplier;

        if (type == 0)
        {
            TrySetEnemyHealth(enemyObject, enemyHealth - damage);
            Debug.Log("Player attacks for " + damage + " damage! Enemy health: " + (enemyHealth - damage));
        }
        else
        {
            player.SetHealth(player.curHealth - damage);
            Debug.Log("Enemy attacks for " + damage + " damage! Player health: " + player.curHealth);
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.attackSound);
    }

    public void Ability(PlayerState player, GameObject enemyObject, int type = 0)
    {
        if (!TryGetEnemyHealth(enemyObject, out float enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            return;
        }

        float chance = Random.Range(1f, 2f);
        if (chance <= 1.65f)
        {
            Debug.Log("Ability failed! No damage dealt.");
            SoundManager.Instance.PlaySound(SoundManager.Instance.missSound);
            return;
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.abilitySound);
        int damageMultiplier = Random.Range(2, 5);
        float damage = 15f * damageMultiplier;

        if (type == 0)
        {
            TrySetEnemyHealth(enemyObject, enemyHealth - damage);
            Debug.Log("Player uses ability for " + damage + " damage! Enemy health: " + (enemyHealth - damage));
        }
        else
        {
            player.SetHealth(player.curHealth - damage);
            Debug.Log("Enemy uses ability for " + damage + " damage! Player health: " + player.curHealth);
        }
    }

    public void Heal(PlayerState player, GameObject enemyObject, int type = 0)
    {
        if (!TryGetEnemyHealth(enemyObject, out float enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            return;
        }

        int healAmount = Random.Range(10, 21);
        SoundManager.Instance.PlaySound(SoundManager.Instance.healSound);
        if (type == 0)
        {
            player.SetHealth(player.curHealth + healAmount);
            Debug.Log("Player heals for " + healAmount + "! Player health: " + player.curHealth);
        }
        else
        {
            TrySetEnemyHealth(enemyObject, enemyHealth + healAmount);
            Debug.Log("Enemy heals for " + healAmount + "! Enemy health: " + (enemyHealth + healAmount));
        }
    }

    public void EnemyTurn()
    {
        PlayerState playerState = PlayerState.Instance;
        if (playerState == null)
        {
            Debug.LogError("PlayerState.Instance not found for EnemyTurn.");
            ChangeButtonState(true);
            return;
        }

        if (!TryResolveEnemyObject(out GameObject enemyObject))
        {
            Debug.LogError("Enemy object not found for EnemyTurn.");
            ChangeButtonState(true);
            return;
        }

        if (!TryGetEnemyHealth(enemyObject, out float enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            ChangeButtonState(true);
            return;
        }

        if (enemyHealth <= 25f && Random.Range(1, 4) != 1)
        {
            Heal(playerState, enemyObject, 1);
            ChangeButtonState(true);
            return;
        }
        else if (enemyHealth <= 60f)
        {
            int distance = (int)(enemyHealth / 10f);
            int healChance = Random.Range(1, 11);
            if (healChance == 2 + distance)
            {
                Heal(playerState, enemyObject, 1);
                ChangeButtonState(true);
                return;
            }
        }

        int actionChoice = Random.Range(1, 11);
        if (actionChoice <= 8)
        {
            Attack(playerState, enemyObject, 1);
        }
        else
        {
            Ability(playerState, enemyObject, 1);
        }
        ChangeButtonState(true);
    }

    public void ChangeButtonState(bool state)
    {
        if (buttonUI == null)
        {
            return;
        }

        foreach (Button button in buttonUI.GetComponentsInChildren<Button>())
        {
            button.interactable = state;
        }
    }

    public void ButtonClickHandler(string button)
    {
        PlayerState player = PlayerState.Instance;
        if (player == null)
        {
            Debug.LogError("PlayerState.Instance not found for ButtonClickHandler.");
            return;
        }

        if (!TryResolveEnemyObject(out GameObject enemyObject))
        {
            Debug.LogError("Enemy object not found for ButtonClickHandler.");
            return;
        }

        if (!TryGetEnemyHealth(enemyObject, out float enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            return;
        }

        switch (button)
        {
            case "Fight":
                Attack(player, enemyObject, 0);
                break;
            case "Ability":
                Ability(player, enemyObject, 0);
                break;
            case "Heal":
                if (player.curHealth == player.maxHealth)
                {
                    Debug.Log("Player is already at full health!");
                    return;
                }
                Heal(player, enemyObject, 0);
                break;
            default:
                Debug.LogWarning($"Unknown fight action: {button}");
                return;
        }

        if (!TryGetEnemyHealth(enemyObject, out enemyHealth, out _))
        {
            Debug.LogError("Enemy state component not found on enemy.");
            return;
        }

        if (enemyHealth <= 0f)
        {
            Debug.Log("Enemy defeated!");
            MarkEnemyDefeated(enemyObject);
            SoundManager.Instance.PlaySound(SoundManager.Instance.enemyDeathSound);
            EndFight(playerWon: true);
            return;
        }

        Invoke(nameof(EnemyTurn), 0.75f);
        if (player.curHealth <= 0)
        {
            Debug.Log("Player defeated! Game Over.");
            EndFight(playerWon: false);
            return;
        }
    }

    private void EndFight(bool playerWon)
    {
        ChangeButtonState(false);

        string targetScene = playerWon
            ? (string.IsNullOrWhiteSpace(preFightSceneName) ? winReturnScene : preFightSceneName)
            : loseScene;

        FadeToBlack fade = FindFirstObjectByType<FadeToBlack>();
        if (fade != null)
        {
            fade.StartTransition(targetScene, () => FinalizeFightState(playerWon));
            return;
        }

        SceneManager.LoadScene(targetScene);
        FinalizeFightState(playerWon);
    }

    private void FinalizeFightState(bool playerWon)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null && hasPreFightSnapshot)
        {
            playerObject.transform.position = preFightPlayerPosition;
        }

        if (playerWon)
        {
            DestroyReturnedSceneEnemy();
        }
        else
        {
            if (PlayerState.Instance != null)
            {
                PlayerState.Instance.SetHealth(PlayerState.Instance.maxHealth);
            }

            if (pendingEnemy != null)
            {
                pendingEnemy.transform.position = preFightEnemyPosition;
            }
        }

        SetPlayerCanMove(true);
        pendingEnemy = null;
        pendingEnemyTag = DefaultEnemyTag;
        hasPreFightSnapshot = false;
    }

    private static void DestroyReturnedSceneEnemy()
    {
        string targetTag = string.IsNullOrWhiteSpace(pendingEnemyTag) ? DefaultEnemyTag : pendingEnemyTag;
        GameObject enemyToDestroy = null;

        GameObject originalEnemy = pendingEnemy;
        if (originalEnemy != null)
        {
            Destroy(originalEnemy);
        }

        if (hasPreFightSnapshot)
        {
            GameObject[] candidates = GameObject.FindGameObjectsWithTag(targetTag);
            float bestDistance = float.MaxValue;

            foreach (GameObject candidate in candidates)
            {
                float distance = Vector3.SqrMagnitude(candidate.transform.position - preFightEnemyPosition);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    enemyToDestroy = candidate;
                }
            }
        }

        if (enemyToDestroy == null)
        {
            enemyToDestroy = GameObject.FindWithTag(targetTag);
        }

        if (enemyToDestroy != null && (originalEnemy == null || enemyToDestroy != originalEnemy))
        {
            Destroy(enemyToDestroy);
        }
    }

    public static void TriggerEnding(string endingType)
    {
        string normalized = string.IsNullOrWhiteSpace(endingType) ? string.Empty : endingType.Trim();
        FadeToBlack fade = FindFirstObjectByType<FadeToBlack>();

        if (normalized.Equals("Quit", System.StringComparison.OrdinalIgnoreCase))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            return;
        }

        string sceneName = normalized;
        if (string.IsNullOrWhiteSpace(sceneName) || sceneName.Equals("End", System.StringComparison.OrdinalIgnoreCase))
        {
            sceneName = MainSceneName;
        }

        if (fade != null)
        {
            fade.StartTransition(sceneName, () => SetPlayerCanMove(true));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            SetPlayerCanMove(true);
        }
    }
}
