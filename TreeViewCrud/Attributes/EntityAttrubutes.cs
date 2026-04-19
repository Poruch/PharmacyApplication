using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewCrud.Attributes
{
    // Атрибут для указания имени таблицы в БД
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; }
        public TableAttribute(string name) => Name = name;
    }

    // Атрибут для указания, что свойство — первичный ключ (обычно Id, но можно переопределить)
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute { }

    // Атрибут для явного указания имени столбца (если отличается от имени свойства)
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; }
        public ColumnAttribute(string name) => Name = name;
    }
}
