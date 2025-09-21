# unity-orm-ish

### Unity - Object Relation Mapping - I Shouldnt Have
Was so concerned about if I could, I didnt think about if I should... I Shouldnt Have - but it is ORM... ish

#### About ORM-ISH
This is an object-relation-mapping built using a JSON flat-file database.
This project started out as a way to learn C# at a deeper level.
I wanted to really push my understanding of generics, as I know they can be quite powerful for code modularity.
Another reason this project was created was my need for a light-weight, local database for my cross-platform game created with Unity.

#### What this project is
Cross-platform<br>
Extremely light-weight<br>
Viable for tables with a few thousand entries<br>

#### What this project is NOT
This is not scalable<br>
This is not an enterprise-ready solution

#### Features
Data-binding layer<br>
NoSQL database type<br>
Easy unit testing<br>
Memory Caching to limit I/O operations


#### Features TODO (In no particular order)
Tempfile writes and backups<br>
More robust unique field system<br>
Option to enforce a field to not be null<br>
Index system for caching complex lookups<br>
FK relationships with helper methods for loading

#### Usage Example
If you have experience with the Google Datastore NDB(I know...) library for Python, then you already know how to use this, as it's based off that with similar verbs for CRUD operations.

1. Create your model class
```csharp
using System;
using ORMish;

[Serializable]
public class User : Record<User>
{
    public string Name { get; set; }
    public int Age { get; set; }

    // Required constructor to use for generic type
    public Score() : base()
    {

    }

    public Score(string name, int age) : base()
    {
        Name = name;
        Age = age;
    }
}
```
2. Instantiate a model instance, commmit to in-memory database, then perist to JSON file
```csharp
//Initialize Database Manager singleton and then call Initialize method with directory where you want the 'tables' folder to be created
DatabaseManager.Initialize(<path/to/local/persistence/folder>);
// create record
User user = new User("rrogerscg", "37");
// add record to in-memory database (record by record id hash table)
user.Put();
// persist to JSON file
User.Table.Save();
```
