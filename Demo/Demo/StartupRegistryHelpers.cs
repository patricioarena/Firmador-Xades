using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

internal static class StartupRegistryHelpers
{
    private const string ApplicationName = "Authentica";
    private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string RegistryValueName = "AuthenticaLauncher";
    private const string AuthenticaConfigPath = @"Software\AuthenticaApp";
    private const string FirstRunKey = "FirstRun";

    public static void RegisterStartupScript(bool isChecked)
    {
        // made by chatgpt
        string appPath = GetClickOnceShortcut();//Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "Authentica.appref-ms");
        string batchFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "launch_authentica.bat");

        // Create batch script content
        string batchContent = $@"
                @echo off
                if exist ""HKCU\{RegistryKeyPath}"" /v ""{ApplicationName}"" >nul 2>&1 (
                    reg delete ""HKCU\{RegistryKeyPath}"" /v ""{ApplicationName}"" /f
                )

                if exist ""{appPath}"" (
                    start """" ""{appPath}""
                ) else (
                    reg delete HKCU\{RegistryKeyPath} /v {RegistryValueName} /f
                    reg delete HKCU\{AuthenticaConfigPath} /f
                )
                ";

        // Write batch script
        File.WriteAllText(batchFilePath, batchContent);

        if (string.IsNullOrEmpty(batchFilePath) || !File.Exists(batchFilePath))
        {
            MessageBox.Show("Could not find the application shortcut. Please check installation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        //string command = $"cmd.exe /c start \"\" \"{shortcut}\"";

        if (isChecked)
        {
            // Add registry entry pointing to the batch script
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue(RegistryValueName, batchFilePath, RegistryValueKind.String);
                }
            }
        }
        else
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                key?.DeleteValue(RegistryValueName, false);
            }
        }
    }

    public static string GetClickOnceShortcut()
    {
        string localAppsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs));

        if (Directory.Exists(localAppsPath))
        {

            var possibleFolders = Directory.GetFiles(localAppsPath, "*", SearchOption.AllDirectories)
                                         .Where(f => Path.GetFileName(f).Contains(ApplicationName + ".appref-ms", StringComparison.OrdinalIgnoreCase))
                                         .ToList();

            if (possibleFolders.Count > 0)
            {
                return possibleFolders[0]; // Return first found ClickOnce app folder
            }
        }

        return "";
    }

    public static bool GetIniciarConWindows()
    {
        using (RegistryKey registry = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
        {
            List<string> names = registry.GetValueNames().ToList();
            return names.Contains(RegistryValueName).Equals(true);
        }
    }
    public static bool IsFirstRun()
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(AuthenticaConfigPath, true))
        {
            if (key == null || key.GetValue(FirstRunKey) == null)
            {
                using (RegistryKey newKey = Registry.CurrentUser.CreateSubKey(AuthenticaConfigPath))
                {
                    newKey.SetValue(FirstRunKey, "1", RegistryValueKind.String);
                }
                return true;
            }
        }
        return false;
    }
}