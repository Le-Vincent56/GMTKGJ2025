using System;
using System.Collections.Generic;

namespace Perennial.Core.Architecture.Optionals
{
    public readonly struct Optional<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        /// <summary>
        /// Creates a new Optional with a value.
        /// </summary>
        /// <param name="value">The value to store.</param>
        public Optional(T value)
        {
            this.value = value;
            hasValue = true;
        }
        
        /// <summary>
        /// A static instance representing the absence of a value.
        /// </summary>
        public static readonly Optional<T> noValue =  new Optional<T>();

        /// <summary>
        /// Gets the stored value if present, otherwise throws an InvalidOperationException.
        /// </summary>
        public T Value => hasValue ? value : throw new InvalidOperationException("No value");
        
        /// <summary>
        /// Indicates whether the Optional contains a value.
        /// </summary>
        public bool HasValue => hasValue;

        /// <summary>
        /// Returns the stored value if present; otherwise, returns the default value of type T.
        /// </summary>
        public T GetValueOrDefault() => value;
        
        /// <summary>
        /// Returns the stored value if present; otherwise, returns the specified default value.
        /// </summary>
        public T GetValueOrDefault(T defaultValue) => hasValue ? value : defaultValue;

        /// <summary>
        /// Pattern-matches on the Optional: applies <paramref name="onValue"/> if value is present,
        /// otherwise applies <paramref name="onNoValue"/>.
        /// </summary>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onNoValue)
        {
            return hasValue ? onValue(value) : onNoValue();
        }

        /// <summary>
        /// Binds a function over the optional value, flattening the result.
        /// Equivalent to monadic bind.
        /// </summary>
        public Optional<TResult> SelectMany<TResult>(Func<T, Optional<TResult>> bind)
        {
            return hasValue ? bind(value) : Optional<TResult>.noValue;
        }

        /// <summary>
        /// Maps a function over the optional value, producing a new Optional.
        /// </summary>
        public Optional<TResult> Select<TResult>(Func<T, TResult> map)
        {
            return hasValue ? new Optional<TResult>(map(value)) : Optional<TResult>.noValue;
        }

        /// <summary>
        /// Combines two Optional values using the provided combiner function,
        /// only if both are present.
        /// </summary>
        public static Optional<TResult> Combine<T1, T2, TResult>(Optional<T1> first, Optional<T2> second,
            Func<T1, T2, TResult> combiner)
        {
            if (first.HasValue && second.HasValue)
            {
                return new Optional<TResult>(combiner(first.Value, second.Value));
            }
            
            return new Optional<TResult>();
        }
        
        public static Optional<T> Some(T value) => new Optional<T>(value);
        public static Optional<T> None() => noValue;
        
        public override bool Equals(object obj) => obj is Optional<T> other && Equals(other);
        private bool Equals(Optional<T> other) => !hasValue ? !other.hasValue : EqualityComparer<T>.Default.Equals(value, other.value);

        public override int GetHashCode() => (hasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(value);

        public override string ToString() => hasValue ? $"Some({value})" : "None";

        public static implicit operator Optional<T>(T value) => new Optional<T>(value);

        public static implicit operator bool(Optional<T> value) => value.hasValue;

        public static explicit operator T(Optional<T> value) => value.Value;
    }
}
