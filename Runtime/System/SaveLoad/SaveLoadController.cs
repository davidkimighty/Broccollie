using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace CollieMollie.System
{
    public class SaveLoadController : MonoBehaviour
    {
        #region Public Functions
        public IEnumerator LoadDataDecrypt(string saveFolderPath, string fileName, string aesKey, object data, Action done = null)
        {
            CheckPath(saveFolderPath);
            string savePath = Path.Combine(saveFolderPath, fileName);

            if (!File.Exists(savePath) || !PlayerPrefs.HasKey(aesKey))
            {
                Debug.Log("[SaveLoadController] Couldn't find any saved player data.");
                yield break;
            }

            try
            {
                using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
                {
                    Aes aes = Aes.Create();
                    byte[] outputIV = new byte[aes.IV.Length];
                    fileStream.Read(outputIV, 0, outputIV.Length);

                    byte[] savedKey = Convert.FromBase64String(PlayerPrefs.GetString(aesKey));
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(savedKey, outputIV), CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            string dataText = streamReader.ReadToEnd();
                            JsonUtility.FromJsonOverwrite(dataText, data);
                            done?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoadController] {e}");
            }
        }

        public IEnumerator SaveDataEncrypt(string saveFolderPath, string fileName, string aesKey, object data, Action done = null)
        {
            CheckPath(saveFolderPath);
            string savePath = Path.Combine(saveFolderPath, fileName);

            try
            {
                Aes aes = Aes.Create();
                byte[] savedKey = aes.Key;
                PlayerPrefs.SetString(aesKey, Convert.ToBase64String(savedKey));

                using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                {
                    byte[] inputIV = aes.IV;
                    fileStream.Write(inputIV, 0, inputIV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            string jsonString = JsonUtility.ToJson(data);
                            streamWriter.Write(jsonString);
                            done?.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoadController] {e}");
            }
            yield return null;
        }
        #endregion

        #region
        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log("[SaveLoadController] Save folder was created successfully.");
            }
        }
        #endregion
    }
}
