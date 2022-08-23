using System;
using Microsoft.Win32;
namespace WallpaperShare
{
    public static class RegistryHelper
    {
        public static void Yeet()
        {
            AllowAlternateSeparator = true;
            var t = new RegistryBase("COMPUTER_64\\Yeeee/yeet\\yote/yee");
        }
        #region Public Static Variables
        public static bool AllowAlternateSeparator = true;
        #endregion
        #region Public Constants
        public const char SeparatorChar = '\\';
        public const string SeparatorString = "\\";

        public const char AlternateSeparatorChar = '/';
        public const string AlternateSeparatorString = "/";

        public const string ComputerName = "COMPUTER";

        public static readonly string[] ComputerNames = new string[] { "COMPUTER", "COMP", "COM", "REGISTRY", "REG" };
        public static readonly string[] Computer32Names = new string[] { "COMPUTER_32", "COMP_32", "COM_32", "REGISTRY_32", "REG_32", "COMPUTER32", "COMP32", "COM32", "REGISTRY32", "REG32" };
        public static readonly string[] Computer64Names = new string[] { "COMPUTER_64", "COMP_64", "COM_64", "REGISTRY_64", "REG_64", "COMPUTER64", "COMP64", "COM64", "REGISTRY64", "REG64" };

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
        /*
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
            RegistryKey registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, false);
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
            RegistryKey registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, true);
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
        public static void CreateRegistryValue(RegistryValueRefrence registryValue, object value, RegistryValueKind registryValueKind = RegistryValueKind.Unknown)
        {
            RegistryKey registryKey = CreateRegistryKey(registryValue.ParentKeyRefrence, true);
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

        public static void DeleteRegistryKey(string registryPath)
        {
            DeleteRegistryKeyInternal(ParseRegistryKeyPath(registryPath));
        }
        public static RegistryKey CreateRegistryKey(string registryPath, bool writable = false)
        {
            return CreateRegistryKeyInternal(ParseRegistryKeyPath(registryPath), writable);
        }
        public static bool RegistryKeyExists(string registryPath)
        {
            try
            {
                SafelyReleaseKey(OpenRegistryKey(registryPath, false));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static RegistryKey OpenRegistryKey(string registryPath, bool writable = false)
        {
            return OpenRegistryKeyInternal(ParseRegistryKeyPath(registryPath), writable);
        }


        private static void DeleteRegistryKeyInternal(RegistryKeyPath registryPath)
        {
            RegistryKey baseKey = OpenBaseInternal(new RegistryBasePath(registryPath.BaseName, registryPath.RegistryView));
            try
            {
                baseKey.DeleteSubKeyTree(registryPath.SubKeyPath, true);
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
        }
        private static RegistryKey CreateRegistryKeyInternal(RegistryKeyPath registryPath, bool writable)
        {
            RegistryKey output;
            RegistryKey baseKey = OpenBaseInternal(new RegistryBasePath(registryPath.BaseName, registryPath.RegistryView));
            try
            {
                RegistryKey subKey = baseKey.CreateSubKey(registryPath.SubKeyPath, writable);
                if (subKey is null)
                {
                    throw new Exception("Registry key could not be created.");
                }
                output = subKey;
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        private static RegistryKey OpenRegistryKeyInternal(RegistryKeyPath registryPath, bool writable)
        {
            RegistryKey output;
            RegistryKey baseKey = OpenBaseInternal(new RegistryBasePath(registryPath.BaseName, registryPath.RegistryView));
            try
            {
                RegistryKey subKey = baseKey.OpenSubKey(registryPath.SubKeyPath, writable);
                if (subKey is null)
                {
                    throw new Exception("Registry key does not exist.");
                }
                output = subKey;
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }

        public static bool BaseExists(string registryPath)
        {
            return BaseExists(new RegistryBasePath(registryPath));
        }
        public static bool BaseExists(RegistryBasePath registryPath)
        {
            try
            {
                SafelyReleaseKey(OpenBase(registryPath));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static RegistryKey OpenBase(string registryPath)
        {
            return OpenBase(new RegistryBasePath(registryPath));
        }
        private static RegistryKey OpenBase(RegistryBasePath registryPath)
        {
            if (registryPath == null)
            {
                throw new Exception("registryPath cannot be null.");
            }

            string baseKeyNameToUpper = registryPath.BaseName.ToUpper();

            if (StringWithinArray(baseKeyNameToUpper, HKEYClassesRootNames))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryPath.RegistryView);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentUserNames))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryPath.RegistryView);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYLocalMachineNames))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryPath.RegistryView);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYUsersNames))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.Users, registryPath.RegistryView);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentConfigNames))
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, registryPath.RegistryView);
            }

            else if (StringWithinArray(baseKeyNameToUpper, HKEYClassesRoot32Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry64)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentUser32Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry64)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYLocalMachine32Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry64)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYUsers32Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry64)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry32);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentConfig32Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry64)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry32);
            }

            else if (StringWithinArray(baseKeyNameToUpper, HKEYClassesRoot64Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry32)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentUser64Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry32)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYLocalMachine64Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry32)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYUsers64Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry32)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry64);
            }
            else if (StringWithinArray(baseKeyNameToUpper, HKEYCurrentConfig64Names))
            {
                if (registryPath.RegistryView is RegistryView.Registry32)
                {
                    throw new Exception("Ambiguous registry view.");
                }
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry64);
            }

            else
            {
                throw new Exception("Base key with given name does not exist.");
            }
        }
        #region Other Registry Helpers
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
                return registryPathA + SeparatorString + registryPathB;
            }
        }
        #endregion*/
        private sealed class RegistryValue
        {
            #region Internal Variables
            internal readonly RegistryKey RegistryKey;
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
            internal RegistryValue(string registryPath)
            {
                RegistryKey = new RegistryKey(registryPath);
                ValueName = IdentifierNames[IdentifierNames.Length - 1];
            }
            #endregion
        }
        private sealed class RegistryKey
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
            internal RegistryKey(string registryPath, bool lastSubKeyIsValue = false)
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
                    Array.Copy(RegistryBase.IdentifierNames, 2, SubKeyNames, 0, SubKeyNames.Length - 1);
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
        private enum BaseName
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS,
            HKEY_CURRENT_CONFIG,
        }
        private sealed class RegistryRoot
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
                if (AllowAlternateSeparator)
                {
                    registryPath = registryPath.Replace(AlternateSeparatorString, SeparatorString);
                }
                //Split the registry path on each seporator char.
                IdentifierNames = registryPath.Split(SeparatorChar);
                //Check if identifierNames is empty.
                if (IdentifierNames.Length is 0)
                {
                    //Set the rootType to the default.
                    RootName = RootName.Computer;
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
                        RootName = RootName.Computer;
                    }
                    else if (StringWithinArray(rootNameToUpper, Computer32Names))
                    {
                        RootName = RootName.Computer_32;
                    }
                    else if (StringWithinArray(rootNameToUpper, Computer64Names))
                    {
                        RootName = RootName.Computer_64;
                    }
                    else
                    {
                        //Set the rootType to the default.
                        RootName = RootName.Computer;
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
            Computer,
            Computer_32,
            Computer_64
        }
        #region From String Helper
        internal static bool StringWithinArray(string target, string[] array)
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
        #endregion
    }
}