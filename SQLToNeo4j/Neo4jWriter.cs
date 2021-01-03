using System;
using System.Collections.Generic;
using Neo4jClient;

namespace SQLToNeo4j
{
    public class Neo4jWriter : IDisposable
    {
        private GraphClient client;

        public Neo4jWriter(Uri uri)
        {
            client = new GraphClient(uri);
            client.ConnectAsync().Wait();
        }

        public void ImportNodes(List<Node> nodes)
        {
            foreach(Node nd in nodes)
            {
                string cypher =
                "(" + nd.Label + "_" + nd.ID.ToString() + ":" + nd.Label + " $prop)";
                client.Cypher.Create(cypher).WithParam("prop", nd.Properties).ExecuteWithoutResultsAsync().Wait(); ;
            }        
        }

        public void ImportEdges(List<Edge> edges)
        {
            foreach (Edge edg in edges)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("prop", edg.Properties);
                param.Add("id1", edg.FromNodeID);
                param.Add("id2", edg.ToNodeID);
                string cypher =
                "(" + edg.FromNode.ToLower() + "1" +  ") -[:" + edg.Label + " $prop]->(" + edg.ToNode.ToLower() + "2" +  ")";
                client.Cypher
                    .Match("(" + edg.FromNode.ToLower() + "1:" + edg.FromNode + ")", "(" + edg.ToNode.ToLower() + "2:" + edg.ToNode + ")")
                    .Where(edg.FromNode.ToLower() + "1.node_id = $id1")
                    .AndWhere(edg.ToNode.ToLower() + "2.node_id = $id2")
                    .Create(cypher)
                    .WithParams(param).ExecuteWithoutResultsAsync().Wait(); ;
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
