using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteLab.Telegram;

internal class TelegramStringBuilder
{
    private StringBuilder _str = new();

    public TelegramStringBuilder AddStr(string str)
    {_str.Append(str); return this; }
    public TelegramStringBuilder AddStrWithSpace(string str)
    {_str.Append("").Append(str); return this; }
    public TelegramStringBuilder AddLineStr(string str = "")
    { _str.AppendLine(str); return this; }

    public TelegramStringBuilder AddBoldStrHtml(string str)
    { _str.Append($"<b>{str}</b>"); return this; }
    public TelegramStringBuilder AddBoldStrMurkdown(string str)
    { _str.Append($"**{str}**"); return this; }


    public TelegramStringBuilder AddItalicStrHtml(string str)
    { _str.Append($"<i>{str}</i>"); return this; }
    public TelegramStringBuilder AddItalicStrMurkdown(string str)
    { _str.Append($"*{str}*"); return this; }

    public override string ToString() => _str.ToString();

}
