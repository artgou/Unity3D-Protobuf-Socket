using UnityEngine;
using System.Collections;
using UnityEditor;

public class LRAudioImrter : AssetPostprocessor {
    /// <summary>
    /// 设置音频导入默认为２D声音
    /// </summary>
    void OnPreprocessAudio() {
        AudioImporter audioImporter = (AudioImporter)assetImporter;
        if ( audioImporter.threeD ) {
            audioImporter.threeD = false;
        }
    }
}
