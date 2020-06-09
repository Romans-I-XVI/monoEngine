using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine
{
    public class VirtualButton
    {
        private List<KeyValuePair<Buttons, PlayerIndex?>> _buttons = new List<KeyValuePair<Buttons, PlayerIndex?>>();
        private List<Keys> _keys = new List<Keys>();
        private List<MouseButtons> _mouse_buttons = new List<MouseButtons>();
        public InputLayer InputLayer = InputLayer.One;

        public VirtualButton() {}

        private void AddButton(Buttons button, PlayerIndex? playerIndex)
        {
            bool exists = false;
            foreach (var pair in _buttons)
            {
                if (pair.Key == button && pair.Value == playerIndex)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                var item = new KeyValuePair<Buttons, PlayerIndex?>(button, playerIndex);
                _buttons.Add(item);
            }
        }

        public void AddButton(Buttons button, PlayerIndex playerIndex)
        {
            AddButton(button, (PlayerIndex?)playerIndex);
        }

        public void AddButton(Buttons button)
        {
            AddButton(button, null);
        }

        public void AddKey(Keys key)
        {
            if (!_keys.Contains(key))
                _keys.Add(key);
        }

        public void AddMouseButton(MouseButtons mouseButton)
        {
            if (!_mouse_buttons.Contains(mouseButton))
                _mouse_buttons.Add(mouseButton);
        }

        public void RemoveButton(Buttons button)
        {
            for (int i = _buttons.Count - 1; i >= 0; i--)
            {
                if (_buttons[i].Key == button && _buttons[i].Value == null)
                    _buttons.RemoveAt(i);
            }
        }

        public void RemoveButton(Buttons button, PlayerIndex playerIndex)
        {
            for (int i = _buttons.Count - 1; i >= 0; i--)
            {
                if (_buttons[i].Key == button && _buttons[i].Value == playerIndex)
                    _buttons.RemoveAt(i);
            }
        }

        public void RemoveKey(Keys key)
        {
            for (int i = _keys.Count - 1; i >= 0; i--)
            {
                if (_keys[i] == key)
                    _keys.RemoveAt(i);
            }
        }

        public void RemoveMouseButton(MouseButtons mouseButton)
        {
            for (int i = _mouse_buttons.Count - 1; i >= 0; i--)
            {
                if (_mouse_buttons[i] == mouseButton)
                    _mouse_buttons.RemoveAt(i);
            }
        }

        public bool IsPressed()
        {
            if ((this.InputLayer & Engine.InputLayer) == 0)
                return false;

            foreach (var key in _keys)
            {
                if (Input.Keyboard.isPressed(key))
                {
                    return true;
                }
            }

            foreach (var mouseButton in _mouse_buttons)
            {
                if (Input.Mouse.isPressed(mouseButton))
                {
                    return true;
                }
            }

            foreach (var button in _buttons)
            {
                if (button.Value == null)
                {
                    if (Input.Gamepad.isPressed(button.Key))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Input.Gamepad.isPressed(button.Key, (PlayerIndex)button.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsReleased()
        {
            if ((this.InputLayer & Engine.InputLayer) == 0)
                return false;

            foreach (var key in _keys)
            {
                if (Input.Keyboard.isReleased(key))
                {
                    return true;
                }
            }

            foreach (var mouseButton in _mouse_buttons)
            {
                if (Input.Mouse.isReleased(mouseButton))
                {
                    return true;
                }
            }

            foreach (var button in _buttons)
            {
                if (button.Value == null)
                {
                    if (Input.Gamepad.isReleased(button.Key))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Input.Gamepad.isReleased(button.Key, (PlayerIndex)button.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsHeld()
        {
            if ((this.InputLayer & Engine.InputLayer) == 0)
                return false;

            foreach (var key in _keys)
            {
                if (Input.Keyboard.isHeld(key))
                {
                    return true;
                }
            }

            foreach (var mouseButton in _mouse_buttons)
            {
                if (Input.Mouse.isHeld(mouseButton))
                {
                    return true;
                }
            }

            foreach (var button in _buttons)
            {
                if (button.Value == null)
                {
                    if (Input.Gamepad.isHeld(button.Key))
                    {
                        return true;
                    }
                }
                else
                {
                    if (Input.Gamepad.isHeld(button.Key, (PlayerIndex)button.Value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
