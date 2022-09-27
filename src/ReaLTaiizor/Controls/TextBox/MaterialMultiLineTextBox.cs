﻿#region Imports

using ReaLTaiizor.Util;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static ReaLTaiizor.Helper.MaterialDrawHelper;

#endregion

namespace ReaLTaiizor.Controls
{
    #region MaterialMultiLineTextBox

    public class MaterialMultiLineTextBox : RichTextBox, MaterialControlI
    {
        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialManager SkinManager => MaterialManager.Instance;

        [Browsable(false)]
        public MaterialMouseState MouseState { get; set; }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        private const char EmptyChar = (char)0;

        private const char VisualStylePasswordChar = '\u25CF';

        private const char NonVisualStylePasswordChar = '\u002A';

        private string hint = string.Empty;

        [Category("Material"), DefaultValue(""), Localizable(true)]
        public string Hint
        {
            get => hint;
            set
            {
                hint = value;
                SendMessage(Handle, EM_SETCUEBANNER, (int)IntPtr.Zero, Hint);
            }
        }

        private bool _leaveOnEnterKey;

        [Category("Material"), DefaultValue(false), Description("Select next control which have TabStop property set to True when enter key is pressed. To add enter in text, the user must press CTRL+Enter")]
        public bool LeaveOnEnterKey
        {
            get => _leaveOnEnterKey;
            set
            {
                _leaveOnEnterKey = value;
                if (value)
                {
                    KeyDown += new KeyEventHandler(LeaveOnEnterKey_KeyDown);
                }
                else
                {
                    KeyDown -= LeaveOnEnterKey_KeyDown;
                }
                Invalidate();
            }
        }

        public new void SelectAll()
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                base.Focus();
                base.SelectAll();
            });
        }

        public new void Focus()
        {
            BeginInvoke((MethodInvoker)delegate ()
            {
                base.Focus();
            });
        }

        private void LeaveOnEnterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && e.Control == false)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendKeys.Send("{TAB}");
            }
        }

        public MaterialMultiLineTextBox()
        {
            base.OnCreateControl();
            this.Multiline = true;

            BorderStyle = BorderStyle.None;
            Font = SkinManager.GetFontByType(MaterialManager.FontType.Body1);
            BackColor = SkinManager.BackgroundColor;
            ForeColor = SkinManager.TextHighEmphasisColor;
            BackColorChanged += (sender, args) => BackColor = SkinManager.BackgroundColor;
            ForeColorChanged += (sender, args) => ForeColor = SkinManager.TextHighEmphasisColor;
        }
    }

    #endregion
}