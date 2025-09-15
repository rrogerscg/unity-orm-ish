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
        Assert.AreEqual("Users", UserCharacter.Table.Name);
        Assert.AreEqual("Scores", Score.Table.Name);
    }

    [Test]
    public void TestDeleteAllRecords()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        UserCharacter user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        UserCharacter user3 = new("John Doe3", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, UserCharacter.GetAll().Count);

        UserCharacter.Table.DeleteAllRecords();
        CollectionAssert.IsEmpty(UserCharacter.GetAll());
    }

    [Test]
    public void TestSaveTable()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        UserCharacter user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        UserCharacter user3 = new("John Doe3", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, UserCharacter.GetAll().Count);

        UserCharacter.SaveTable();
        UserCharacter.ReloadRecords();
        Assert.AreEqual(3, UserCharacter.GetAll().Count);
        Assert.IsTrue(UserCharacter.Table.Records.ContainsKey(user.Id));
        Assert.IsTrue(UserCharacter.Table.Records.ContainsKey(user2.Id));
        Assert.IsTrue(UserCharacter.Table.Records.ContainsKey(user3.Id));
    }
}

[TestFixture]
public class RecordTests : BaseTestFixture
{

    [Test]
    public void TestHasId()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        Assert.AreNotEqual(Guid.Empty, user.Id);
    }

    [Test]
    public void TestGetAllOnEmpty()
    {
        CollectionAssert.IsEmpty(UserCharacter.GetAll());
    }

    [Test]
    public void TestDelete()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual(1, UserCharacter.GetAll().Count);

        user.Delete();
        Assert.AreEqual(0, UserCharacter.GetAll().Count);
    }

    [Test]
    public void TestDeleteAllRecords()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        UserCharacter user2 = new("John Doe1", "red", "green", "blue");
        user2.Put();
        UserCharacter user3 = new("John Doe2", "red", "green", "blue");
        user3.Put();
        Assert.AreEqual(3, UserCharacter.GetAll().Count);

        UserCharacter.DeleteAll();
        CollectionAssert.IsEmpty(UserCharacter.GetAll());
    }

    [Test]
    public void TestGetDerivedTypes()
    {
        List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(Record<>));
        Assert.AreEqual(2, derivedTypes.Count);
        Assert.AreEqual(typeof(Score), derivedTypes[0]);
        Assert.AreEqual(typeof(UserCharacter), derivedTypes[1]);
    }

    [Test]
    public void TestRecordPut()
    {

        CollectionAssert.IsEmpty(UserCharacter.Table.Records);
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        UserCharacter user2 = new("John Doe2", "red", "green", "blue");
        user2.Put();
        Assert.AreEqual(2, UserCharacter.GetAll().Count);
    }

    [Test]
    public void TestRecordGetById()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual("John Doe", user.Name);
        UserCharacter userReload = UserCharacter.GetById(user.Id);
        Assert.AreEqual(user, userReload);
    }

    [Test]
    public void TestRecordPersistence()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.Put();
        Assert.AreEqual("John Doe", user.Name);
        UserCharacter userReload = UserCharacter.GetById(user.Id);
        Assert.AreEqual("John Doe", userReload.Name);

        user.Name = "John Doe Updated";
        user.Put();

        UserCharacter userReloaded = UserCharacter.GetById(user.Id);
        Assert.AreEqual("John Doe Updated", userReloaded.Name);
    }

    [Test]
    public void TestRecordSetsToActive()
    {
        UserCharacter user = new("John Doe", "red", "green", "blue");
        user.SetAsActiveUserCharacter();
        Assert.IsTrue(user.IsActive);
        user.Put();
        Assert.IsTrue(user.IsActive);
        UserCharacter userReload = UserCharacter.GetById(user.Id);
        Assert.IsTrue(userReload.IsActive);
    }
}
