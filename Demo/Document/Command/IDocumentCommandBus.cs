namespace Demo.Document.Command
{
    public interface IDocumentCommandBus
    {
        /// <exception cref="RuntimeException">Thrown when execution fails</exception>
        public DocumentEntity Execute(GetDocumentCommand getDocumentCommand);

        /// <exception cref="RuntimeException">Thrown when execution fails</exception>
        public void Execute(CreateDocumentCommand persistDocumentCommand);

        /// <exception cref="RuntimeException">Thrown when execution fails</exception>
        public void Execute(UpdateDocumentCommand updateDocumentCommand);
    }
}