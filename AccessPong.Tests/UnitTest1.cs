using AccessPong.Events;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // Arrange
            int num = 1;
            int expected = 2;

            var tempClass = new TempClass();

            // Act
            int actual = tempClass.AddOne(num);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}