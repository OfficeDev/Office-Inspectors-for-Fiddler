using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WOPIautomation
{
    public class User32API
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private static bool isShift = false;

        public static byte GetKeys(char c)
        {
            string temp = c.ToString();

            if (c >= 'a' && c <= 'z')
            {
                return (byte)(Keys)Enum.Parse(typeof(Keys), System.Text.Encoding.ASCII.GetBytes(temp.ToUpper())[0].ToString());
            }

            if (c >= 'A' && c <= 'Z')
            {
                isShift = true;
                return (byte)(Keys)Enum.Parse(typeof(Keys), System.Text.Encoding.ASCII.GetBytes(temp)[0].ToString());
            }

            switch(temp)
            {
                case "0":
                    return (byte)Keys.NumPad0;
                case "1":
                    return (byte)Keys.NumPad1;
                case "2":
                    return (byte)Keys.NumPad2;
                case "3":
                    return (byte)Keys.NumPad3;
                case "4":
                    return (byte)Keys.NumPad4;
                case "5":
                    return (byte)Keys.NumPad5;
                case "6":
                    return (byte)Keys.NumPad6;
                case "7":
                    return (byte)Keys.NumPad7;
                case "8":
                    return (byte)Keys.NumPad8;
                case "9":
                    return (byte)Keys.NumPad9;
                case "|":
                    isShift = true;
                    return 0xDC;
                case "\\":                    
                    return 0xDC;
                case "{":
                    isShift = true;
                    return 0xDB;
                case "[":
                    return 0xDB;
                case "}":
                    isShift = true;
                    return 0xDD;
                case "]":
                    return 0xDD;
                case ";":
                    return 0xBA;
                case ":":
                    isShift = true;
                    return 0xBA;
                case "'":
                    return 0xDE;
                case "\"":
                    isShift = true;
                    return 0XDE;
                case ",":
                    return 0xBC;
                case "<":
                    isShift = true;
                    return 0xBC;
                case ".":
                    return (byte)Keys.Decimal;
                case ">":
                    // TODO
                case "/":
                    return (byte)Keys.Divide;
                case "?":
                    isShift = true;
                    return 0xBF;
                case "*":
                    return (byte)Keys.Multiply;
                case "!":
                    isShift = true;
                    return (byte)Keys.D1;
                case "@":
                    isShift = true;
                    return (byte)Keys.D2;
                case "#":
                    isShift = true;
                    return (byte)Keys.D3;
                case "$":
                    isShift = true;
                    return (byte)Keys.D4;
                case "%":
                    isShift = true;
                    return (byte)Keys.D5;
                case "^":
                    isShift = true;
                    return (byte)Keys.D6;
                case "&":
                    isShift = true;
                    return (byte)Keys.D7;
                case "(":
                    isShift = true;
                    return (byte)Keys.D9;
                case ")":
                    isShift = true;
                    return (byte)Keys.D0;
                case "-":
                    return (byte)Keys.Subtract;
                case "_":
                    isShift = true;
                    return 0xBD;
                case "=":
                    return 0xBB;
                case "+":
                    return (byte)Keys.Add;
                case "`":
                    return 0xC0;
                case "~":
                    isShift = true;
                    return 0xC0;
                case " ":
                    return (byte)Keys.Space;
            }

            return 0;
        }

        public static void KeybdInput(string str)
        {
            foreach (char c in str)
            {
                byte key = GetKeys(c);

                if (key == 0)
                {
                    throw new Exception("un-organized input");
                }

                if (isShift)
                {
                    keybd_event((byte)System.Windows.Forms.Keys.ShiftKey, 0, 0, 0);
                    keybd_event(key, 0, 0, 0);
                    keybd_event((byte)System.Windows.Forms.Keys.ShiftKey, 0, 2, 0);
                    keybd_event(key, 0, 2, 0);

                    isShift = false;
                }
                else
                {
                    keybd_event(key, 0, 0, 0);
                    keybd_event(key, 0, 2, 0);
                }
            }            
        }
    }
}
