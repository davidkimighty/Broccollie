using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace CollieMollie.System
{
    public class SaveLoadController : MonoBehaviour
    {
        #region Save Load
        public IEnumerator LoadDataDecrypt(string saveFolderPath, string fileName, string aesKey, object data)
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Debug.Log("[Helper] Save folder does not exist.");
                Directory.CreateDirectory(saveFolderPath);
                Debug.Log("[Helper] Save folder was created successfully.");
            }

            if (!File.Exists(saveFolderPath + fileName) || !PlayerPrefs.HasKey(aesKey))
            {
                Debug.Log("[Helper] Couldn't find any saved player data.");
                yield break;
            }

            try
            {
                using (FileStream fileStream = new FileStream(saveFolderPath + fileName, FileMode.Open))
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Helper] {e}");
            }
        }

        public IEnumerator SaveDataEncrypt(string savePath, string aesKey, object data)
        {
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Helper] {e}");
            }
            yield return null;
        }
        #endregion
    }
}
