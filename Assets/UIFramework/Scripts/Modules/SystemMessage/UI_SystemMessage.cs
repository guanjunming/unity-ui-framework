using TMPro;
using UnityEngine;

public class UI_SystemMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    public void ShowMessage(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void OnFinished()
    {
        gameObject.SetActive(false);
    }
}