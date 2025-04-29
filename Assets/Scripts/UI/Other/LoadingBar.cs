using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] Image loadingValueImage;

    public void SetValue(float value) {
        loadingValueImage.fillAmount = value;
    }
}
