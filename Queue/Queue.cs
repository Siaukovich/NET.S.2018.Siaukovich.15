namespace EpamDay15
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Class that implements Queue DS.
    /// </summary>
    /// <typeparam name="T">
    /// Type of queue elements.
    /// </typeparam>
    public class Queue<T> : IEnumerable<T>
    {
        #region Private Fields

        /// <summary>
        /// This queue's elements.
        /// </summary>
        private T[] elements;

        /// <summary>
        /// Index of queue's head.
        /// </summary>
        private int head;

        /// <summary>
        /// Index of queue's tail.
        /// </summary>
        private int tail;

        /// <summary>
        /// Version of this queue.
        /// </summary>
        private int version;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">
        /// Queue's initial capacity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if initialCapacity &lt;= 0.
        /// </exception>
        public Queue(int initialCapacity = 4)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity can not be negative.");
            }

            this.elements = new T[initialCapacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// </summary>
        /// <param name="values">
        /// Values that will be added to the dictionary.
        /// </param>
        /// <param name="initialCapacity">
        /// Queue's initial capacity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if initialCapacity &lt;= 0.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if values is null.
        /// </exception>
        public Queue(IEnumerable<T> values, int initialCapacity = 4)
        {
            ThrowForInvalidParameters();

            this.elements = new T[initialCapacity];

            foreach (T value in values)
            {
                this.Enqueue(value);
            }

            void ThrowForInvalidParameters()
            {
                if (values == null)
                {
                    throw new ArgumentNullException(nameof(values));
                }

                if (initialCapacity <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity can not be negative.");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue{T}"/> class.
        /// Initial capacity will be twice passed ICollection's length. 
        /// </summary>
        /// <param name="values">
        /// Values that will be added to the dictionary.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if values is null.
        /// </exception>
        public Queue(ICollection<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.elements = new T[2 * values.Count];

            foreach (T value in values)
            {
                this.Enqueue(value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets capacity of this queue.
        /// </summary>
        public int Capacity => this.elements.Length;

        /// <summary>
        /// Gets amount of elements in queue.
        /// </summary>
        public int Size => this.head > this.tail ?
                           this.Capacity - this.head + this.tail + 1 :
                           this.tail - this.head + 1;

        #endregion

        #region Public Methods

        /// <summary>
        /// Enqueues value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Enqueue(T value)
        {
            if (this.Size == this.elements.Length)
            {
                this.Resize();
            }

            this.elements[this.tail] = value;

            MoveTail();

            this.UpdateVersion();

            void MoveTail()
            {
                this.tail++;
                if (this.tail == this.Capacity)
                {
                    this.tail = 0;
                }
            }
        }

        /// <summary>
        /// Returns element from this queue's head.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this queue have no elements.
        /// </exception>
        public T Dequeue()
        {
            if (this.elements.Length == 0)
            {
                throw new InvalidOperationException("Cannot dequeue from empty queue.");
            }

            T value = this.elements[this.head];
            
            MoveHead();

            this.UpdateVersion();

            return value;

            void MoveHead()
            {
                this.head++;
                if (this.head == this.Capacity)
                {
                    this.head = 0;
                }
            }
        }

        /// <summary>
        /// Checks if this queue is empty.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if empty, false otherwise.
        /// </returns>
        public bool IsEmpty() => this.head == this.tail;

        /// <summary>
        /// Reduces this queue's capacity to its size.
        /// </summary>
        public void ShrinkToFit() => Array.Resize(ref this.elements, this.Size);

        #endregion

        #region GetEnumerator Methods

        /// <summary>
        /// Returns this queue's enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="QueueEnumerator"/>.
        /// This queue's enumerator.
        /// </returns>
        public QueueEnumerator GetEnumerator() => new QueueEnumerator();

        /// <summary>
        /// Returns this queue's enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator&lt;T&gt;"/>.
        /// This queue's enumerator.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Returns this queue's enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// This queue's enumerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region Private Methods

        /// <summary>
        /// Resizes queue by doubling its capacity.
        /// </summary>
        private void Resize()
        {
            int newCapacity = this.Capacity != 0 ? 2 * this.Capacity : 1;
            
            var newElements = new T[newCapacity];

            CopyElements();

            this.head = 0;
            this.tail = this.Capacity - 1;
            this.elements = newElements;

            // Copies elements from this.elements 
            // array to newElements array.
            void CopyElements()
            {
                int oldElementsIndex = this.head;
                int newElementsIndex = 0;

                if (this.tail <= this.head)
                {
                    int size = this.Size;
                    while (oldElementsIndex < size)
                    {
                        newElements[newElementsIndex] = this.elements[oldElementsIndex];

                        newElementsIndex++;
                        oldElementsIndex++;
                    }

                    oldElementsIndex = 0;
                }

                while (oldElementsIndex <= this.tail)
                {
                    newElements[newElementsIndex] = this.elements[oldElementsIndex];

                    newElementsIndex++;
                    oldElementsIndex++;
                }
            }
        }

        /// <summary>
        /// The update version.
        /// </summary>
        private void UpdateVersion() => this.version++;

        #endregion

        #region Enumerator

        /// <summary>
        /// The Queue's class enumerator.
        /// </summary>
        public struct QueueEnumerator : IEnumerator<T>
        {
            #region Private Fields

            /// <summary>
            /// Initial version of the enumerated queue.
            /// </summary>
            private readonly int initialVersion;

            /// <summary>
            /// Queue that is enumerated.
            /// </summary>
            private readonly Queue<T> queue;

            /// <summary>
            /// Current element's position.
            /// </summary>
            private int position;

            #endregion

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueEnumerator"/> struct.
            /// </summary>
            /// <param name="queue">
            /// Queue that will be enumerated.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// Thrown if passed queue is null.
            /// </exception>
            public QueueEnumerator(Queue<T> queue)
            {
                if (queue == null)
                {
                    throw new ArgumentNullException(nameof(queue));
                }

                this.initialVersion = queue.version;
                this.queue = queue;
                this.position = -1;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets current element of enumerated queue.
            /// </summary>
            public T Current => this.queue.elements[this.position];

            /// <summary>
            /// Gets current element of enumerated queue.
            /// </summary>
            object IEnumerator.Current => this.Current;

            #endregion

            #region Public Methods

            /// <summary>
            /// Moves Current to the next element if it exists.
            /// </summary>
            /// <returns>
            /// The <see cref="bool"/>.
            /// True if next element exists, false otherwise.
            /// </returns>
            /// <exception cref="InvalidOperationException">
            /// Thrown if queue was modified during the enumeration.
            /// </exception>
            public bool MoveNext()
            {
                if (this.initialVersion != this.queue.version)
                {
                    throw new InvalidOperationException("Queue was changed during the enumeration.");
                }

                this.position++;

                return this.position < this.queue.Size;
            }

            /// <summary>
            /// Resets this Enumerator.
            /// </summary>
            public void Reset()
            {
                this.position = -1;
            }

            /// <summary>
            /// Dispose method.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }

        #endregion
    }
}
