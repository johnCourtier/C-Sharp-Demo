using Demo.Document.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Document.Repository
{
    [TestFixture]
    internal class CombinedRepositoryTest
    {
        private Mock<IDocumentRepository> PrimaryRepository;

        private Mock<IDocumentRepository> SecondaryRepository;

        private Mock<ILogger<CombinedRepository>> Logger;

        private CombinedRepository CombinedRepository;

        [SetUp]
        public void Setup()
        {
            this.PrimaryRepository = new Mock<IDocumentRepository>();
            this.SecondaryRepository = new Mock<IDocumentRepository>();
            this.Logger = new Mock<ILogger<CombinedRepository>>();
            this.CombinedRepository = new CombinedRepository(
                this.Logger.Object,
                this.PrimaryRepository.Object,
                this.SecondaryRepository.Object
            );
        }
    }
}