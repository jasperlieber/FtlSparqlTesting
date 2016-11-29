using UnityEngine;
using System.Collections;
using VDS.RDF;
using VDS.RDF.Query;
using UnityEngine.UI;
using System;

public class DotNetRdfManager : MonoBehaviour {

    public string mOntologyFilename;
    //public string mSprqlQuery;
    public Text mSprqlResult;

    // Use this for initialization
    void Start () {
        Debug.Log("start");

        string mSprqlQuery = System.IO.File.ReadAllText(@"Assets\query.txt");

        mSprqlResult.text = "FTL Sparql Query Tester (2016-11-15) -- "
            + "Using ontology in file '" + mOntologyFilename
            + "'\n\nQuery:\n\n" + mSprqlQuery.Trim() + "\n\n";

        //Debug.Log(text);

        //Graph g = new Graph();
        //IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
        //IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
        //ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
        //ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");

        //g.Assert(new Triple(dotNetRDF, says, helloWorld));
        //g.Assert(new Triple(dotNetRDF, says, bonjourMonde));

        //foreach (Triple t in g.Triples)
        //{
        //    Debug.Log(t.ToString());
        //}

        //Define your Graph here - it may be better to use a QueryableGraph if you plan
        //on making lots of Queries against this Graph as that is marginally more performant
        IGraph g = new Graph();

        //Load some data into your Graph using the LoadFromFile() extension method
        g.LoadFromFile(mOntologyFilename);// "myfile.rdf");

        //int cnt = 1;
        //foreach (object tt in g.Triples)
        //{
        //    Debug.Log(cnt++ + ": " + tt.ToString());
        //}

        //Debug.Log("g: " + g.Nodes.ToString());

        //Use the extension method ExecuteQuery() to make the query against the Graph
        try
        {
            //string q1 = "SELECT * WHERE { ?s a ?type }";
            //string q2 = "SELECT ?x WHERE { ?x <http://www.w3.org/2001/vcard-rdf/3.0#FN>  \"John Smith\" }";

            object results = g.ExecuteQuery(mSprqlQuery);
            if (results is SparqlResultSet)
            {
                //SELECT/ASK queries give a SparqlResultSet
                SparqlResultSet rset = (SparqlResultSet)results;

                string msg = rset.Count + " results:";

                mSprqlResult.text += "\n" + msg;
                Debug.Log(msg);

                int cnt = 0;
                foreach (SparqlResult r in rset)
                {
                    cnt++;
                    msg = "#" + cnt + ": " + r.ToString();
                    Debug.Log(msg);
                    mSprqlResult.text += "\n" + msg;
                    //Do whatever you want with each Result
                }
            }
            else if (results is IGraph)
            {
                //CONSTRUCT/DESCRIBE queries give a IGraph
                IGraph resGraph = (IGraph)results;

                mSprqlResult.text += "\n" + "IGraph results:\n";

                int cnt = 0;
                foreach (Triple t in resGraph.Triples)
                {
                    string msg = "Triple #" + ++cnt + ": " + t.ToString();
                    Debug.Log(msg);
                    mSprqlResult.text += msg;
                    //Do whatever you want with each Triple
                }
            }
            else
            {
                //If you don't get a SparqlResutlSet or IGraph something went wrong 
                //but didn't throw an exception so you should handle it here
                string msg = "ERROR, or no results";
                Debug.Log(msg);
                mSprqlResult.text += "\n" + msg;
            }
        }
        catch (RdfQueryException queryEx)
        {
            //There was an error executing the query so handle it here
            Debug.Log(queryEx.Message);
            mSprqlResult.text += queryEx.Message     ;
        }

        Debug.Log(mSprqlResult.text + "\n\n");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
