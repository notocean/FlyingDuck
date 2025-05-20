using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; private set; }

    public List<Transform> TileTransformList = new List<Transform>();
    Dictionary<string, int> tileIndexes = new Dictionary<string, int>();
    HashSet<int> takenPosIndexList = new HashSet<int>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else Destroy(gameObject);

        InitTileTransformList();
    }

    void InitTileTransformList() {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Tile");

        Dictionary<int, Transform> objList = new Dictionary<int, Transform>();
        foreach (GameObject obj in list) {
            objList.Add(SplitStr2Number(obj.name), obj.transform);
        }

        objList = objList.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        int count = 0;
        foreach (Transform tran in objList.Values) {
            if (tran.GetComponent<BaseTile>() != null) {
                TileTransformList.Add(tran);
                tileIndexes.Add(tran.name, count);
                count++;
            }
        }
    }

    int SplitStr2Number(string str) {
        StringBuilder numberStr = new StringBuilder();

        for (int i = str.Length - 1; i >= 0; i--) {
            if (char.IsDigit(str[i])) {
                numberStr.Insert(0, str[i]);
            }
            else break;
        }

        return int.Parse(numberStr.ToString());
    }

    public bool RegisterPos(int index) {
        if (!takenPosIndexList.Contains(index)) {
            takenPosIndexList.Add(index);
            return true;
        }
        return false;
    }

    public bool UnregisterPos(int index) {
        if (takenPosIndexList.Contains(index)) {
            takenPosIndexList.Remove(index);
            return true;
        }
        return false;
    }

    public bool IsPosIndexTaken(int index) {
        return takenPosIndexList.Contains(index);
    }

    public int GetPosIndexByName(string name) {
        if (tileIndexes.ContainsKey(name))
            return tileIndexes[name];
        else return -1;
    }

    /// <summary>
    /// Tìm kiếm và trả về index của tile chưa có đối tượng nào sử dụng với giới hạn các đối tượng trong khoảng [currentPosIndex - lowerLimit, currentPosIndex + upperLimit].
    /// </summary>
    public int GetNewTileIndex(int currentPosIndex, int lowerLimit, int upperLimit, int maxIndex) {
        int randomIndex;
        int min = Mathf.Clamp(currentPosIndex - lowerLimit, 0, maxIndex);
        int max = Mathf.Clamp(currentPosIndex + upperLimit, 0, maxIndex);

        List<int> emptyTileIndexes = new();
        for (int index = min; index <= max; index++) {
            if (!IsPosIndexTaken(index) && index != currentPosIndex)
                emptyTileIndexes.Add(index);
        }

        if (emptyTileIndexes.Count > 0) {
            randomIndex = emptyTileIndexes[Random.Range(0, emptyTileIndexes.Count)];
        }
        else {
            int negative = currentPosIndex > TileTransformList.Count / 2 ? -1 : 1;
            randomIndex = negative == -1 ? min : max;
            do {
                randomIndex += negative;
            }
            while (IsPosIndexTaken(randomIndex));
        }

        return randomIndex;
    }
}