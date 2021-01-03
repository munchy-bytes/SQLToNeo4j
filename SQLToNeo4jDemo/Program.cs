using System;
using SQLToNeo4j;
namespace SQLToNeo4jDemo
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
