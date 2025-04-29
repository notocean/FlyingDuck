using UnityEngine;

public class AttentionIndicator : MonoBehaviour {
    [SerializeField] ScriptableObject targetData;
    [SerializeField] GameObject attentionPrefab;

    IHasAttention atten;
    GameObject attentionObj;

    private void Start() {
        if (atten !=  null) {
            UpdateAttention();
        }
    }

    public void UpdateAttention() {
        if (atten != null) {
            bool hasAttention = atten.HasAttention();

            if (hasAttention) {
                if (attentionObj == null) {
                    attentionObj = Instantiate(attentionPrefab, transform);
                }
            }
            else {
                if (attentionObj != null) {
                    Destroy(attentionObj);
                }
            }
        }
    }

    private void OnEnable() {
        if (targetData is IHasAttention atten) {
            this.atten = atten;
            atten.hasAttentionChanged += UpdateAttention;
        }
    }

    private void OnDisable() {
        if (atten != null) {
            atten.hasAttentionChanged -= UpdateAttention;
        }
    }
}