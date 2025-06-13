namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains a list of propValues.
    /// </summary>
    public class PropList : SyntacticalBase
    {
        /// <summary>
        /// A list of PropValue objects.
        /// </summary>
        public PropValue[] PropValues;

        /// <summary>
        /// Initializes a new instance of the PropList class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropList.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized PropList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropValue.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<PropValue> propValuesList = new List<PropValue>();

            while (PropValue.Verify(stream))
            {
                propValuesList.Add(PropValue.ParseFrom(stream) as PropValue);
            }

            this.PropValues = propValuesList.ToArray();
        }
    }
}
