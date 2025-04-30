using BT_COMMONS.Operators;
using System;
using System.Windows.Controls;

namespace BT_BO.Buttons;

public interface IButtonData
{
    Action<MainWindow> OnClick { get; set; }
    OperatorBoolPermission? Permission { get; set; }
    string Name { get; set; }
}