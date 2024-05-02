using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using MyCourse.Models.Services.Infrastractures;

namespace MyCourse.Models.Services.Infrastractures
{
    //classe che implementerà concretamente il servizio infrastrutturale
    //si connetterà al db ed eseguirà le query SQL
    public class SqliteDatabaseAccessor : IDatabaseAccessor
    {
        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {
            var queryArguments = formattableQuery.GetArguments(); //recupero tutti i valori iniettati dall'applicazione nella query
            var sqliteParameters = new List<SqliteParameter>();
            for(var i=0; i<queryArguments.Length; i++)
            {
                var parameter = new SqliteParameter(i.ToString(), queryArguments[i]);
                sqliteParameters.Add(parameter);
                queryArguments[i] = "@"+i;  
            }

            string query = formattableQuery.ToString();
            //stabilisco connessione col db Sqlite MyCourse.db
            using(var conn = new SqliteConnection("Data Source=Data/MyCourse.db"))
            {
                await conn.OpenAsync(); //apro la connessione: ADO.NET recupera una nuova connessione dal connection pool

                using(var cmd = new SqliteCommand(query, conn))
                {   
                    //eseguo la query sul db
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        //nuovo oggetto di tipo SqliteDataReader: eseguo query di tipo SELECT
                        //che restituisce una tabella di risultati

                        //nel caso in cui devo eseguire una query che non restituisce tabelle (insert, create, update, delete)
                        //devo usare il metodo ExecuteNonQuery()

                        //nel caso in cui devo eseguire una query che restituisce un numero
                        //devo usare il metodo ExecuteSclar()

                        var dataSet = new DataSet();
                        dataSet.EnforceConstraints = false;
                        do
                        {
                        var dataTable = new DataTable();
                        dataSet.Tables.Add(dataTable);
                        dataTable.Load(reader); //cosi evitiamo di leggere riga per riga i dati dalla tab risultante
                        }while(!reader.IsClosed);
                        /*
                        while(reader.Read()) //devo leggere una riga per volta della tabella restituita
                        {
                            string titolo = (string)reader["Title"]; //recupero il valore del campo Title della tab Courses

                            //ricorda sempre il casting, altrimenti torna un SuperObject
                        }
                        */

                        return dataSet;
                    }   
                }   
            } //chiusura BLOCCHI DI USING: in automatico fa il Dispose(), anche in caso di errori

            //conn.Dispose(); //non lasciare mai la connessione aperta (potresti avere problemi con le query successive!)
        }
    }
}