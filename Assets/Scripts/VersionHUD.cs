using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VersionHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI uiText;
    void Start()
    {
        string version = Application.version;

        if (uiText != null)
        {
            uiText.text = "v" + version;
        }
    }
}