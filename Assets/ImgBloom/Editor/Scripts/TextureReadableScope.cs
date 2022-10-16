using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ImgBloom.Editor
{
    internal class TextureReadableScope : IDisposable
    {
        private bool _isReadable;

        private TextureImporter _textureImporter;
        
        internal TextureReadableScope(Texture2D target)
        {
            if (target == null)
            {
                throw new ArgumentNullException($"{nameof(target)} must NOT be null");
            }
            
            string path = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(path))
            {
                throw new FileNotFoundException("Asset file is not found.");
            }
            
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter == null)
            {
                throw new InvalidOperationException($"AssetImporter at {path} can't be casted to {nameof(textureImporter)}");
            }

            _isReadable = textureImporter.isReadable;
            _textureImporter = textureImporter;

            textureImporter.isReadable = true;
            textureImporter.SaveAndReimport();
            AssetDatabase.Refresh();
        }
        
        public void Dispose()
        {
            _textureImporter.isReadable = _isReadable;
            _textureImporter.SaveAndReimport();
            AssetDatabase.Refresh();
        }
    }
}
