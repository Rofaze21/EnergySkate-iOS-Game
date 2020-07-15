//  Copyright (c) 2018-present amlovey
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace uCodeEditor
{
    /// <summary>
    /// Key code for Monaco Editor 
    /// </summary>
    public class KeyCode
    {
        public const int Unknown = 0;

        public const int Backspace = 1;
        public const int Tab = 2;
        public const int Enter = 3;
        public const int PauseBreak = 7;
        public const int CapsLock = 8;
        public const int Escape = 9;
        public const int Space = 10;
        public const int PageUp = 11;
        public const int PageDown = 12;
        public const int End = 13;
        public const int Home = 14;
        public const int LeftArrow = 15;
        public const int UpArrow = 16;
        public const int RightArrow = 17;
        public const int DownArrow = 18;
        public const int Insert = 19;
        public const int Delete = 20;

        public const int KEY_0 = 21;
        public const int KEY_1 = 22;
        public const int KEY_2 = 23;
        public const int KEY_3 = 24;
        public const int KEY_4 = 25;
        public const int KEY_5 = 26;
        public const int KEY_6 = 27;
        public const int KEY_7 = 28;
        public const int KEY_8 = 29;
        public const int KEY_9 = 30;

        public const int KEY_A = 31;
        public const int KEY_B = 32;
        public const int KEY_C = 33;
        public const int KEY_D = 34;
        public const int KEY_E = 35;
        public const int KEY_F = 36;
        public const int KEY_G = 37;
        public const int KEY_H = 38;
        public const int KEY_I = 39;
        public const int KEY_J = 40;
        public const int KEY_K = 41;
        public const int KEY_L = 42;
        public const int KEY_M = 43;
        public const int KEY_N = 44;
        public const int KEY_O = 45;
        public const int KEY_P = 46;
        public const int KEY_Q = 47;
        public const int KEY_R = 48;
        public const int KEY_S = 49;
        public const int KEY_T = 50;
        public const int KEY_U = 51;
        public const int KEY_V = 52;
        public const int KEY_W = 53;
        public const int KEY_X = 54;
        public const int KEY_Y = 55;
        public const int KEY_Z = 56;

        public const int Meta = 57;
        public const int ContextMenu = 58;

        public const int F1 = 59;
        public const int F2 = 60;
        public const int F3 = 61;
        public const int F4 = 62;
        public const int F5 = 63;
        public const int F6 = 64;
        public const int F7 = 65;
        public const int F8 = 66;
        public const int F9 = 67;
        public const int F10 = 68;
        public const int F11 = 69;
        public const int F12 = 70;
        public const int F13 = 71;
        public const int F14 = 72;
        public const int F15 = 73;
        public const int F16 = 74;
        public const int F17 = 75;
        public const int F18 = 76;
        public const int F19 = 77;

        public const int NumLock = 78;
        public const int ScrollLock = 79;

        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the ';:' key
         */
        public const int US_SEMICOLON = 80;
        /**
         * For any country/region, the '+' key
         * For the US standard keyboard, the '=+' key
         */
        public const int US_EQUAL = 81;
        /**
         * For any country/region, the ',' key
         * For the US standard keyboard, the ',<' key
         */
        public const int US_COMMA = 82;
        /**
         * For any country/region, the '-' key
         * For the US standard keyboard, the '-_' key
         */
        public const int US_MINUS = 83;
        /**
         * For any country/region, the '.' key
         * For the US standard keyboard, the '.>' key
         */
        public const int US_DOT = 84;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the '/?' key
         */
        public const int US_SLASH = 85;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the '`~' key
         */
        public const int US_BACKTICK = 86;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the '[{' key
         */
        public const int US_OPEN_SQUARE_BRACKET = 87;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the '\|' key
         */
        public const int US_BACKSLASH = 88;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the ']}' key
         */
        public const int US_CLOSE_SQUARE_BRACKET = 89;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         * For the US standard keyboard, the ''"' key
         */
        public const int US_QUOTE = 90;
        /**
         * Used for miscellaneous characters; it can vary by keyboard.
         */
        public const int OEM_8 = 91;
        /**
         * Either the angle bracket key or the backslash key on the RT 102-key keyboard.
         */
        public const int OEM_102 = 92;

        public const int NUMPAD_0 = 93; // VK_NUMPAD0, 0x60, Numeric keypad 0 key
        public const int NUMPAD_1 = 94; // VK_NUMPAD1, 0x61, Numeric keypad 1 key
        public const int NUMPAD_2 = 95; // VK_NUMPAD2, 0x62, Numeric keypad 2 key
        public const int NUMPAD_3 = 96; // VK_NUMPAD3, 0x63, Numeric keypad 3 key
        public const int NUMPAD_4 = 97; // VK_NUMPAD4, 0x64, Numeric keypad 4 key
        public const int NUMPAD_5 = 98; // VK_NUMPAD5, 0x65, Numeric keypad 5 key
        public const int NUMPAD_6 = 99; // VK_NUMPAD6, 0x66, Numeric keypad 6 key
        public const int NUMPAD_7 = 100; // VK_NUMPAD7, 0x67, Numeric keypad 7 key
        public const int NUMPAD_8 = 101; // VK_NUMPAD8, 0x68, Numeric keypad 8 key
        public const int NUMPAD_9 = 102; // VK_NUMPAD9, 0x69, Numeric keypad 9 key

        public const int NUMPAD_MULTIPLY = 103;  // VK_MULTIPLY, 0x6A, Multiply key
        public const int NUMPAD_ADD = 104;       // VK_ADD, 0x6B, Add key
        public const int NUMPAD_SEPARATOR = 105; // VK_SEPARATOR, 0x6C, Separator key
        public const int NUMPAD_SUBTRACT = 106;  // VK_SUBTRACT, 0x6D, Subtract key
        public const int NUMPAD_DECIMAL = 107;   // VK_DECIMAL, 0x6E, Decimal key
        public const int NUMPAD_DIVIDE = 108;    // VK_DIVIDE, 0x6F,

        /**
         * Cover all key codes when IME is processing input.
         */
        public const int KEY_IN_COMPOSITION = 109;

        public const int ABNT_C1 = 110; // Brazilian (ABNT) Keyboard
        public const int ABNT_C2 = 111; // Brazilian (ABNT) Keyboard
        public const int CtrlCmd = 2048;
        public const int Shift = 1024;
        public const int Alt = 512;
    }

    /// <summary>
    /// Define the command id, name and shortcuts
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class UCommandAttribute : Attribute
    {
        /// <summary>
        /// The id of the command
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The label of the command, it's the display name in Command Palette.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Shortcut for this command. -1 means no shortcut. 
        /// </summary>
        public int Shortcut { get; set; }

        public UCommandAttribute(string id, string label, int shortcut = -1)
        {
            this.Id = id;
            this.Label = label;
            this.Shortcut = shortcut;
        }

        /// <summary>
        /// To Json string
        /// </summary>
        public virtual string ToJsonString()
        {
            return string.Format(@"{{""id"":""{0}"",""shortcut"":""{1}"",""label"":""{2}""}}", this.Id, this.Shortcut, this.Label);
        }
    }

    public class UContextMenuCommand : UCommandAttribute
    {
        /// <summary>
        /// Menu item order in context menu group. Please set value that larger than 0.
        /// </summary>
        public int ContextMenuOrder;

        /// <summary>
        /// Menu item group. Item will not show in Context menu if it's empty.
        /// </summary>
        public string ContextMenuGroup;

        public UContextMenuCommand(string id, string label, int contextMenuOrder, string contextGroup, int shortcut = -1)
            : base(id, label, shortcut)
        {
            this.ContextMenuOrder = contextMenuOrder;
            this.ContextMenuGroup = contextGroup;
        }

        /// <summary>
        /// To Json string
        /// </summary>
        public override string ToJsonString()
        {
            return string.Format(@"{{""id"":""{0}"",""shortcut"":""{1}"",""label"":""{2}"",""contextMenuOrder"":{3},""contextMenuGroup"":""{4}""}}",
                        this.Id, this.Shortcut, this.Label, this.ContextMenuOrder, this.ContextMenuGroup);
        }
    }

    [UnityEditor.InitializeOnLoad]
    public class UCommandController
    {
        private static Dictionary<string, UDynmaicCommand> DynamicCommands = new Dictionary<string, UDynmaicCommand>();
        private static Dictionary<string, UQuickOpenCommand> QuickOpenCommands = new Dictionary<string, UQuickOpenCommand>();

        static UCommandController()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                if (item.GetName().Name.Equals("Assembly-CSharp-Editor", StringComparison.OrdinalIgnoreCase)
                    || item.GetName().Name.Equals("uCodeEditor"))
                {
                    var types = item.GetTypes();
                    foreach (Type t in types)
                    {
                        if (t.IsSubclassOf(typeof(UDynmaicCommand)))
                        {
                            Register(Activator.CreateInstance(t), true);
                        }

                        if (t.IsSubclassOf(typeof(UQuickOpenCommand)))
                        {
                            Register(Activator.CreateInstance(t), false);
                        }
                    }
                }
            }
        }

        public static string GetAllDynamicCommandsJson()
        {
            var allJson = new List<string>();
            foreach (var item in DynamicCommands.Values)
            {
                allJson.Add(item.GetCommandConfigJson());
            }

            return string.Format("[{0}]", string.Join(",", allJson.ToArray()));
        }

        public static string GetAllQuickOpenCommandsJson()
        {
            var allJson = new List<string>();
            foreach (var item in QuickOpenCommands.Values)
            {
                allJson.Add(item.GetCommandConfigJson());
            }

            return string.Format("[{0}]", string.Join(",", allJson.ToArray()));
        }

        private static string GetId(string baseId)
        {
            return string.Format("uce.cmd.{0}", baseId);
        }

        public static void RunDynamicCommands(string id)
        {
            var cmdId = GetId(id);
            if (DynamicCommands.ContainsKey(cmdId))
            {
                DynamicCommands[cmdId].Run();
            }
        }

        public static void RunQuickOpenEntry(string commandId, string entryId)
        {
            var id = GetId(commandId);
            if (QuickOpenCommands.ContainsKey(id))
            {
                QuickOpenCommands[id].RunEntry(entryId);
            }
        }

        public static IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries(string id)
        {
            var cmd = GetId(id);
            if (QuickOpenCommands.ContainsKey(cmd))
            {
                return QuickOpenCommands[cmd].GetQuickOpenEntries();
            }

            return null;
        }

        public static void Register(object cmd, bool isDynamic)
        {
            var attributes = cmd.GetType().GetCustomAttributes(typeof(UCommandAttribute), false);

            if (attributes.Length <= 0)
            {
                throw new Exception("Command needs UCommandAttribute");
            }

            var attr = attributes[0] as UCommandAttribute;
            var id = GetId(attr.Id);

            if (isDynamic)
            {
                if (DynamicCommands.ContainsKey(id))
                {
                    DynamicCommands[id] = cmd as UDynmaicCommand;
                }
                else
                {
                    DynamicCommands.Add(id, cmd as UDynmaicCommand);
                }
            }
            else
            {
                if (QuickOpenCommands.ContainsKey(id))
                {
                    QuickOpenCommands[id] = cmd as UQuickOpenCommand;
                }
                else
                {
                    QuickOpenCommands.Add(id, cmd as UQuickOpenCommand);
                }
            }
        }
    }

    public class UCommandBase
    {
        /// <summary>
        /// Get the json string of command config
        /// </summary>
        /// <returns></returns>
        public string GetCommandConfigJson()
        {
            var attributes = this.GetType().GetCustomAttributes(typeof(UCommandAttribute), false);

            if (attributes.Length <= 0)
            {
                return string.Empty;
            }

            return (attributes[0] as UCommandAttribute).ToJsonString();
        }
    }

    public abstract class UDynmaicCommand : UCommandBase
    {
        /// <summary>
        /// Action the command will execute
        /// </summary>
        public abstract void Run();
    }

    public abstract class UQuickOpenCommand : UCommandBase
    {
        public abstract IEnumerable<QuickOpenEntryMeta> GetQuickOpenEntries();
        public abstract void RunEntry(string id);
    }

    public class QuickOpenEntryMeta
    {
        public string id { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string icon { get; set; }

        public string ToJsonString()
        {
            return string.Format("{{\"id\":\"{0}\",\"label\":\"{1}\",\"description\":\"{2}\",\"icon\":\"{3}\"}}", id, label, description, icon);
        }
    }
}
