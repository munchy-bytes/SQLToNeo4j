using System;
using SQLToNeo4j;
namespace SQLToNeo4jDemo
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
