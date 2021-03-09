# SQLToNeo4j
A class library used to migrate SQL Server graph database into Neo4j.

# Published Articles

- [Migrating SQL Server graph databases to Neo4j](https://www.sqlshack.com/migrating-sql-server-graph-databases-to-neo4j/)
- Export Indexes and Constraints from SQL Server graph database to Neo4j (Under review)

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
            using (SQLReader reader = new SQLReader("Server=DESKTOP-KCL006K\\DATASERVER;Database=GraphPL;Trusted_Connection=yes;"))
            {
                reader.GetNodes();
                reader.GetEdges();
                reader.GetIndexes();
                reader.GetFullTextIndexes();
                reader.GetUniqueConstraints();
                //reader.GetExistenceConstraints(); -- available only in enterprise edition
                //reader.GetNodeKeyConstraints(); -- available only in enterprise edition

                using (Neo4jWriter importer = new Neo4jWriter(new Uri("http://neo4j:123@localhost:7474")))
                {
                    importer.ImportNodes(reader.Nodes);
                    importer.ImportEdges(reader.Edges);
                    importer.ImportIndexes(reader.Indexes);
                    importer.ImportFullTextIndexes(reader.FullTextIndexes);
                    importer.ImportConstraints(reader.UniqueConstraints);
                    //importer.ImportConstraints(reader.ExistenceConstraints); -- available only in enterprise edition
                    //importer.ImportConstraints(reader.NodeKeyConstraints); -- available only in enterprise edition
                }
            }
            }
        }
    }
}

```
