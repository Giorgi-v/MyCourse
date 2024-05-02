using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace MyCourse.Models.Services.Infrastractures
{
    //interfaccia che rappresenta il servizio infrastrutturale
    public interface IDatabaseAccessor
    {
        Task<DataSet> QueryAsync(FormattableString formattableQuery); //metodo che eseguirà una query SELECT passata come parametro
    }
}