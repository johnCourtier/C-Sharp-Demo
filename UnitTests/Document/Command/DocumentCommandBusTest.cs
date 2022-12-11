using Demo.Document;
using Demo.Document.Command;
using Moq;
using Newtonsoft.Json.Linq;

namespace UnitTests.Document.Command
{
    [TestFixture]
    public class DocumentCommandBusTest
    {
        private DocumentCommandBus DocumentCommandBus;

        private Mock<IDocumentRepository> DocumentRepository;

        [SetUp]
        public void Setup()
        {
            this.DocumentRepository = new Mock<IDocumentRepository>();
            this.DocumentCommandBus = new DocumentCommandBus(this.DocumentRepository.Object);
        }

        [Test]
        public void Execute_readingFailure_RuntimeExceptionThrown()
        {
            GetDocumentCommand getDocumentCommand = new GetDocumentCommand()
            {
                Criteria = new Criteria()
                {
                    Id = "123"
                }
            };
            RuntimeException repositoryException = new RuntimeException("Repository exception");
            this.DocumentRepository.Setup(me => me.Read(getDocumentCommand.Criteria)).Throws(repositoryException);

            try
            {
                this.DocumentCommandBus.Execute(getDocumentCommand);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to execute '{getDocumentCommand}' command. Reading from '{this.DocumentRepository.Object}' repository has failed."));
                Assert.That(exception.InnerException!, Is.EqualTo(repositoryException));
                this.DocumentRepository.Verify(me => me.Read(getDocumentCommand.Criteria), Times.Once());
            }
        }

        [Test]
        public void Execute_readingSuccess()
        {
            GetDocumentCommand getDocumentCommand = new GetDocumentCommand()
            {
                Criteria = new Criteria()
                {
                    Id = "123"
                }
            };
            DocumentEntity document = new DocumentEntity
            {
                Id = "123",
                Tags = new string[] { "tag1", "tag2" },
                Data = new JArray()
            };
            this.DocumentRepository.Setup(me => me.Read(getDocumentCommand.Criteria)).Returns(document);

            DocumentEntity actualDocument = this.DocumentCommandBus.Execute(getDocumentCommand);
            Assert.That(actualDocument, Is.EqualTo(document));
            this.DocumentRepository.Verify(me => me.Read(getDocumentCommand.Criteria), Times.Once());
        }

        [Test]
        public void Execute_creatingFailure_RuntimeExceptionThrown()
        {
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand()
            {
                Document = new DocumentEntity
                {
                    Id = "123",
                    Tags = new string[] { "tag1", "tag2" },
                    Data = new JArray()
                }
            };
            RuntimeException repositoryException = new RuntimeException("Repository exception");
            this.DocumentRepository.Setup(me => me.Create(createDocumentCommand.Document)).Throws(repositoryException);

            try
            {
                this.DocumentCommandBus.Execute(createDocumentCommand);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to execute '{createDocumentCommand}' command. Creating by '{this.DocumentRepository.Object}' repository has failed."));
                Assert.That(exception.InnerException!, Is.EqualTo(repositoryException));
                this.DocumentRepository.Verify(me => me.Create(createDocumentCommand.Document), Times.Once());
            }
        }

        [Test]
        public void Execute_creatingSuccess()
        {
            CreateDocumentCommand createDocumentCommand = new CreateDocumentCommand()
            {
                Document = new DocumentEntity
                {
                    Id = "123",
                    Tags = new string[] { "tag1", "tag2" },
                    Data = new JArray()
                }
            };
            this.DocumentRepository.Setup(me => me.Create(createDocumentCommand.Document));

            this.DocumentCommandBus.Execute(createDocumentCommand);
            this.DocumentRepository.Verify(me => me.Create(createDocumentCommand.Document), Times.Once());
        }

        [Test]
        public void Execute_updatingFailure_RuntimeExceptionThrown()
        {
            UpdateDocumentCommand updateDocumentCommand = new UpdateDocumentCommand()
            {
                Document = new DocumentEntity
                {
                    Id = "123",
                    Tags = new string[] { "tag1", "tag2" },
                    Data = new JArray()
                }
            };
            RuntimeException repositoryException = new RuntimeException("Repository exception");
            this.DocumentRepository.Setup(me => me.Update(updateDocumentCommand.Document)).Throws(repositoryException);

            try
            {
                this.DocumentCommandBus.Execute(updateDocumentCommand);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (RuntimeException exception)
            {
                Assert.That(exception.Message, Is.EqualTo($"Unable to execute '{updateDocumentCommand}' command. Updating by '{this.DocumentRepository.Object}' repository has failed."));
                Assert.That(exception.InnerException!, Is.EqualTo(repositoryException));
                this.DocumentRepository.Verify(me => me.Update(updateDocumentCommand.Document), Times.Once());
            }
        }

        [Test]
        public void Execute_updatingSuccess()
        {
            UpdateDocumentCommand updateDocumentCommand = new UpdateDocumentCommand()
            {
                Document = new DocumentEntity
                {
                    Id = "123",
                    Tags = new string[] { "tag1", "tag2" },
                    Data = new JArray()
                }
            };
            this.DocumentRepository.Setup(me => me.Update(updateDocumentCommand.Document));

            this.DocumentCommandBus.Execute(updateDocumentCommand);
            this.DocumentRepository.Verify(me => me.Update(updateDocumentCommand.Document), Times.Once());
        }
    }
}