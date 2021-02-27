using System;
using System.Collections.Generic;
using System.Text;

namespace SQLToNeo4j
{
    public static class SQLcommands
    {
        public static string GetIndexes = "SELECT 'INDEX ' + ind.name + ' FOR (t:' + t.name +') ON (' + STRING_AGG('t.' + Col.name,',') + ')'" +
                                " FROM sys.indexes ind INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id INNER JOIN sys.tables t ON ind.object_id = t.object_id" +
                                " WHERE ind.type = 2 and (t.is_node = 1 or t.is_edge = 1) and col.name not like 'graph_id_%' and t.name <> 'sysdiagrams' and ind.is_unique = 0 and ind.is_unique_constraint = 0 " +
                                " GROUP BY  ind.name,  t.name";
        public static string GetFullTextIndexes = "SELECT 'db.index.fulltext.' + case t.is_node when 1 then 'createNodeIndex(\"N_' else 'createRelationshipIndex(\"Rel_' end  + cat.name + '\", [' + STRING_AGG('\"' + t.name + '\"',', ') + '], [' + STRING_AGG('\"' + col.name + '\"',', ') + '])' " +
                                " FROM sys.fulltext_catalogs cat INNER JOIN sys.fulltext_indexes ind ON cat.fulltext_catalog_id = ind.fulltext_catalog_id INNER JOIN sys.fulltext_index_columns ic ON  ind.object_id = ic.object_id " +
                                " INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                                " WHERE (t.is_node = 1 or t.is_edge = 1) and col.name not like 'graph_id_%' and t.name <> 'sysdiagrams' " +
                                " GROUP BY t.is_node, cat.name";
        public static string GetUniqueConstraints = "SELECT 'CONSTRAINT UC_' + ind.name + ' ON (n:' + t.name + ') ASSERT n.' + col.name + ' IS UNIQUE' " +
                                " FROM sys.indexes ind INNER JOIN  sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id " +
                                " INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id " +
                                " INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                                " WHERE ind.type = 2 	and (t.is_node = 1 or t.is_edge = 1) and col.name not like 'graph_id_%' and t.name <> 'sysdiagrams' " +
                                "	and (ind.is_unique = 1 or ind.is_unique_constraint = 1) " +
                                " GROUP BY ind.name,t.name,col.name" +
                                " HAVING COUNT(*) = 1";
        public static string GetExistenceConstraints = "select CASE WHEN t.is_node = 1 then 'CONSTRAINT ' + t.name + '_' + col.name + '_exist ON (n:' + t.name + ') ASSERT EXISTS (n.' + col.name + ')' ELSE 'CONSTRAINT ' + t.name + '_' + col.name + '_exist ON ()-\"[\"R:' + t.name + '\"]\"-() ASSERT EXISTS (R.' + col.name + ')' END " +
                                " FROM sys.columns col INNER JOIN  sys.tables t ON col.object_id = t.object_id " +
                                " where col.is_nullable = 0 and col.graph_type IS NULL and (t.is_edge = 1 or t.is_node = 1) " +
                                "	and not exists (select * from sys.index_columns ic inner join sys.indexes ind on ic.index_id = ind.index_id and ind.object_id = ic.object_id " +
                                "		where ind.is_primary_key = 1 and ic.column_id = col.column_id and t.object_id = ind.object_id and t.is_node = 1)";
        public static string GetNodeKeyConstraints = "select 'CONSTRAINT NKC_' + t.name + ' ON (n:' + t.name + ') ASSERT (' + STRING_AGG('n.' + col.name,',') + ') IS NODE KEY' " +
                                " FROM sys.indexes ind  INNER JOIN sys.index_columns ic ON ind.object_id = ic.object_id and ind.index_id = ic.index_id INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                                " INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id " +
                                " where  graph_type IS NULL -- Ignore graph system columns and t.is_node = 1 and ind.is_primary_key = 1 " +
                                " GROUP BY t.name";


    }
}
