using System;

namespace Perennial.Core.Architecture.State_Machine
{
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        /// <summary>
        /// Evaluate the predicate
        /// </summary>
        public bool Evaluate() => _func.Invoke();
    }
}