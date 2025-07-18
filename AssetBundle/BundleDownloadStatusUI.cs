using UnityEngine;
using TMPro;

public class BundleDownloadStatusUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text progressText;

    public void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    public void SetProgress(float progress)
    {
        if (progressText != null)
            progressText.text = $"{(progress * 100f):F0}%";
    }

    public void ResetUI()
    {
        SetStatus("Checking...");
        SetProgress(0f);
    }
}
