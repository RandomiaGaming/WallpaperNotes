using System;
using Microsoft.Win32;
namespace WallpaperShare
{
    public static class RegistryHelper
    {
        #region Public Static Variables
        public static bool AllowAltSeparator = true;
        #endregion
        #region Public Constants
        public const char SeparatorChar = '\\';
        public const string SeparatorString = "\\";

        public const char AltSeparatorChar = '/';
        public const string AltSeparatorString = "/";

        public const string ComputerName = "COMPUTER";
        public const string Computer32Name = "COMPUTER_32";
        public const string Computer64Name = "COMPUTER_64";

        public static readonly string[] ComputerNames = new string[] { "COMPUTER", "REGISTRY", "REG" };
        public static readonly string[] Computer32Names = new string[] { "COMPUTER_32", "REGISTRY_32", "REG_32", "COMPUTER32", "REGISTRY32", "REG32" };
        public static readonly string[] Computer64Names = new string[] { "COMPUTER_64", "REGISTRY_64", "REG_64", "COMPUTER64", "REGISTRY64", "REG64" };

        public const string HKEYClassesRootName = "HKEY_CLASSES_ROOT";
        public const string HKEYCurrentUserName = "HKEY_CURRENT_USER";
        public const string HKEYLocalMachineName = "HKEY_LOCAL_MACHINE";
        public const string HKEYUsersName = "HKEY_USERS";
        public const string HKEYCurrentConfigName = "HKEY_CURRENT_CONFIG";

