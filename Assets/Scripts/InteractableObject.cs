using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public Transform playerPos;
    public RawImage img;
    public GameObject interactionUI;
    public Image interactableImg;
    public Sprite objectSprite;
    public string interactableName;
    public string endingType;
    public string enemyTag;
    public string[] text = new string[2];
    float dist;
    TextMeshProUGUI title, infoObj;
    public int interactionIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img.enabled = false;
        interactionUI.SetActive(false);
        title = interactionUI.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        infoObj = interactionUI.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        if (objectSprite == null && interactableImg != null)
        {
            objectSprite = interactableImg.sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerPos = player.transform;
            }
            else
            {
                if (img != null)
                {
                    img.enabled = false;
                }

                return;
            }
        }

        dist = Vector3.Distance(playerPos.position, transform.position);
        if (dist < 3)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }

        if (img.enabled && Input.GetKeyDown(KeyCode.E))
        {
            img.enabled = false;
            ShowInteraction();
        }

        if (img.enabled && !PlayerState.Instance.canMove && Input.GetKeyDown(KeyCode.Space))
        {
            MoveInteraction();
        }
    }

    public void ShowInteraction()
    {
        PlayerState.Instance.canMove = false;
        UpdateSpeakerVisuals();

        infoObj.text = text[interactionIndex];
        interactionUI.SetActive(true);
    }

    public void MoveInteraction()
    {
        
        interactionIndex++;
        if (interactionIndex >= text.Length)
        {
            CloseInteraction();
            return;
        }

        UpdateSpeakerVisuals();
        infoObj.text = text[interactionIndex];
    }

    private void UpdateSpeakerVisuals()
    {
        bool isObjectTurn = interactionIndex % 2 == 0;

        if (isObjectTurn)
        {
            title.text = interactableName;
            if (interactableImg != null && objectSprite != null)
            {
                interactableImg.sprite = objectSprite;
            }

            return;
        }

        title.text = "You";
        if (interactableImg != null && PlayerState.Instance != null && PlayerState.Instance.img != null)
        {
            interactableImg.sprite = PlayerState.Instance.img;
        }
    }

    public void CloseInteraction()
    {
        if (endingType == null || endingType == "")
        {
            PlayerState.Instance.canMove = true;
            interactionUI.SetActive(false);
            img.enabled = false;
            interactionIndex = 0;
            return;
        } else if (endingType == "Fight")
        {
            interactionUI.SetActive(false);
            img.enabled = false;
            interactionIndex = 0;
            SoundManager.Instance.PlaySound(SoundManager.Instance.encounterSound);
            FightLogic.StartFight(enemyTag, gameObject);
            return;
        }

        interactionUI.SetActive(false);
        img.enabled = false;
        interactionIndex = 0;
        FightLogic.TriggerEnding(endingType);

    }
}
