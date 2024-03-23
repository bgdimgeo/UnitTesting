namespace Database.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class DatabaseTests
    {
        private Database db;
        [SetUp]
        public void SetUp()
        {
            this.db = new Database();
        }
        [Test]
        public void IntilizeDatabase()
        {
            Database database = new Database();
            Assert.IsNotNull(database);
        }
        [Test]
        public void CheckFirstValue()
        {
            Database database = new Database(1, 2, 3, 4);
            int[] numbers = database.Fetch();
            Assert.AreEqual(1, numbers[0]);
        }
        [Test]
        public void CountMustReturnActualCount()
        {
            Database database = new Database(1, 2, 3, 4);
            Assert.That(database.Count, Is.EqualTo(4));
        }
        [Test]
        public void CheckAddMethod()
        {
            Database database = new Database(1, 2, 3, 4);
            database.Add(99);
            int[] numbers = database.Fetch();
            Assert.AreEqual(99, numbers[numbers.Length - 1]);

        }
        [Test]

        [TestCase(new int[] { })]
        [TestCase(new int[] { })]
        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]

        public void ConstructorShouldAddLessThan16Elements(int[] elementsToAdd)
        {
            //Arrange
            Database testDb = new Database(elementsToAdd);

            //Act

            int[] actualData = testDb.Fetch();

            int[] expectedData = elementsToAdd;

            int actualCount = testDb.Count;
            int expectedCount = expectedData.Length;


            //Asssert
            CollectionAssert.AreEqual(expectedData, actualData,
                "Database constructur should initialize data field correctly!");

            Assert.AreEqual(expectedCount, actualCount,
                "Constructor should set intial value for count field!");

        }
        public void CheckOutOfRangeException()
        {
            string returnValue = string.Empty;

            try
            {
                Database database = new Database(1, 3, 4, 5, 6, 7, 1, 2, 3, 4, 5, 6, 7, 2, 2, 2, 2, 2, 2);
            }
            catch (InvalidOperationException ex)
            {

                returnValue = ex.Message;

            }


            Assert.That(returnValue, Is.EqualTo("Array's capacity must be exactly 16 integers!"));


        }

        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        public void ConstructorMustNotAllowToExceedMaximumCount(int[] elementsToAdd)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Database testDb = new Database(elementsToAdd);
            }, "Array's capacity must be exactly 16 integers!");
        }

        [Test]
        public void CountMustReturnZeroWhenNoElements()
        {
            int actualCount = this.db.Count;
            int expectedCount = 0;
            Assert.AreEqual(expectedCount, actualCount, "Count is 0 - OK");
        }
        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        public void AddShouldAddLessThan16Elements(int[] elementsToAdd)
        {
            foreach (var el in elementsToAdd)
            {
                this.db.Add(el);
            }
            int[] actualData = this.db.Fetch();
            int[] expectedData = elementsToAdd;

            int actualCount = this.db.Count;
            int expectedCount = expectedData.Length;

            CollectionAssert.AreEqual(expectedData, actualData);
            Assert.AreEqual(expectedCount, actualCount);
        }
        [Test]
        public void AddShouldThrowExceptionWhenAddedMoreThan16Elements()
        {
            for (int i = 0; i < 16; i++)
            {
                this.db.Add(i);
            }
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.db.Add(17);
            }, "Array's capacity must be exactly 16 integers!");
        }
        [TestCase(new int[] { 1 })]
        public void RemoveShouldRemoveTheLastElementSuccessfully(int[] startElements)
        {
            //Act
            foreach (var el in startElements)
            {
                this.db.Add(el);
            }
            this.db.Remove();
            List<int> elementList = new List<int>(startElements);
            elementList.RemoveAt(elementList.Count - 1);

            int[] actualData = this.db.Fetch();
            int[] expectedData = elementList.ToArray();

            int actualCount = this.db.Count;
            int expectedCount = expectedData.Length;

            CollectionAssert.AreEqual(expectedData, actualData
                , "Remove should physically remove the element in the data field!");
            Assert.AreEqual(expectedCount, actualCount, "Remove should decrement the count of the Database!");
        }

        [Test]
        public void RemoveShouldRemoveTheLastElementMoreThanOnce()
        {
            List<int> initData = new List<int>() { 1, 2, 3 };
            foreach (int el in initData)
            {
                this.db.Add(el);
            }
            for (int i = 0; i < initData.Count; i++)
            {
                this.db.Remove();
            }
            int[] actualData = this.db.Fetch();
            int[] expectedData = new int[] { };

            int actualCount = this.db.Count;
            int expectedCount = 0;

            CollectionAssert.AreEqual(expectedData, actualData,
                "Remove should physically remove the element in the data field");
            Assert.AreEqual(expectedCount, actualCount, "Remove should decrement the count of the Database!");
        }

        [Test]
        public void RemoveShouldThrowErrorWhenThereAreNoElements() 
        {
            Assert.Throws<InvalidOperationException>(() => { this.db.Remove(); }, "The collection is empty!");
        }

        [TestCase(new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        public void FetchShouldReturnCopyArray(int[] initData) 
        {
            //Act
            foreach (int el in initData) 
            {
                this.db.Add(el);
            }
            int[] actualResult = this.db.Fetch();
            int[] expectedResult = initData;
            CollectionAssert.AreEqual(expectedResult, actualResult, "Fetch should return copy of the existing data!");


        }
    }
}