        public static readonly string[] HKEYClassesRootNames = new string[] { "HKEY_CLASSES_ROOT", "CLASSES_ROOT", "HKEYCLASSESROOT", "CLASSESROOT", "HKCR" };
        public static readonly string[] HKEYCurrentUserNames = new string[] { "HKEY_CURRENT_USER", "CLASSES_ROOT", "HKEYCURRENTUSER", "CURRENTUSER", "HKCU" };
        public static readonly string[] HKEYLocalMachineNames = new string[] { "HKEY_LOCAL_MACHINE", "LOCAL_MACHINE", "HKEYLOCALMACHINE", "LOCALMACHINE", "HKLM" };
        public static readonly string[] HKEYUsersNames = new string[] { "HKEY_USERS", "USERS", "HKEYUSERS", "HKU" };
        public static readonly string[] HKEYCurrentConfigNames = new string[] { "HKEY_CURRENT_CONFIG", "CURRENT_CONFIG", "HKEYCURRENTCONFIG", "CURRENTCONFIG", "HKCC" };
        #endregion
        public static bool RegistryValueExists(string registryPath)
        {
            try
            {
                GetRegistryValue(registryValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static object GetRegistryValue(string registryPath)
        {
            object output = null;
            RegistryKeyPath registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, false);
            try
            {
                object value = registryKey.GetValue(registryValue.ValueName);
                if (value is null)
                {
                    throw new Exception("registryValue does not exist or could not be accessed.");
                }
                output = value;
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
            return output;
        }
        public static void SetRegistryValue(RegistryValueRefrence registryValue, object value, RegistryValueKind registryValueKind = RegistryValueKind.Unknown)
        {
            RegistryKeyPath registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, true);
            try
            {
                if (registryValueKind is RegistryValueKind.Unknown)
                {
                    registryKey.SetValue(registryValue.ValueName, value, GetObjectRegistryKind(value));
                }
                else
                {
                    registryKey.SetValue(registryValue.ValueName, value, registryValueKind);
                }
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }
        public static void CreateRegistryValue(string registryPath, object value, RegistryValueKind registryValueKind = RegistryValueKind.Unknown)
        {
            RegistryValuePath registryValuePath = new RegistryValuePath(registryPath);
            RegistryKey registryKey = CreateKey(registryPath, true);
            try
            {
                if (registryValueKind is RegistryValueKind.Unknown)
                {
                    registryKey.value
                    registryKey.SetValue(registryValue.ValueName, value, GetObjectRegistryKind(value));
                }
                else
                {
                    registryKey.SetValue(registryValue.ValueName, value, registryValueKind);
                }
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }
        public static bool KeyExists(string registryPath)
        {
            try
            {
                SafelyReleaseKey(OpenKey(registryPath, false));
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static RegistryKey DeleteKey(string registryPath, bool recursive = true)
        {
            RegistryKeyPath registryKeyPath = new RegistryKeyPath(registryPath);
            RegistryKey output;
            RegistryKey baseKey = OpenBase(registryPath);
            try
            {
                if (recursive)
                {
                    baseKey.DeleteSubKeyTree(registryKeyPath.SubKeyPath);
                }
                else
                {
                    baseKey.DeleteSubKey(registryKeyPath.SubKeyPath);
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        private static RegistryKey CreateKey(string registryPath, bool writable = false)
        {
            RegistryKeyPath registryKeyPath = new RegistryKeyPath(registryPath);
            RegistryKey output;
            RegistryKey baseKey = OpenBase(registryPath);
            try
            {
                output = baseKey.CreateSubKey(registryKeyPath.SubKeyPath, writable);
                if (output is null)
                {
                    throw new Exception("Registry key does not exist.");
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        private static RegistryKey OpenKey(string registryPath, bool writable = false)
        {
            RegistryKeyPath registryKeyPath = new RegistryKeyPath(registryPath);
            RegistryKey output;
            RegistryKey baseKey = OpenBase(registryPath);
            try
            {
                output = baseKey.OpenSubKey(registryKeyPath.SubKeyPath, writable);
                if (output is null)
                {
                    throw new Exception("Registry key does not exist.");
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        private static RegistryKey OpenBase(string registryPath)
        {
            RegistryBase registryBase = new RegistryBase(registryPath);

            RegistryView registryView;

            if (registryBase.RootName is RootName.COMPUTER_32)
            {
                registryView = RegistryView.Registry32;
            }
            else if (registryBase.RootName is RootName.COMPUTER_64)
            {
                registryView = RegistryView.Registry64;
            }
            else
            {
                registryView = RegistryView.Default;
            }

            if (registryBase.BaseName is BaseName.HKEY_CLASSES_ROOT)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView);
            }
            else if (registryBase.BaseName is BaseName.HKEY_CURRENT_CONFIG)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, registryView);
            }
            else if (registryBase.BaseName is BaseName.HKEY_CURRENT_USER)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            }
            else if (registryBase.BaseName is BaseName.HKEY_USERS)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.Users, registryView);
            }
            else
            {
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            }
        }
        public static void SafelyReleaseKey(RegistryKey registryKey)
        {
            try
            {
                registryKey.Flush();
            }
            catch
            {

            }
            try
            {
                registryKey.Close();
            }
            catch
            {

            }
            try
            {
                registryKey.Dispose();
            }
            catch
            {

            }
        }
        public static RegistryValueKind GetObjectRegistryKind(object data)
        {
            if (data is null)
            {
                return RegistryValueKind.Unknown;
                RegistryValueKind.
            }

            Type dataType = data.GetType();

            if (dataType == typeof(string))
            {
                return RegistryValueKind.String;
            }
            else if (dataType == typeof(long))
            {
                return RegistryValueKind.QWord;
            }
            else if (dataType == typeof(int))
            {
                return RegistryValueKind.DWord;
            }
            else if (dataType == typeof(byte[]))
            {
                return RegistryValueKind.Binary;
            }
            else if (dataType == typeof(string[]))
            {
                return RegistryValueKind.MultiString;
            }
            else
            {
                return RegistryValueKind.Unknown;
            }
        }
        public static string CombineRegistryPaths(string registryPathA, string registryPathB)
        {
            bool registryPathAEmpty = registryPathA is null || registryPathA is "";
            bool registryPathBEmpty = registryPathB is null || registryPathB is "";
            if (registryPathAEmpty && registryPathBEmpty)
            {
                return "";
            }
            else if (registryPathAEmpty && !registryPathBEmpty)
            {
                return registryPathB;
            }
            else if (!registryPathAEmpty && registryPathBEmpty)
            {
                return registryPathA;
            }
            else
            {
                if (registryPathA[registryPathA.Length - 1] == SeparatorChar || (AllowAltSeparator && registryPathA[registryPathA.Length - 1] == AltSeparatorChar))
                {
                    registryPathA = registryPathA.Substring(0, registryPathA.Length - 1);
                }
                if (registryPathB[0] == SeparatorChar || (AllowAltSeparator && registryPathB[0] == AltSeparatorChar))
                {
                    registryPathB = registryPathB.Substring(1, registryPathB.Length - 1);
                }

                if (registryPathA is "" && registryPathB is "")
                {
                    return "";
                }
                else if (registryPathA is "" && !(registryPathB is ""))
                {
                    return registryPathB;
                }
                else if (!(registryPathA is "") && registryPathB is "")
                {
                    return registryPathA;
                }
                else
                {
                    return registryPathA + SeparatorString + registryPathB;
                }
            }
        }
        private sealed class RegistryValuePath
        {
            #region Internal Variables
            internal readonly RegistryKeyPath RegistryKey;
            public RegistryBase RegistryBase
            {
                get
                {
                    return RegistryKey.RegistryBase;
                }
            }
            internal RegistryRoot RegistryRoot
            {
                get
                {
                    return RegistryBase.RegistryRoot;
                }
            }
            internal string[] IdentifierNames
            {
                get
                {
                    return RegistryRoot.IdentifierNames;
                }
            }
            internal RootName RootName
            {
                get
                {
                    return RegistryRoot.RootName;
                }
            }
            internal BaseName BaseName
            {
                get
                {
                    return RegistryBase.BaseName;
                }
            }
            internal string[] SubKeyNames
            {
                get
                {
                    return RegistryKey.SubKeyNames;
                }
            }
            internal string SubKeyPath
            {
                get
                {
                    return RegistryKey.SubKeyPath;
                }
            }
            internal readonly string ValueName;
            #endregion
            #region Internal Constructors
            internal RegistryValuePath(string registryPath)
            {
                RegistryKey = new RegistryKeyPath(registryPath, true);
                ValueName = IdentifierNames[IdentifierNames.Length - 1];
            }
            #endregion
        }
        private sealed class RegistryKeyPath
        {
            #region Internal Variables
            internal readonly RegistryBase RegistryBase;
            internal RegistryRoot RegistryRoot
            {
                get
                {
                    return RegistryBase.RegistryRoot;
                }
            }
            internal string[] IdentifierNames
            {
                get
                {
                    return RegistryRoot.IdentifierNames;
                }
            }
            internal RootName RootName
            {
                get
                {
                    return RegistryRoot.RootName;
                }
            }
            internal BaseName BaseName
            {
                get
                {
                    return RegistryBase.BaseName;
                }
            }
            internal readonly string[] SubKeyNames;
            internal readonly string SubKeyPath;
            #endregion
            #region internal Constructors
            internal RegistryKeyPath(string registryPath, bool lastSubKeyIsValue = false)
            {
                RegistryBase = new RegistryBase(registryPath);
                if (lastSubKeyIsValue)
                {
                    //Check that the registryRoot has enough identifier names for at least one sub key path.
                    if (RegistryBase.IdentifierNames.Length < 4)
                    {
                        throw new Exception("Not enough identifiers were supplied in registry path.");
                    }
                    //Create an empty array of subKeyPaths of the correct length.
                    SubKeyNames = new string[RegistryBase.IdentifierNames.Length - 3];
                    //Copy the sub key paths to the newly created array.
                    Array.Copy(RegistryBase.IdentifierNames, 2, SubKeyNames, 0, SubKeyNames.Length);
                }
                else
                {
                    //Check that the registryRoot has enough identifier names for at least one sub key path.
                    if (RegistryBase.IdentifierNames.Length < 3)
                    {
                        throw new Exception("Not enough identifiers were supplied in registry path.");
                    }
                    //Create an empty array of subKeyPaths of the correct length.
                    SubKeyNames = new string[RegistryBase.IdentifierNames.Length - 2];
                    //Copy the sub key paths to the newly created array.
                    Array.Copy(RegistryBase.IdentifierNames, 2, SubKeyNames, 0, SubKeyNames.Length);
                }
                //Calculate the managed path.
                for (int i = 0; i < SubKeyNames.Length; i++)
                {
                    if (i is 0)
                    {
                        SubKeyPath += SubKeyNames[i];
                    }
                    else
                    {
                        SubKeyPath += SeparatorString + SubKeyNames[i];
                    }
                }
            }
            #endregion
        }
        private sealed class RegistryBase
        {
            #region Internal Variables
            internal readonly RegistryRoot RegistryRoot;
            internal string[] IdentifierNames
            {
                get
                {
                    return RegistryRoot.IdentifierNames;
                }
            }
            internal RootName RootName
            {
                get
                {
                    return RegistryRoot.RootName;
                }
            }
            internal readonly BaseName BaseName;
            #endregion
            #region Internal Constructors
            internal RegistryBase(string registryPath)
            {
                RegistryRoot = new RegistryRoot(registryPath);
                //Check that the registryRoot has enough identifier names to specify a baseName.
                if (RegistryRoot.IdentifierNames.Length < 2)
                {
                    throw new Exception("Registry path does not specify a base name.");
                }
                //Get the base key name string in all uppercase.
                string baseNameToUpper = RegistryRoot.IdentifierNames[1].ToUpper();
                //Check it agains the valid base key names and throw an exception if none match. If a match is found select that BaseName.
                if (StringWithinArray(baseNameToUpper, HKEYClassesRootNames))
                {
                    BaseName = BaseName.HKEY_CLASSES_ROOT;
                }
                else if (StringWithinArray(baseNameToUpper, HKEYCurrentUserNames))
                {
                    BaseName = BaseName.HKEY_CURRENT_USER;
                }
                else if (StringWithinArray(baseNameToUpper, HKEYLocalMachineNames))
                {
                    BaseName = BaseName.HKEY_LOCAL_MACHINE;
                }
                else if (StringWithinArray(baseNameToUpper, HKEYUsersNames))
                {
                    BaseName = BaseName.HKEY_USERS;
                }
                else if (StringWithinArray(baseNameToUpper, HKEYCurrentConfigNames))
                {
                    BaseName = BaseName.HKEY_CURRENT_CONFIG;
                }
                else
                {
                    throw new Exception("Registry path specified a base key name which does not exist.");
                }
            }
            #endregion
        }
        public static ValueType GetValueType(object value)
        {
            if (value is null)
            {
                throw new Exception("Type of value is not supported.");
            }

            Type dataType = data.GetType();

            if (dataType == typeof(string))
            {
                return ValueType.EXPAND_SZ;
            }
            else if (dataType == typeof(long))
            {
                return ValueType.QWORD;
            }
            else if (dataType == typeof(int))
            {
                return ValueType.DWORD;
            }
            else if (dataType == typeof(byte[]))
            {
                return RegistryValueKind.Binary;
            }
            else if (dataType == typeof(string[]))
            {
                return ValueType.MULTI_SZ;
            }
            else
            {
                throw new Exception("Type of value is not supported.");
            }
        }
        private enum ValueType
        {
            BINARY,
            DWORD,
            QWORD,
            SZ,
            MULTI_SZ,
            EXPAND_SZ
        }
        private enum BaseName
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS,
            HKEY_CURRENT_CONFIG,
        }
        private class RegistryRoot
        {
            #region Internal Variables
            internal readonly string[] IdentifierNames;
            internal readonly RootName RootName;
            #endregion
            #region Internal Constructors
            internal RegistryRoot(string registryPath)
            {
                //Check if the registry path is null so we dont get a null refrence exception when attempting to split the string.
                if (registryPath is null)
                {
                    throw new Exception("Registry path cannot be null.");
                }
                //If specified allow for / to be used in place of \
                if (AllowAltSeparator)
                {
                    registryPath = registryPath.Replace(AltSeparatorString, SeparatorString);
                }
                //Split the registry path on each seporator char.
                IdentifierNames = registryPath.Split(SeparatorChar);
                //Check if identifierNames is empty.
                if (IdentifierNames.Length is 0)
                {
                    //Set the rootType to the default.
                    RootName = RootName.COMPUTER;
                    //Append the default root name.
                    IdentifierNames = new string[] { ComputerName };
                }
                else
                {
                    //Trim the first identifier if it is empty.
                    if (IdentifierNames[0] is "")
                    {
                        if (IdentifierNames.Length is 1)
                        {
                            throw new Exception("Registry path contained no real information.");
                        }
                        string[] trimStartIdentifierNames = new string[IdentifierNames.Length - 1];
                        Array.Copy(IdentifierNames, 1, trimStartIdentifierNames, 0, trimStartIdentifierNames.Length);
                        IdentifierNames = trimStartIdentifierNames;
                    }
                    //Trim the last identifier if it is empty.
                    if (IdentifierNames[IdentifierNames.Length - 1] is "")
                    {
                        if (IdentifierNames.Length is 1)
                        {
                            throw new Exception("Registry path contained no real information.");
                        }
                        string[] trimEndIdentifierNames = new string[IdentifierNames.Length - 1];
                        Array.Copy(IdentifierNames, 0, trimEndIdentifierNames, 0, trimEndIdentifierNames.Length);
                        IdentifierNames = trimEndIdentifierNames;
                    }
                    //Check the identifier names for empty elements.
                    for (int i = 0; i < IdentifierNames.Length; i++)
                    {
                        if (IdentifierNames[i] is "")
                        {
                            throw new Exception("Registry path contained empty identifiers.");
                        }
                    }
                    //Get the root name with its original case and in all uppercase.
                    string rootNameToUpper = IdentifierNames[0].ToUpper();
                    //Check to see if the rootName is a valid RootType and if so then assign the propper RootTyp if not then append the defualt root name to the start of the identifier names.
                    if (StringWithinArray(rootNameToUpper, ComputerNames))
                    {
                        RootName = RootName.COMPUTER;
                    }
                    else if (StringWithinArray(rootNameToUpper, Computer32Names))
                    {
                        RootName = RootName.COMPUTER_32;
                    }
                    else if (StringWithinArray(rootNameToUpper, Computer64Names))
                    {
                        RootName = RootName.COMPUTER_64;
                    }
                    else
                    {
                        //Set the rootType to the default.
                        RootName = RootName.COMPUTER;
                        //Resize the identifierNames to make room for the defualt root name which we are about to append.
                        string[] resizedIdentifierNames = new string[IdentifierNames.Length + 1];
                        Array.Copy(IdentifierNames, 0, resizedIdentifierNames, 1, IdentifierNames.Length);
                        IdentifierNames = resizedIdentifierNames;
                        //Append the default root name.
                        IdentifierNames[0] = ComputerName;
                    }
                }
            }
            #endregion
        }
        private enum RootName
        {
            COMPUTER,
            COMPUTER_32,
            COMPUTER_64
        }
        private string[] GetIdentifiers(string registryPath)
        {
            //Check if the registry path is null so we dont get a null refrence exception when attempting to split the string.
            if (registryPath is null)
            {
                throw new Exception("Registry path cannot be null.");
            }
            //If specified allow for / to be used in place of \
            if (AllowAltSeparator)
            {
                registryPath = registryPath.Replace(AltSeparatorChar, SeparatorChar);
            }
            //Split the registry path on each seporator char.
            string[] IdentifierNames = registryPath.Split(SeparatorChar);
            //Check if identifierNames is empty.
            if (IdentifierNames.Length is 0)
            {
                //Set the rootType to the default.
                RootName = RootName.COMPUTER;
                //Append the default root name.
                IdentifierNames = new string[] { ComputerName };
            }
            else
            {
                //Trim the first identifier if it is empty.
                if (IdentifierNames[0] is "")
                {
                    if (IdentifierNames.Length is 1)
                    {
                        throw new Exception("Registry path contained no real information.");
                    }
                    string[] trimStartIdentifierNames = new string[IdentifierNames.Length - 1];
                    Array.Copy(IdentifierNames, 1, trimStartIdentifierNames, 0, trimStartIdentifierNames.Length);
                    IdentifierNames = trimStartIdentifierNames;
                }
                //Trim the last identifier if it is empty.
                if (IdentifierNames[IdentifierNames.Length - 1] is "")
                {
                    if (IdentifierNames.Length is 1)
                    {
                        throw new Exception("Registry path contained no real information.");
                    }
                    string[] trimEndIdentifierNames = new string[IdentifierNames.Length - 1];
                    Array.Copy(IdentifierNames, 0, trimEndIdentifierNames, 0, trimEndIdentifierNames.Length);
                    IdentifierNames = trimEndIdentifierNames;
                }
                //Check the identifier names for empty elements.
                for (int i = 0; i < IdentifierNames.Length; i++)
                {
                    if (IdentifierNames[i] is "")
                    {
                        throw new Exception("Registry path contained empty identifiers.");
                    }
                }
                //Get the root name with its original case and in all uppercase.
                string rootNameToUpper = IdentifierNames[0].ToUpper();
                //Check to see if the rootName is a valid RootType and if so then assign the propper RootTyp if not then append the defualt root name to the start of the identifier names.
                if (StringWithinArray(rootNameToUpper, ComputerNames))
                {
                    RootName = RootName.COMPUTER;
                }
                else if (StringWithinArray(rootNameToUpper, Computer32Names))
                {
                    RootName = RootName.COMPUTER_32;
                }
                else if (StringWithinArray(rootNameToUpper, Computer64Names))
                {
                    RootName = RootName.COMPUTER_64;
                }
                else
                {
                    //Set the rootType to the default.
                    RootName = RootName.COMPUTER;
                    //Resize the identifierNames to make room for the defualt root name which we are about to append.
                    string[] resizedIdentifierNames = new string[IdentifierNames.Length + 1];
                    Array.Copy(IdentifierNames, 0, resizedIdentifierNames, 1, IdentifierNames.Length);
                    IdentifierNames = resizedIdentifierNames;
                    //Append the default root name.
                    IdentifierNames[0] = ComputerName;
                }
            }
        }
        private static bool StringWithinArray(string target, string[] array)
        {
            if (array is null)
            {
                throw new Exception("array cannot be null.");
            }
            int localArrayLength = array.Length;
            for (int i = 0; i < localArrayLength; i++)
            {
                if (target == array[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}