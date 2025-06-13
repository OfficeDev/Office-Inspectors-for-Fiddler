namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;

    /// <summary>
    /// The folderMessages element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessages : SyntacticalBase
    {
        /// <summary>
        /// A list of MetaTagFxDelPropMessageList.
        /// </summary>
        public MetaTagFxDelPropMessageList[] MetaTagFxDelPropMessageLists;

        /// <summary>
        /// Initializes a new instance of the FolderMessages class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderMessages(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderMessages
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderMessages, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && MetaTagFxDelPropMessageList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            int count = 0;
            List<MetaTagFxDelPropMessageList> interMessageLists = new List<MetaTagFxDelPropMessageList>();

            while (!stream.IsEndOfStream && count < 2)
            {
                if (MetaTagFxDelPropMessageList.Verify(stream))
                {
                    interMessageLists.Add(new MetaTagFxDelPropMessageList(stream));
                }
                else
                {
                    break;
                }

                count++;
            }

            this.MetaTagFxDelPropMessageLists = interMessageLists.ToArray();
        }
    }
}
