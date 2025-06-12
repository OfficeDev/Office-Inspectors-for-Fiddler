namespace MAPIInspector.Parsers
{
    using System;
    using System.Text;

    /// <summary>
    /// The GroupPropertyName.
    /// 2.2.2.8.1.1 GroupPropertyName
    /// </summary>
    public class GroupPropertyName : BaseStructure
    {
        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// A value that identifies the type of property. 
        /// </summary>
        public uint Kind;

        /// <summary>
        ///  A value that identifies the named property within its property set. 
        /// </summary>
        public uint? Lid;

        /// <summary>
        /// A value that specifies the length of the Name field, in bytes. 
        /// </summary>
        public uint? NameSize;

        /// <summary>
        /// A Unicode (UTF-16) string that identifies the property within the property set. 
        /// </summary>
        public MAPIString Name;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains GroupPropertyName.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Guid = stream.ReadGuid();
            this.Kind = stream.ReadUInt32();

            if (this.Kind == 0x00000000)
            {
                this.Lid = stream.ReadUInt32();
            }
            else if (this.Kind == 0x00000001)
            {
                this.NameSize = stream.ReadUInt32();
                this.Name = new MAPIString(Encoding.Unicode, string.Empty, (int)this.NameSize / 2);
                this.Name.Parse(stream);
            }
        }
    }
}
