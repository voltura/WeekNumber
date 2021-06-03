#region Using statements

using System;
using System.Windows.Forms;

#endregion Using statements

namespace WeekNumber
{
    internal static class Message
    {
        #region Caption

        internal static readonly string CAPTION = $"{Application.ProductName} {Resources.Version} {Application.ProductVersion}";

        #endregion Caption

        #region Show Information or Error dialog methods

        internal static void Show(string text, Exception ex = null)
        {
            var message = ex is null ? text : $"{text}\r\n{ex}";
            if (ex is null) Log.Info = message; else Log.ErrorString = message;
            Forms.MessageForm.DisplayMessage(message, !(ex is null));
        }

        internal static void Show(string text)
        {
            Show(text, null);
        }

        internal static void Show(string message, bool isError)
        {
            if (isError) Log.Info = message; else Log.ErrorString = message;
            Forms.MessageForm.DisplayMessage(message, isError);
        }

        internal static bool UserAcceptedQuestion(string message)
        {
            Log.Info = message;
            // TODO: Customize and use MessageForm
            DialogResult userAnswer = MessageBox.Show(message, CAPTION, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            return userAnswer == DialogResult.Yes;
        }

        #endregion Show Information or Error dialog methods
    }
}
