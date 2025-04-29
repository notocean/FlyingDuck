using UnityEngine;

public class AchievementManager : MonoBehaviour {
    public static AchievementManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


}