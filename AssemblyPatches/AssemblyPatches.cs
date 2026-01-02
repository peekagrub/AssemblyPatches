using MonoMod;
using Patches.Modifiers;
using System;
using System.IO;
using UnityEngine;

#pragma warning disable CS0626

namespace Patches;

[MonoModPatch("global::GameManager")]
public class GameManagerPatch : global::GameManager
{
    public Configuration Config = new();

    private void OnGUI()
    {
        if (this.GetSceneNameString() == Constants.MENU_SCENE)
        {
            var oldBackgroundColor = GUI.backgroundColor;
            var oldContentColor = GUI.contentColor;
            var oldColor = GUI.color;
            var oldMatrix = GUI.matrix;

            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
            GUI.color = Color.white;
            GUI.matrix = Matrix4x4.TRS(
                Vector3.zero,
                Quaternion.identity,
                new Vector3(Screen.width / 1920f, Screen.height / 1080f, 1f)
            );

            string WarningText = string.Empty;
            else if (Config.MiniSaveStates && Config.ScreenShakeModifier)
            {
                WarningText = "MiniSaveStates and ScreenShakeModifier Active";
            }
            else if (Config.MiniSaveStates)
            {
                WarningText = "MiniSaveStates Active";
            }
            else if (Config.ScreenShakeModifier)
            {
                WarningText = "ScreenShakeModifier Active";
            }

            WarningText += "\nRuntime Patches";

            GUI.Label(
                new Rect(20f, 20f, 200f, 200f),
                WarningText,
                new GUIStyle
                {
                    fontSize = 30,
                    normal = new GUIStyleState
                    {
                        textColor = Color.white,
                    }
                }
            );

            GUI.backgroundColor = oldBackgroundColor;
            GUI.contentColor = oldContentColor;
            GUI.color = oldColor;
            GUI.matrix = oldMatrix;
        }
    }

    public void Update()
    {
        if (!IsMiniSaveStatesActive) return;
        if (Input.GetKeyDown(SaveStateManager.Keybinds.SaveStateButton))
        {
            SaveStateManager.SaveState();
        }
        else if (Input.GetKeyDown(SaveStateManager.Keybinds.LoadStateButton))
        {
            SaveStateManager.LoadState();
        }
    }

    public static string ConfigPath => Path.Combine(Application.persistentDataPath, "assemblyPatchesConfiguration.json");

    public extern void orig_Start();

    public void Start()
    {
        try
        {
            if (!File.Exists(ConfigPath))
            {
                File.WriteAllText(ConfigPath, JsonUtility.ToJson(Config, true));
            }

            Config = JsonUtility.FromJson<Configuration>(File.ReadAllText(ConfigPath));

            if (Constants.GAME_VERSION.StartsWith("1.5"))
            {
                Config.ScreenShakeModifier = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        orig_Start();
        if (Config.MiniSaveStates) SaveStateManager.LoadKeybinds();
        if (Config.ScreenShakeModifier) ScreenShakeModifier.EditScreenShake();
    }
}
