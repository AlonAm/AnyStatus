﻿using System;
using System.Windows.Input;

namespace AnyStatus.Core.ContextMenu
{
    internal class DefaultContextMenuItem<TDataContext> : ContextMenu<TDataContext>
    {
        public DefaultContextMenuItem()
        {
            Name = "No actions available";
            Command = new NoActionsAvailableCommand();
        }

        private class NoActionsAvailableCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter) => false;
            public void Execute(object parameter) => throw new NotImplementedException();
        }
    }
}
