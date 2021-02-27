# SQLToNeo4j
A class library used to migrate SQL Server graph database into Neo4j.

# Dependencies

- [Neo4jClient](https://www.nuget.org/packages/Neo4jClient/)

## Example
```csharp
using System;

namespace SQLToNeo4j
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SQLReader reader = new SQLReader("Server=<machine name>\\<instance name>;Database=GraphPL;Trusted_Connection=yes;"))
            {
                reader.GetNodes();
                reader.GetEdges();

                using (Neo4jWriter importer = new Neo4jWriter(new Uri("http://<user>:<password>@localhost:7474")))
                {
                    importer.ImportNodes(reader.Nodes);
                    importer.ImportEdges(reader.Edges);
                }
            }
        }
    }
}

```
