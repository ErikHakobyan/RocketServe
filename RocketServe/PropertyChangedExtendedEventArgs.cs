using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RocketServe
{
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public virtual object OldValue { get; private set; }
        public virtual object NewValue { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, object oldValue,
               object newValue)
               : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

}
