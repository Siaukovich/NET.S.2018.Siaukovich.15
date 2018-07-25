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
                return new CustomQueue.Queue<int>((int)initialCapacity).Capacity;
            }
            
            return new CustomQueue.Queue<int>().Capacity;
        }

        [TestCase(1, 5, 7, 8, 9, ExpectedResult = 8)]
        [TestCase(1, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 16)]
        [TestCase(new int[0], ExpectedResult = 4)]
        public int Constructor_ValidEnumerable_ValidCapacity(params int[] elements) =>
            new CustomQueue.Queue<int>(elements.AsEnumerable()).Capacity;

        [TestCase(3, 5, 7, 8, 9, ExpectedResult = 6)]
        [TestCase(5, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 10)]
        [TestCase(5, new int[0], ExpectedResult = 5)]
        public int Constructor_ValidEnumerableAndCapacity_ValidCapacity(int capacity, params int[] elements) =>
            new CustomQueue.Queue<int>(elements.AsEnumerable(), capacity).Capacity;

        [TestCase(1, 5, 7, 8, 9, ExpectedResult = 10)]
        [TestCase(1, 5, 7, 8, 9, 1, 5, 7, 8, 9, ExpectedResult = 20)]
        [TestCase(new int[0], ExpectedResult = 0)]
        public int Constructor_ValidCollection_ValidCapacity(params int[] elements) => new CustomQueue.Queue<int>(elements).Capacity;

        [Test]
        public void Constructor_InvalidInitialCapacity_ThrowsArgumentOutOfRangeExc() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => new CustomQueue.Queue<int>(-1));

        [Test]
        public void Constructor_NullEnumerate_ThrowsArgumentNullExc() =>
            Assert.Throws<ArgumentNullException>(() => new CustomQueue.Queue<int>((IEnumerable<int>)null));

        [Test]
        public void Constructor_ValidEnumerableInvalidInitialCapacity_ThrowsArgumentOutOfRangeExc() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => new CustomQueue.Queue<int>(new int[0].AsEnumerable(), -1));

        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullExc() =>
            Assert.Throws<ArgumentNullException>(() => new CustomQueue.Queue<int>(null));

        [Test]
        public void Enumeration_ValidInput_ValidEnumerationOrder()
        {
            var elements = Enumerable.Range(1, 20).ToArray();
            var q = new CustomQueue.Queue<int>(elements);

            var zip = elements.Zip(q, (fromElements, fromQueue) => new { fromElements, fromQueue });
            foreach (var value in zip)
            {
                if (value.fromElements != value.fromQueue)
                {
                    Assert.Fail("Not valid elements");
                }
            }
        }

        [Test]
        public void EnqueueDequeue_SavesOrder()
        {
            var elements = Enumerable.Range(1, 20).ToArray();
            var q = new CustomQueue.Queue<int>();
            foreach (int element in elements)
            {
                q.Enqueue(element);
            }

            foreach (int element in elements)
            {
                if (element != q.Dequeue())
                {
                    Assert.Fail();
                }
            }

            Assert.That(q.IsEmpty());
        }

        [Test]
        public void Clear_EmptyQueue()
        {
            var q = new CustomQueue.Queue<int>(Enumerable.Range(1, 20));
            q.Clear();
            
            Assert.That(q.IsEmpty());
        }

        [Test]
        public void Enqueue_ValidInput_ValidCapacityAndSizeAndEnumerationOrderAfterResizing()
        {
            var elements = Enumerable.Range(1, 20).ToArray();
            var q = new CustomQueue.Queue<int>(elements.Take(5).ToArray());
            foreach (int element in elements.Skip(5))
            {
                q.Enqueue(element);
            }

            var zip = elements.Zip(q, (fromElements, fromQueue) => new { fromElements, fromQueue });
            foreach (var value in zip)
            {
                if (value.fromElements != value.fromQueue)
                {
                    Assert.Fail("Not valid elements");
                }
            }

            if (q.Size != 20)
            {
                Assert.Fail($"Not valid size. Actual = {q.Size}");
            }

            if (q.Capacity != 20)
            {
                Assert.Fail($"Not valid capacity. Actual = {q.Capacity}");
            }
        }

        [Test]
        public void Dequeue_ValidInput_ValidCapacityAndSizeAndEnumerationOrderAfterResizing()
        {
            var elements = Enumerable.Range(1, 11).ToArray();
            var q = new CustomQueue.Queue<int>();
            foreach (int element in elements)
            {
                q.Enqueue(element);
            }

            q.Dequeue();
            q.Dequeue();
            q.Dequeue();

            var zip = elements.Skip(3).Zip(q, (fromElements, fromQueue) => new { fromElements, fromQueue });
            foreach (var value in zip)
            {
                if (value.fromElements != value.fromQueue)
                {
                    Assert.Fail("Not valid elements");
                }
            }

            if (q.Size != 8)
            {
                Assert.Fail($"Not valid size. Actual = {q.Size}");
            }

            if (q.Capacity != 16)
            {
                Assert.Fail($"Not valid capacity. Actual = {q.Capacity}");
            }
        }

        [Test]
        public void Dequeue_ValidInput_ValidCapacityAndSizeAndEnumerationOrderAfterShrinking()
        {
            var elements = Enumerable.Range(1, 11).ToArray();
            var q = new CustomQueue.Queue<int>();
            foreach (int element in elements)
            {
                q.Enqueue(element);
            }

            q.Dequeue();
            q.Dequeue();
            q.Dequeue();

            q.ShrinkToFit();

            var zip = elements.Skip(3).Zip(q, (fromElements, fromQueue) => new { fromElements, fromQueue });
            foreach (var value in zip)
            {
                if (value.fromElements != value.fromQueue)
                {
                    Assert.Fail("Not valid elements");
                }
            }

            if (q.Size != 8)
            {
                Assert.Fail($"Not valid size. Actual = {q.Size}");
            }

            if (q.Capacity != 8)
            {
                Assert.Fail($"Not valid capacity. Actual = {q.Capacity}");
            }
        }

        [Test]
        public void Enumeration_EqueueInForeach_ThrowsInvalidOperationExc()
        {
            var q = new CustomQueue.Queue<int>(Enumerable.Range(1, 11));

            Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        foreach (int _ in q)
                        {
                            q.Enqueue(42);
                        }
                    });
        }

        [Test]
        public void Enumeration_DequeueInForeach_ThrowsInvalidOperationExc()
        {
            var q = new CustomQueue.Queue<int>(Enumerable.Range(1, 11));

            Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        foreach (int _ in q)
                        {
                            q.Dequeue();
                        }
                    });
        }
    }
}
