using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    Dictionary<string, GameObject> dialogPrefabs = new Dictionary<string, GameObject>();
    // Các dialog đã được khởi tạo và có thể sử dụng
    Dictionary<string, Dialog> availableDialogs = new Dictionary<string, Dialog>();
    // Các dialog đang hoạt động
    Stack<Dialog> activeDialogs = new Stack<Dialog>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void RegisterDialog(string dialogId, GameObject dialogPrefab) {
        if (!dialogPrefabs.ContainsKey(dialogId)) {
            dialogPrefabs.Add(dialogId, dialogPrefab);
        }
    }

    // Mở dialog
    public Dialog ShowDialog(string dialogId, DialogParamater dialogParamater = null) {
        if (!dialogPrefabs.ContainsKey(dialogId)) return null;

        // Tái sử dụng dialog cũ hoặc tạo mới nếu chưa được tạo trước đó
        Dialog dialog = null;
        if (availableDialogs.ContainsKey(dialogId)) {
            dialog = availableDialogs[dialogId];
        }
        else {
            GameObject dialogInstance = Instantiate(dialogPrefabs[dialogId]);
            dialog = dialogInstance.GetComponent<Dialog>();
            availableDialogs.Add(dialogId, dialog);
        }

        // Ẩn dialog ở trên nhất để hiển thị dialog mới ở phía trên đó
        if (activeDialogs.Count > 0) {
            Dialog openDialog = activeDialogs.Peek();
            openDialog.Close();
        }

        dialog.Init(dialogParamater);
        dialog.Open();
        activeDialogs.Push(availableDialogs[dialogId]);

        if (GameManager.Instance.gameState != GameState.Pause) {
            GameManager.Instance.gameState = GameState.Pause;
        }

        return availableDialogs[dialogId];
    }

    // Ẩn dialog ở trên nhất
    public void HideDialog() {
        if (activeDialogs.Count <= 0) return;

        // Lấy dialog hiện tại (ở trên nhất) để ẩn
        Dialog openDialog = activeDialogs.Pop();
        openDialog.Close();

        if (activeDialogs.Count == 0) {
            GameManager.Instance.gameState = GameState.Play;
        }
        else {
            Dialog dialog = activeDialogs.Peek();
            dialog.Open();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Xóa các dialog đã được khởi tạo khi chuyển scene
        ClearAllDialogs();
    }

    void ClearAllDialogs() {
        availableDialogs.Clear();
        activeDialogs.Clear();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
