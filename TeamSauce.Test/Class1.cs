using NUnit.Framework;

namespace TeamSauce.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void BeTrue()
        {
            Assert.That(true, Is.True);
        }
    }
}
