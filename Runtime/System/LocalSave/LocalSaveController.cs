using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;

namespace Broccollie.System
{
    public class LocalSaveController : MonoBehaviour
    {
        [SerializeField] private LocalSaveEventChannel _eventChannel = null;
        [SerializeField] private LocalSaveOptionsPreset _preset = null;

        private void OnEnable()
        {
            _eventChannel.OnSaveAsync += RequestSaveAsync;
            _eventChannel.OnLoadAsync += RequestLoadAsync;
        }

        private void OnDisable()
        {
            _eventChannel.OnSaveAsync -= RequestSaveAsync;
            _eventChannel.OnLoadAsync -= RequestLoadAsync;
        }

        #region Subscribers
        private async Task RequestSaveAsync()
        {
            if (_preset.Options.UseSaveables)
                await SaveSaveablesAsync();
            await SaveDataAsync(_preset);
        }

        private async Task RequestLoadAsync()
        {
            await LoadDataAsync(_preset);
            if (_preset.Options.UseSaveables)
                await LoadSaveablesAsync();
        }

        #endregion

        #region Public Functions
        public async Task SaveAsync(Action done = null)
        {
            if (_preset.Options.UseSaveables)
                await SaveSaveablesAsync();
            await SaveDataAsync(_preset, done);
        }

        public async Task LoadAsync(Action done = null)
        {
            await LoadDataAsync(_preset, done);
            if (_preset.Options.UseSaveables)
                await LoadSaveablesAsync();
        }

        #endregion

        private async Task SaveSaveablesAsync()
        {
            LocalSaveableEntity[] saveables = FindObjectsOfType<LocalSaveableEntity>();
            foreach (LocalSaveableEntity saveable in saveables)
            {
                saveable.SaveStates();
                await Task.Yield();
            }
        }

        private async Task LoadSaveablesAsync()
        {
            foreach (LocalSaveableEntity saveable in FindObjectsOfType<LocalSaveableEntity>())
            {
                saveable.LoadStates();
                await Task.Yield();
            }
        }

        private async Task LoadDataAsync(LocalSaveOptionsPreset preset, Action done = null)
        {
            CheckPath(Path.Combine(Application.persistentDataPath, preset.Options.SaveDirectory));
            string savePath = Path.Combine(Application.persistentDataPath, preset.Options.SaveDirectory,
                $"{preset.Options.SaveFileName}.{preset.Options.FileExtension.ToString()}");

            if (!File.Exists(savePath) || !PlayerPrefs.HasKey(preset.Options.AesKey))
            {
                Helper.Log("Couldn't find any saved data.", Helper.Broccollie, this);
                return;
            }

            try
            {
                using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
                {
                    if (preset.Options.UseCryptoStream)
                    {
                        Aes aes = Aes.Create();
                        byte[] outputIV = new byte[aes.IV.Length];
                        fileStream.Read(outputIV, 0, outputIV.Length);

                        byte[] savedKey = Convert.FromBase64String(PlayerPrefs.GetString(preset.Options.AesKey));
                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(savedKey, outputIV), CryptoStreamMode.Read))
                        {
                            await StreamReadAsync(cryptoStream, preset.Options.Data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        await StreamReadAsync(fileStream, preset.Options.Data, () => done?.Invoke());
                    }
                }
            }
            catch (Exception e)
            {
                Helper.LogError(e.Message, Helper.Broccollie, this);
            }
        }

        private async Task SaveDataAsync(LocalSaveOptionsPreset preset, Action done = null)
        {
            CheckPath(Path.Combine(Application.persistentDataPath, preset.Options.SaveDirectory));
            string savePath = Path.Combine(Application.persistentDataPath, preset.Options.SaveDirectory,
                $"{preset.Options.SaveFileName}.{preset.Options.FileExtension.ToString()}");

            try
            {
                Aes aes = Aes.Create();
                byte[] savedKey = aes.Key;
                PlayerPrefs.SetString(preset.Options.AesKey, Convert.ToBase64String(savedKey));

                using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                {
                    if (preset.Options.UseCryptoStream)
                    {
                        byte[] inputIV = aes.IV;
                        await fileStream.WriteAsync(inputIV, 0, inputIV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                        {
                            await StreamWriterAsync(cryptoStream, preset.Options.Data, () => done?.Invoke());
                        }
                    }
                    else
                    {
                        await StreamWriterAsync(fileStream, preset.Options.Data, () => done?.Invoke());
                    }
                }
            }
            catch (Exception e)
            {
                Helper.LogError(e.Message, Helper.Broccollie, this);
            }
        }

        private void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Helper.Log("Save folder was created.", Helper.Broccollie, this);
            }
        }

        private async Task StreamReadAsync(Stream stream, object data, Action done = null)
        {
            using (StreamReader streamReader = new StreamReader(stream))
            {
                string dataText = await streamReader.ReadToEndAsync();
                JsonUtility.FromJsonOverwrite(dataText, data);
                done?.Invoke();
                Helper.Log("Data loaded.", Helper.Broccollie, this);
            }
        }

        private async Task StreamWriterAsync(Stream stream, object data, Action done = null)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                string jsonString = JsonUtility.ToJson(data);
                await streamWriter.WriteAsync(jsonString);
                done?.Invoke();
                Helper.Log("Data saved", Helper.Broccollie, this);
            }
        }
    }
}
