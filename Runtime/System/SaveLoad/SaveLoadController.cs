using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CollieMollie.System
{
    public class SaveLoadController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _useSaveables = true;
        [SerializeField] private bool _useCryptoStream = true;
        [SerializeField] private bool _dataEncryption = true;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        #endregion

        #region Public Functions
        public async Task SaveAsync(object data, SaveOptions options, CancellationToken token, Action done = null)
        {
            if (_useSaveables)
                await SaveSaveablesAsync(token);
            await SaveDataAsync(data, options, token, done);
        }

        public async Task LoadAsync(object data, SaveOptions options, CancellationToken token, Action done = null)
        {
            await LoadDataAsync(data, options, token, done);
            if (_useSaveables)
                await LoadSaveablesAsync(token);
        }

        #endregion

        #region Private Functions
        private async Task SaveSaveablesAsync(CancellationToken token)
        {
            SaveableEntity[] saveables = FindObjectsOfType<SaveableEntity>();
            foreach (SaveableEntity saveable in saveables)
            {
                token.ThrowIfCancellationRequested();
                saveable.SaveStates();
                await Task.Yield();
            }
        }

        private async Task LoadSaveablesAsync(CancellationToken token)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                token.ThrowIfCancellationRequested();
                saveable.LoadStates();
                await Task.Yield();
            }
        }

        private async Task LoadDataAsync(object data, SaveOptions options, CancellationToken token, Action done = null)
        {
            CheckPath(options.SaveDirectory);
            string savePath = Path.Combine(options.SaveDirectory, options.SaveFileName);

            if (!File.Exists(savePath) || !PlayerPrefs.HasKey(options.AesKey))
            {
                Helper.Helper.Log("Couldn't find any saved data.", Helper.Helper.Broccollie, this);
                return;
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

                        byte[] savedKey = Convert.FromBase64String(PlayerPrefs.GetString(options.AesKey));
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(savedKey, outputIV), CryptoStreamMode.Read))
                        {
                            await StreamReadAsync(cryptoStream, data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        await StreamReadAsync(fileStream, data, () => done?.Invoke());
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.LogError(e.Message, Helper.Helper.Broccollie, this);
            }
        }

        private async Task SaveDataAsync(object data, SaveOptions options, CancellationToken token, Action done = null)
        {
            CheckPath(options.SaveDirectory);
            string savePath = Path.Combine(options.SaveDirectory, options.SaveFileName);

            try
            {
                Aes aes = Aes.Create();
                byte[] savedKey = aes.Key;
                PlayerPrefs.SetString(options.AesKey, Convert.ToBase64String(savedKey));

                using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                {
                    if (_useCryptoStream)
                    {
                        byte[] inputIV = aes.IV;
                        await fileStream.WriteAsync(inputIV, 0, inputIV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                        {
                            await StreamWriterAsync(cryptoStream, data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        await StreamWriterAsync(fileStream, data, () => done?.Invoke());
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.LogError(e.Message, Helper.Helper.Broccollie, this);
            }
        }

        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Helper.Helper.Log("Save folder was created.", Helper.Helper.Broccollie, this);
            }
        }

        private async Task StreamReadAsync(Stream stream, object data, Action done = null)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string dataText = await streamReader.ReadToEndAsync();
                JsonUtility.FromJsonOverwrite(dataText, data);
                done?.Invoke();
                Helper.Helper.Log("Data loaded.", Helper.Helper.Broccollie, this);
            }
        }

        private async Task StreamWriterAsync(Stream stream, object data, Action done = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                string jsonString = JsonUtility.ToJson(data);
                await streamWriter.WriteAsync(jsonString);
                done?.Invoke();
                Helper.Helper.Log("Data saved", Helper.Helper.Broccollie, this);
            }
        }

        #endregion
    }

    [Serializable]
    public struct SaveOptions
    {
        public string SaveDirectory;
        public string SaveFileName;
        public string AesKey;
    }
}
