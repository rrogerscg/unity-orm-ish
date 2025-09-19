using ORMish;
using NUnit.Framework;
using System.IO;
using UnityEngine;
using Example;

public abstract class BaseTestFixture
{
    protected string tableDirectory = Path.Combine(Application.temporaryCachePath, "tables");
    [SetUp]
    public virtual void SetUp()
    {
        tableDirectory = Path.Combine(Application.temporaryCachePath, "tables");
        UserCharacter.Table = new Table<UserCharacter>();
        Score.Table = new Table<Score>();
    }

    [TearDown]
    public virtual void TearDown()
    {
        if (Directory.Exists(tableDirectory))
        {
            Directory.Delete(tableDirectory, true);
        }
    }
}
