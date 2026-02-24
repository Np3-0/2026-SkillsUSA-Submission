using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public Transform playerPos;
    public RawImage img;
    public GameObject interactionUI;
    public string interactableName;
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
    }

    // Update is called once per frame
    void Update()
    {
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
        if (interactionIndex % 2 == 0)
        {
            title.text = interactableName;
        }
        else
        {
            title.text = "Player";
        }

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

        if (interactionIndex % 2 == 0)
        {
            title.text = interactableName;
        }
        else
        {
            title.text = "Player";
        }
        infoObj.text = text[interactionIndex];
    }

    public void CloseInteraction()
    {
        PlayerState.Instance.canMove = true;
        interactionUI.SetActive(false);
        img.enabled = false;
        interactionIndex = 0;

    }
}
