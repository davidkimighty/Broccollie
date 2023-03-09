using System;
using UnityEngine;

namespace CollieMollie.System
{
    [CreateAssetMenu(fileName = "LocalSaveOptions", menuName = "CollieMollie/System/Local Save Options")]
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
