using System;
using System.Windows.Input;

namespace SongTheoryApplication.ViewModels.Base;

public class RelayCommand : ICommand
    {
        private readonly Action _methodToBeExecuted;
        private readonly Func<bool>? _canBeMethodToBeExecuted;
        
        public event EventHandler? CanExecuteChanged = delegate { };

        /// <summary>
        /// This constructor creates command that will be surely executed 
        /// </summary>
        /// <param name="methodToBeExecuted">Method that will be executed</param>
        public RelayCommand(Action methodToBeExecuted)
        {
            _methodToBeExecuted = methodToBeExecuted;
        }

        
        /// <summary>
        /// Creates command with <see cref="_methodToBeExecuted" /> that will be executed if <see cref="_canBeMethodToBeExecuted"/> will be true/>
        /// </summary>
        /// <param name="methodToBeExecuted">Method that will be executed</param>
        /// <param name="canBeMethodToBeExecuted">Boolean parameter that determines whether the method will be executed or not</param>
        public RelayCommand(Action methodToBeExecuted, Func<bool> canBeMethodToBeExecuted)
        {
            _methodToBeExecuted = methodToBeExecuted;
            _canBeMethodToBeExecuted = canBeMethodToBeExecuted;
        }

        
        /// <summary>
        /// Fires everytime that '<see cref="_canBeMethodToBeExecuted"/>' changes...
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines if command can be executed
        /// </summary>
        public bool CanExecute(object? parameter)
        {
            return _canBeMethodToBeExecuted == null || _canBeMethodToBeExecuted();
        }

        
        /// <summary>
        /// Executes the command if command is not null
        /// </summary>
        public void Execute(object? parameter)
        {
            _methodToBeExecuted.Invoke();
        }
    }