using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] Image loadingValueImage;

    public void SetValue(float value) {
        loadingValueImage.fillAmount = value;
    }
}
