using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.Converter
{
    public interface IValueConverter<T>
    {
        T Convert(object value, params object[] parameters);
    }
}