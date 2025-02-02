using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Azure;
using Azure.Core;
using Azure.Storage.Blobs;
using System.Text;
using Azure.Data.Tables;

namespace WebApp1
{
    public partial class About : Page
    {
        private bool loading = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                loading = true;
                Label1.Text = "Currently loading..";
                var client = new WebClient();
                string error_mes = "Error! Input not found!";
                try
                {
                    var content = client.DownloadString("[this is where you put the data you want to download]");
                    // to test using the table this was tested with, upload the contents of the file "Teacher's Table.txt" to a website.
                    Label1.Text = content;
                    if (content == "")
                    {
                        Label1.Text = error_mes;
                        loading = false;
                        return;
                    }
                    error_mes = "Error! Something is wrong with the Blob Database!";
                    Label1.Text = "Got Blob Database..";
                    Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(content ?? ""));
                    BlobClient blob_client = new BlobClient("DefaultEndpointsProtocol=https;AccountName=[input your own];AccountKey=[ditto];BlobEndpoint=[again];TableEndpoint=[hmm];QueueEndpoint=[yeah];FileEndpoint=[indeed]", "the-container", "the-blob");
                    blob_client.Upload(stream, true);
                    Label1.Text = "Uploaded to Blob Database..";
                    error_mes = "Error! Something is wrong with the Table Database!";
                    TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=[input your own];AccountKey=[ditto];BlobEndpoint=[again];TableEndpoint=[hmm];QueueEndpoint=[yeah];FileEndpoint=[indeed]", "thetable");
                    content = content.Replace("\r\n", " ");
                    string[] delimiters = new string[] {" "};
                    string[] words = content.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    bool first = true;
                    bool true_first = true;
                    string first_temp = "";
                    string last_temp = "";
                    TableEntity temp = new TableEntity(first_temp, last_temp);
                    foreach (string str in words)
                    {
                        if (str.Contains("="))
                        {
                            temp.Add(str.Substring(0, str.IndexOf("=")), str.Substring((str.IndexOf("=") + 1)));
                        }
                        else
                        {
                            if (first)
                            {
                                if (true_first)
                                {
                                    true_first = false;
                                }
                                else
                                {
                                    table.UpsertEntity(temp);
                            }
                                first_temp = str;
                                first = !first;
                            }
                            else
                            {
                                first = !first;
                                last_temp = str;
                                temp = new TableEntity(first_temp, last_temp);
                            }
                        }
                    }
                    table.UpsertEntity(temp);
            }
                catch (Exception)
            {
                Label1.Text = error_mes;
                loading = false;
                return;
            }
            Label1.Text = "Done uploading to Table!";
            loading = false;
        }
    }
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                loading = true;
                Label1.Text = "Currently loading..";
                var client = new WebClient();
                string error_mes = "Error! Table not found!";
                try
                {
                    TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=[input your own];AccountKey=[ditto];BlobEndpoint=[again];TableEndpoint=[hmm];QueueEndpoint=[yeah];FileEndpoint=[indeed]", "thetable");
                    Pageable<TableEntity> queryResultsFilter = table.Query<TableEntity>();
                    error_mes += $"The query returned {queryResultsFilter.Count()} entities";
                    foreach (TableEntity qEntity in queryResultsFilter)
                    {
                        table.DeleteEntity(qEntity.PartitionKey, qEntity.RowKey);
                    }
                }
                catch (Exception)
                {
                    Label1.Text = error_mes;
                    loading = false;
                    return;
                }
                Label1.Text = "Done clearing the Table.";
                loading = false;
            }
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                string first = "";
                string last = "";

                first = TextBox1.Text;
                last = TextBox2.Text;


                if (first.Length == 0 && last.Length == 0)
                {
                    Label1.Text = "ERROR! The first and last name fields must not be empty!";
                    loading = false;
                    return;

                }

                if (!(first.All(c => Char.IsLetterOrDigit(c) || c == '-' || c == '.') && last.All(c => Char.IsLetterOrDigit(c) || c == '-' || c == '.')))
                {
                    Label1.Text = "ERROR! The first and last name fields must only contain letters, dots (.), and hyphes (-)!";
                    loading = false;
                    return;
                }

                if (first.Length == 0)
                {
                    get_all("f", first, last);
                    loading = false;
                    return;
                }

                if (last.Length == 0)
                {
                    get_all("l", first, last);
                    loading = false;
                    return;
                }

                get_all("a", first, last);
                loading = false;
                return;
            }


        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void get_all(string which, string first, string last)
        {
            Label1.Text = "";
            try
            {
                TableClient table = new TableClient("DefaultEndpointsProtocol=https;AccountName=[input your own];AccountKey=[ditto];BlobEndpoint=[again];TableEndpoint=[hmm];QueueEndpoint=[yeah];FileEndpoint=[indeed]", "thetable");
                Pageable<TableEntity> queryResultsFilter = table.Query<TableEntity>();
                if (which == "a")
                {
                    queryResultsFilter = table.Query<TableEntity>(filter: $"PartitionKey eq '" + first + "' and RowKey eq '" + last + "'");
                }
                else if (which == "l")
                {
                    queryResultsFilter = table.Query<TableEntity>(filter: $"PartitionKey eq '" + first + "'");
                }
                else if (which == "f")
                {
                    queryResultsFilter = table.Query<TableEntity>(filter: $"RowKey eq '" + last + "'");
                }

                foreach (TableEntity qEntity in queryResultsFilter)
                {
                    Label1.Text += "<br>{";
                    IDictionary<String, Object> wha = qEntity;
                    foreach(var what in wha.ToList())
                    {
                        Label1.Text += what.Key + ": " + what.Value + "; ";
                    }
                    Label1.Text +=  "}";
                }
            }
            catch (Exception)
            {
                Label1.Text = "An error happened.";
                loading = false;
                return;
            }
            Label1.Text += "<br>Done searching the Table.";
            loading = false;
        }

    }

}