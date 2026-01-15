using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

public class ProductionBuilder
{
    [MenuItem("Build/Build To Production Folder")]
    public static void BuildGame()
    {
        // Path to the production folder
        // Application.dataPath gives us "AmDoUMinh/Assets"
        // Directory.GetParent gives us "AmDoUMinh"
        string projectPath = Directory.GetParent(Application.dataPath).FullName;
        string buildPath = Path.Combine(projectPath, "production", Application.productName + ".exe");
        string buildDir = Path.GetDirectoryName(buildPath);

        // Ensure production directory exists
        if (!Directory.Exists(buildDir))
        {
            Directory.CreateDirectory(buildDir);
        }

        // Get enabled scenes from Build Settings
        var scenes = EditorBuildSettings.scenes;
        System.Collections.Generic.List<string> enabledScenes = new System.Collections.Generic.List<string>();
        
        foreach (var scene in scenes)
        {
            if (scene.enabled)
            {
                enabledScenes.Add(scene.path);
            }
        }

        if (enabledScenes.Count == 0)
        {
            Debug.LogError("Error: No scenes are enabled in Build Settings. Go to File > Build Settings and ensure your scenes are added and checked.");
            EditorUtility.DisplayDialog("Build Failed", "No scenes are enabled in Build Settings. Please add scenes via File > Build Settings.", "OK");
            return;
        }

        Debug.Log($"Building {enabledScenes.Count} scenes to: {buildPath}");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = enabledScenes.ToArray();
        buildPlayerOptions.locationPathName = buildPath;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            EditorUtility.DisplayDialog("Build Succeeded", $"Game built successfully to:\n{buildPath}", "OK");
            EditorUtility.RevealInFinder(buildPath);
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("Build failed");
            EditorUtility.DisplayDialog("Build Failed", $"Build failed with {summary.totalErrors} errors. Check Console for details.", "OK");
        }
    }
}
