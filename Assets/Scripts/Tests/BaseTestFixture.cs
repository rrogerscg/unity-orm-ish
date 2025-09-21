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
        DatabaseManager.Initialize(tableDirectory);
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
