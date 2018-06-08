using System.IO;
using UnityEditor;
using UnityEngine;

public class MakerMenu
{
    [MenuItem("Assets/导出动画", false)]
    static void Export()
    {
        AnimatorMaker am = new AnimatorMaker(Selection.activeGameObject);
        am.Bake();
    }
}
