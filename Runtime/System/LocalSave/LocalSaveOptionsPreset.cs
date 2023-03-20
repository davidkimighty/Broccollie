using System;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "LocalSaveOptions", menuName = "Broccollie/System/Local Save Options")]
    public class LocalSaveOptionsPreset : ScriptableObject
    {
        public LocalSaveOptions Options;
    }

    [Serializable]
    public struct LocalSaveOptions
    {
        public ScriptableObject Data;
        public string SaveDirectory;
        public string SaveFileName;
        public FileExtensionTypes FileExtension;
        public string AesKey;

        public bool UseSaveables;
        public bool UseCryptoStream;
        public bool DataEncryption;
    }

    public enum FileExtensionTypes { json }
}
