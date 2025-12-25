using WhiteLab.PCConfigurator.Core;
using WhiteLab.PCConfigurator.Requirenments;

namespace WhiteLab.PCConfigurator.Tests
{
    public class Tests
    {
        [Test]
        public async Task ContainsResultAsync()
        {
            var req = new Requirements();
            var pc = new Core.PCConfigurator();

            var cnf = await pc.Configure(req);

            Assert.That(cnf, Is.Not.Null);
        }
    }
}
