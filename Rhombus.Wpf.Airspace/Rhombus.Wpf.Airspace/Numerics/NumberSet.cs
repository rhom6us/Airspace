﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rhombus.Wpf.Airspace.Numerics {
    /// <summary>
    ///     This is just an ordered set of numbers.  The .Net Framework will
    ///     probably provide better solutions soon.  SortedSet in 4.0?
    /// </summary>
    public class NumberSet<T> : IEnumerable<T> {
        public Interval<T> Interval { get; private set; }

        public int Count => _values.Count;

        public T this[int index] {
            get {
                if (index < 0 || index >= _values.Count)
                    throw new IndexOutOfRangeException();

                return _values[index];
            }
        }

        public bool TryFind(T value, out int index) {
            dynamic nValue = value;

            for (var i = 0; i < _values.Count; i++) {
                if (nValue == _values[i]) {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public bool Add(T value) {
            dynamic nValue = value;

            // If this is the first number to be added to the set then add it
            // to the front.
            if (_values.Count == 0) {
                _values.Add(value);
                this.Interval = new Interval<T>(value, true, value, true);
                return true;
            }

            // Do a simple linear search for the first number currently in the
            // set that is greater than the number being added.  This is where
            // we need to insert the new number.  Of course, since this is a
            // set, any duplicates are discarded so we check for that along
            // the way too.
            for (var i = 0; i < _values.Count; i++) {
                if (nValue == _values[i]) {
                    // Discard duplicates.
                    return false;
                }

                if (nValue < _values[i]) {
                    // We found the first place where:
                    // _values[i-1] < value < _values[i]
                    //
                    // This is where we need to insert. 
                    _values.Insert(i, value);

                    if (i == 0)
                        this.Interval = new Interval<T>(_values[0], true, _values[_values.Count - 1], true);

                    return true;
                }
            }

            // If we get here, the number being added must be greater than the
            // entire set.  Add it to the end.
            _values.Add(value);
            this.Interval = new Interval<T>(_values[0], true, _values[_values.Count - 1], true);
            return true;
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (var value in _values) {
                yield return value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        private readonly List<T> _values = new List<T>();
    }
}