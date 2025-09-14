using ORMish;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[TestFixture]
public class TableTests : BaseTestFixture
{

    [Test]
    public void TableDirectoryExists()
    {
        Assert.IsTrue(Directory.Exists(tableDirectory), "The table directory should.");
    }

    [Test]
    public void TestCreateInstance()
    {
        Assert.AreEqual("Users", User.Table.Name);
        Assert.AreEqual("Scores", Score.Table.Name);
    }

    [Test]
    public void TestDeleteAllRecords()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        User user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        User user3 = new("John Doe3", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, User.GetAll().Count);

        User.Table.DeleteAllRecords();
        CollectionAssert.IsEmpty(User.GetAll());
    }

    [Test]
    public void TestSaveTable()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        User user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        User user3 = new("John Doe3", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, User.GetAll().Count);

        User.SaveTable();
        User.ReloadRecords();
        Assert.AreEqual(3, User.GetAll().Count);
        Assert.IsTrue(User.Table.Records.ContainsKey(user.Id));
        Assert.IsTrue(User.Table.Records.ContainsKey(user2.Id));
        Assert.IsTrue(User.Table.Records.ContainsKey(user3.Id));
    }
}

[TestFixture]
public class RecordTests : BaseTestFixture
{

    [Test]
    public void TestHasId()
    {
        User user = new("John Doe", "red", "green", "blue");
        Assert.AreNotEqual(Guid.Empty, user.Id);
    }

    [Test]
    public void TestGetAllOnEmpty()
    {
        CollectionAssert.IsEmpty(User.GetAll());
    }

    [Test]
    public void TestDelete()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual(1, User.GetAll().Count);

        user.Delete();
        Assert.AreEqual(0, User.GetAll().Count);
    }

    [Test]
    public void TestDeleteAllRecords()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        User user2 = new("John Doe1", "red", "green", "blue");
        user2.Put();
        User user3 = new("John Doe2", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, User.GetAll().Count);

        User.DeleteAll();
        CollectionAssert.IsEmpty(User.GetAll());
    }

    [Test]
    public void TestGetDerivedTypes()
    {
        List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(Record<>));
        Assert.AreEqual(2, derivedTypes.Count);
        Assert.AreEqual(typeof(Score), derivedTypes[0]);
        Assert.AreEqual(typeof(User), derivedTypes[1]);
    }

    [Test]
    public void TestRecordPut()
    {

        CollectionAssert.IsEmpty(User.Table.Records);
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        User user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        Assert.AreEqual(2, User.GetAll().Count);
    }

    [Test]
    public void TestRecordGetById()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual("John Doe", user.Name);
        User userReload = User.GetById(user.Id);
        Assert.AreEqual(user, userReload);
    }

    [Test]
    public void TestRecordPersistence()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual("John Doe", user.Name);
        User userReload = User.GetById(user.Id);
        Assert.AreEqual("John Doe", userReload.Name);

        user.Name = "John Doe Updated";
        user.Put();

        User userReloaded = User.GetById(user.Id);
        Assert.AreEqual("John Doe Updated", userReloaded.Name);
    }

    [Test]
    public void TestRecordSetsToActive()
    {
        User user = new("John Doe", "red", "green", "blue");
        user.SetAsActiveUser();
        Assert.IsTrue(user.IsActive);
        user.Put();
        Assert.IsTrue(user.IsActive);
        User userReload = User.GetById(user.Id);
        Assert.IsTrue(userReload.IsActive);
    }
}
