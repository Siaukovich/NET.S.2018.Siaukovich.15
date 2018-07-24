namespace Queue.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    /// <summary>
    /// Class for testing Queue class.
    /// </summary>
    [TestFixture]
    public class QueueTests
    {
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(5, ExpectedResult = 5)]
        [TestCase(null, ExpectedResult = 4)]
        public int Constructor_ValidInitialCapacity_ValidCapacity(int? initialCapacity)
        {
            if (initialCapacity != null)
            {
                return new EpamDay15.Queue<int>((int)initialCapacity).Capacity;
            }
            
            return new EpamDay15.Queue<int>().Capacity;
        }

        [TestCase(1, 5, 7, 8, 9, ExpectedResult = 8)]
        [TestCase(1, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 16)]
        [TestCase(new int[0], ExpectedResult = 4)]
        public int Constructor_ValidEnumerable_ValidCapacity(params int[] elements) =>
            new EpamDay15.Queue<int>(elements.AsEnumerable()).Capacity;

        [TestCase(3, 5, 7, 8, 9, ExpectedResult = 6)]
        [TestCase(5, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 10)]
        [TestCase(5, new int[0], ExpectedResult = 5)]
        public int Constructor_ValidEnumerableAndCapacity_ValidCapacity(int capacity, params int[] elements) =>
            new EpamDay15.Queue<int>(elements.AsEnumerable(), capacity).Capacity;

        [TestCase(1, 5, 7, 8, 9, ExpectedResult = 10)]
        [TestCase(1, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 20)]
        [TestCase(new int[0], ExpectedResult = 0)]
        public int Constructor_ValidCollection_ValidCapacity(params int[] elements) => new EpamDay15.Queue<int>(elements).Capacity;

        [Test]
        public void Constructor_InvalidInitialCapacity_ThrowsArgumentOutOfRangeExc() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => new EpamDay15.Queue<int>(-1));

        [Test]
        public void Constructor_NullEnumerate_ThrowsArgumentNullExc() =>
            Assert.Throws<ArgumentNullException>(() => new EpamDay15.Queue<int>((IEnumerable<int>)null));

        [Test]
        public void Constructor_ValidEnumerableInvalidInitialCapacity_ThrowsArgumentOutOfRangeExc() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => new EpamDay15.Queue<int>(new int[0].AsEnumerable(), -1));

        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullExc() =>
            Assert.Throws<ArgumentNullException>(() => new EpamDay15.Queue<int>(null));
    }
}
