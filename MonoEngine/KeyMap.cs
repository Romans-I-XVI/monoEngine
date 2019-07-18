// Keymap from https://gist.github.com/koenbollen/942161

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
    public static class KeyMap
    {
        public enum Modifier : int
        {
            None,
            Shift,
        }

        private static Dictionary<Keys, Dictionary<Modifier, char>> _map = null;
        public static Dictionary<Keys, Dictionary<Modifier, char>> Map
        {
            get
            {
                if (_map == null)
                {
                    _map = new Dictionary<Keys, Dictionary<Modifier, char>>();
                    _map[Keys.Space] = new Dictionary<Modifier, char>();
                    _map[Keys.Space][Modifier.None] = ' ';

                    char[] specials = {')', '!', '@', '#', '$', '%', '^', '&', '*', '('};

                    for (int i = 0; i <= 9; i++)
                    {
                        char c = (char)(i + 48);
                        _map[(Keys)c] = new Dictionary<Modifier, char>();
                        _map[(Keys)c][Modifier.None] = c;
                        _map[(Keys)c][Modifier.Shift] = specials[i];
                    }

                    for (char c = 'A'; c <= 'Z'; c++)
                    {
                        _map[(Keys)c] = new Dictionary<Modifier, char>();
                        _map[(Keys)c][Modifier.None] = (char)(c + 32);
                        _map[(Keys)c][Modifier.Shift] = c;
                    }

                    _map[Keys.OemPipe] = new Dictionary<Modifier, char>();
                    _map[Keys.OemPipe][Modifier.None] = '\\';
                    _map[Keys.OemPipe][Modifier.Shift] = '|';

                    _map[Keys.OemOpenBrackets] = new Dictionary<Modifier, char>();
                    _map[Keys.OemOpenBrackets][Modifier.None] = '[';
                    _map[Keys.OemOpenBrackets][Modifier.Shift] = '{';

                    _map[Keys.OemCloseBrackets] = new Dictionary<Modifier, char>();
                    _map[Keys.OemCloseBrackets][Modifier.None] = ']';
                    _map[Keys.OemCloseBrackets][Modifier.Shift] = '}';

                    _map[Keys.OemComma] = new Dictionary<Modifier, char>();
                    _map[Keys.OemComma][Modifier.None] = ',';
                    _map[Keys.OemComma][Modifier.Shift] = '<';

                    _map[Keys.OemPeriod] = new Dictionary<Modifier, char>();
                    _map[Keys.OemPeriod][Modifier.None] = '.';
                    _map[Keys.OemPeriod][Modifier.Shift] = '>';

                    _map[Keys.OemSemicolon] = new Dictionary<Modifier, char>();
                    _map[Keys.OemSemicolon][Modifier.None] = ';';
                    _map[Keys.OemSemicolon][Modifier.Shift] = ':';

                    _map[Keys.OemQuestion] = new Dictionary<Modifier, char>();
                    _map[Keys.OemQuestion][Modifier.None] = '/';
                    _map[Keys.OemQuestion][Modifier.Shift] = '?';

                    _map[Keys.OemQuotes] = new Dictionary<Modifier, char>();
                    _map[Keys.OemQuotes][Modifier.None] = '\'';
                    _map[Keys.OemQuotes][Modifier.Shift] = '"';

                    _map[Keys.OemMinus] = new Dictionary<Modifier, char>();
                    _map[Keys.OemMinus][Modifier.None] = '-';
                    _map[Keys.OemMinus][Modifier.Shift] = '_';

                    _map[Keys.OemPlus] = new Dictionary<Modifier, char>();
                    _map[Keys.OemPlus][Modifier.None] = '=';
                    _map[Keys.OemPlus][Modifier.Shift] = '+';
                }

                return _map;
            }
        }

        public static char? GetChar(Keys key, Modifier mod)
        {
            if (!Map.ContainsKey(key))
                return null;
            if (!Map[key].ContainsKey(mod))
                return null;
            return Map[key][mod];
        }

        public static List<char> ListChars()
        {
            List<char> chars = new List<char>();

            foreach (Keys key in Map.Keys)
            foreach (Modifier mod in Map[key].Keys)
                if (!chars.Contains(Map[key][mod]))
                    chars.Add(Map[key][mod]);

            return chars;
        }
    }
}
