MicroSolr
=========

A simple fast/parallel connector for SOLR.

Requirements
============
.Net 4.0.
Some knowledge of Solr (http://lucene.apache.org/solr/).
Keep It Simple attitude.

Intro
=====
A sample console application to connect and query to Solr is given below:


var solr = MicroSolr.Connectors.SimpleConnector<T>.Create(@"http://localhost.:8983/solr/", "collection1");
solr.Save(new T { prop1=1, prop2=2.... });
var results = solr.Query("*:*");

The idea is to keep things simple with minimum lines of code.

How to Get It?
=============
Use NuGet. See the link below for more details:
http://nuget.org/packages/MicroSolr.Connectors

