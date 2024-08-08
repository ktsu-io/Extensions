namespace ktsu.io.Extensions.Tests;

using System.Collections.Generic;
using System.Linq;
using ktsu.io.DeepClone;
using ktsu.io.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class EnumerableExtensionsTests
{
	[TestMethod]
	public void WithIndex_EnumeratesWithIndex()
	{
		string[] items = ["a", "b", "c"];
		var result = items.WithIndex().ToList();

		Assert.AreEqual(3, result.Count);
		Assert.AreEqual(("a", 0), result[0]);
		Assert.AreEqual(("b", 1), result[1]);
		Assert.AreEqual(("c", 2), result[2]);
	}

	[TestMethod]
	public void ToCollection_CreatesCollection()
	{
		int[] items = [1, 2, 3];
		var collection = items.ToCollection(items);

		Assert.AreEqual(3, collection.Count);
		Assert.AreEqual(1, collection[0]);
		Assert.AreEqual(2, collection[1]);
		Assert.AreEqual(3, collection[2]);
	}

	[TestMethod]
	public void ForEach_AppliesAction()
	{
		int[] items = [1, 2, 3];
		int sum = 0;
		items.ForEach(item => sum += item);

		Assert.AreEqual(6, sum);
	}

	[TestMethod]
	public void ToCollection_ThrowsOnNull()
	{
		IEnumerable<int> items = null!;
		Assert.ThrowsException<ArgumentNullException>(() => items.ToCollection());
	}

	[TestMethod]
	public void ForEach_ThrowsOnNullEnumerable()
	{
		IEnumerable<int> items = null!;
		Assert.ThrowsException<ArgumentNullException>(() => items.ForEach(item => { }));
	}

	[TestMethod]
	public void ForEach_ThrowsOnNullAction()
	{
		int[] items = [1, 2, 3];
		Assert.ThrowsException<ArgumentNullException>(() => items.ForEach(null!));
	}

	public class SampleItem : IDeepCloneable<SampleItem>
	{
		public int Value { get; set; }

		public SampleItem DeepClone()
		{
			return new SampleItem { Value = Value };
		}
	}

	[TestMethod]
	public void DeepClone_ShouldCloneCollectionCorrectly()
	{
		// Arrange
		var originalItems = new List<SampleItem>
		{
			new() { Value = 1 },
			new() { Value = 2 },
			new() { Value = 3 }
		};

		// Act
		var clonedItems = originalItems.DeepClone<SampleItem, List<SampleItem>>();

		// Assert
		Assert.AreEqual(originalItems.Count, clonedItems.Count);
		for (int i = 0; i < originalItems.Count; i++)
		{
			Assert.AreNotSame(originalItems[i], clonedItems[i]);
			Assert.AreEqual(originalItems[i].Value, clonedItems[i].Value);
		}
	}

	[TestMethod]
	public void DeepClone_ShouldThrowArgumentNullException_WhenItemsIsNull()
	{
		// Arrange
		List<SampleItem> originalItems = null!;

		// Act & Assert
		Assert.ThrowsException<ArgumentNullException>(originalItems.DeepClone<SampleItem, List<SampleItem>>);
	}

	[TestMethod]
	public void DeepClone_WithLockObj_ShouldCloneCollectionCorrectly()
	{
		// Arrange
		var originalItems = new List<SampleItem>
		{
			new() { Value = 1 },
			new() { Value = 2 },
			new() { Value = 3 }
		};
		object lockObj = new();

		// Act
		var clonedItems = originalItems.DeepClone<SampleItem, List<SampleItem>>(lockObj);

		// Assert
		Assert.AreEqual(originalItems.Count, clonedItems.Count);
		for (int i = 0; i < originalItems.Count; i++)
		{
			Assert.AreNotSame(originalItems[i], clonedItems[i]);
			Assert.AreEqual(originalItems[i].Value, clonedItems[i].Value);
		}
	}

	[TestMethod]
	public void DeepClone_WithLockObj_ShouldThrowArgumentNullException_WhenItemsIsNull()
	{
		// Arrange
		List<SampleItem> originalItems = null!;
		object lockObj = new();

		// Act & Assert
		Assert.ThrowsException<ArgumentNullException>(() => originalItems.DeepClone<SampleItem, List<SampleItem>>(lockObj));
	}

	[TestMethod]
	public void DeepClone_WithLockObj_ShouldThrowArgumentNullException_WhenLockObjIsNull()
	{
		// Arrange
		var originalItems = new List<SampleItem>
		{
			new() { Value = 1 },
			new() { Value = 2 },
			new() { Value = 3 }
		};
		object lockObj = null!;

		// Act & Assert
		Assert.ThrowsException<ArgumentNullException>(() => originalItems.DeepClone<SampleItem, List<SampleItem>>(lockObj));
	}

	[TestMethod]
	public void DeepClone_WithLockObj_ShouldLockAndCloneCorrectly()
	{
		// Arrange
		var originalItems = new List<SampleItem>
		{
			new() { Value = 1 },
			new() { Value = 2 },
			new() { Value = 3 }
		};
		object lockObj = new();
		bool wasLocked = false;

		// Act
		lock (lockObj)
		{
			wasLocked = true;
			var clonedItems = originalItems.DeepClone<SampleItem, List<SampleItem>>(lockObj);
			wasLocked = false;
		}

		// Assert
		Assert.IsFalse(wasLocked, "The lock object should have been released after the method executed.");
	}
}
