using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace CollieMollie.System
{
    public class SaveLoadController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _useCryptoStream = true;
        [SerializeField] private bool _dataEncryption = true;

        #endregion

        #region Public Functions
        public IEnumerator LoadData(string saveFolderPath, string fileName, object data, string aesKey = "", Action done = null)
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
                    if (_useCryptoStream)
                    {
                        Aes aes = Aes.Create();
                        byte[] outputIV = new byte[aes.IV.Length];
                        fileStream.Read(outputIV, 0, outputIV.Length);

                        byte[] savedKey = Convert.FromBase64String(PlayerPrefs.GetString(aesKey));
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(savedKey, outputIV), CryptoStreamMode.Read))
                        {
                            StreamRead(cryptoStream, data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        StreamRead(fileStream, data, () => done?.Invoke());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveLoadController] {e}");
            }
        }

        public IEnumerator SaveData(string saveFolderPath, string fileName, object data, string aesKey = "", Action done = null)
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
                    if (_useCryptoStream)
                    {
                        byte[] inputIV = aes.IV;
                        fileStream.Write(inputIV, 0, inputIV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                        {
                            StreamWriter(cryptoStream, data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        StreamWriter(fileStream, data, () => done?.Invoke());
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

        #region Private Functions
        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log("[SaveLoadController] Save folder was created successfully.");
            }
        }

        private void StreamRead(Stream stream, object data, Action done = null)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string dataText = streamReader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(dataText, data);
                done?.Invoke();
                Debug.Log($"[SaveLoadController] Data loaded.");
            }
        }

        private void StreamWriter(Stream stream, object data, Action done = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                string jsonString = JsonUtility.ToJson(data);
                streamWriter.Write(jsonString);
                done?.Invoke();
                Debug.Log($"[SaveLoadController] Data saved.");
            }
        }
        #endregion
    }
}
