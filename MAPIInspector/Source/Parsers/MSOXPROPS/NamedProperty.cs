using System;

namespace MAPIInspector.Parsers
{
    public class NamedProperty
    {
        public NamedProperty(string name, ushort id, Guid guid, PropertyDataType type, string set)
        {
            Name = name;
            Id = id;
            Guid = guid;
            Type = type;
            Set = set;
        }

        public string Name { get; }
        public ushort Id { get; }
        public Guid Guid { get; }
        public PropertyDataType Type { get; }
        public string Set { get; }

        public static NamedProperty Lookup(Guid guid, uint id)
        {
            // Loop over NamedProperties looking for a match and return it
            foreach (var namedProperty in NamedProperties.Properties)
            {
                if (namedProperty.Guid == guid && namedProperty.Id == id)
                {
                    return namedProperty;
                }
            }

            return null;
        }
    }
}
