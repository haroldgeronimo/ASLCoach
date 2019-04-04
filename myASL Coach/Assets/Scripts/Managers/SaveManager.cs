using System.IO;
using UnityEngine;
public class SaveManager : MonoBehaviour {
	public string dataPath;
	void Awake () {
		dataPath = Path.Combine (Application.persistentDataPath, "PlayerData.dat");
	}
	public void SaveGame (PlayerData playerData, string path) {
		Save (playerData, path);
	}
	public PlayerData LoadGame (string path) {
		return Load (path);
	}
	static void Save (PlayerData playerData, string path) {
		string jsonString = JsonUtility.ToJson (playerData);
		using (StreamWriter streamwriter = File.CreateText (path)) {
			streamwriter.Write (jsonString);
		}
	}
	static PlayerData Load (string path) {
		try {
			using (StreamReader streamReader = File.OpenText (path)) {
				string jsonString = streamReader.ReadToEnd ();
				return JsonUtility.FromJson<PlayerData> (jsonString);
			}
		} catch (System.Exception ex) {
			Debug.Log (ex);
			return null;
		}

	}

}