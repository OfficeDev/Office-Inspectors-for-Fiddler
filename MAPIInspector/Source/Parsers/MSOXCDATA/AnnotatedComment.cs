namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The AnnotatedComment class is a comment node that takes up no space in the stream. It is used to annotate other nodes with comments.
    /// </summary>
    public class AnnotatedComment : AnnotatedData
    {
        /// <summary>
        /// Initializes a new instance of the Information class with parameters.
        /// </summary>
        /// <param name="comment">The comment</param>
        public AnnotatedComment(string comment) => ParsedValue = comment;
        public static implicit operator AnnotatedComment(string comment) => new AnnotatedComment(comment);
        public override int Size { get { return 0; } }
    }
}