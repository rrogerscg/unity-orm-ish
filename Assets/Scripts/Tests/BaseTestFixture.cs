using ORMish;
using NUnit.Framework;
using System.IO;
using UnityEngine;

public abstract class BaseTestFixture
{
    protected readonly string tableDirectory = Path.Combine(Application.temporaryCachePath, "tables");
    [SetUp]
    public virtual void SetUp()
    {
        DatabaseManager.Initialize(tableDirectory);
    }

    [TearDown]
    public virtual void TearDown()
    {
        TableRegistry.Instance.ClearAllTables();
    }
}
