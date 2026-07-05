using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InspectorLockToggle
{
    [MenuItem("Tools/Toggle Inspector Lock #&l")] // ¥‹√‡≈∞: Shift + Alt + L
    private static void ToggleLock()
    {
        ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
        tracker.isLocked = !tracker.isLocked;
        tracker.ForceRebuild();
    }
}